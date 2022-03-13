using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainingManager.Data.DTO;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
using Xamarin.Forms;

namespace TrainingManager.ViewModel
{
    public class HomeVM : WorkoutManagerBaseVM
    {
        //FIELDS
        private IProfileService _profileService;
        private byte[] _originalImage;

        public HomeVM(IApiServices apiServices, IProfileService profileService)
        {
            ApiServices = apiServices;
            _profileService = profileService;
            SetupHomeAsync();
            WeightWorkoutMenuSelectedCommand = new DelegateCommand(WeightWorkoutMenuSelectedFunction);
            ProfileSelectedCommand = new DelegateCommand(ProfileSelectedFunction);
        }

        //PROPERTIES
        private ObservableCollection<HistoryItemVM> _recentWorkouts;
        public ObservableCollection<HistoryItemVM> RecentWorkouts { get => _recentWorkouts; set { _recentWorkouts = value; OnPropertyChanged(); } }

        private ImageSource _profilePicture;
        public ImageSource ProfilePicture { get => _profilePicture; set { _profilePicture = value; OnPropertyChanged(); } }

        private string _wellcomeMessage;
        public string WellcomeMessage { get => _wellcomeMessage; set { _wellcomeMessage = value; OnPropertyChanged(); } }

        private DateTime _date;
        public DateTime Date { get => _date; set { _date = value; OnPropertyChanged(); } }

        //COMMANDS
        public DelegateCommand WeightWorkoutMenuSelectedCommand { get; private set; }
        public DelegateCommand ProfileSelectedCommand { get; private set; }

        //EVENTS
        public event EventHandler<MessageEventArgs> RecentWorkoutItemSelected;
        public event EventHandler ProfileSelected;

        private async void SetupHomeAsync()
        {
            try
            {
                var initializeTasks = new Task[]
                {
                    InitializeProfilePicture(),
                    UpdateRecentWorkoutsAsync(),
                };

                Date = DateTime.Now;
                WellcomeMessage = $"Hello{Environment.NewLine}{await ApiServices.GetNameOfTheUser()}";

                await Task.WhenAll(initializeTasks);
            }
            catch (Exception ex)
            {
                OnExeptionOccured(new ExceptionArgs(ex));
            }
        }

        private async Task InitializeProfilePicture()
        {
            if (_profileService.IsProfilePictureStored())
                _originalImage = await _profileService.LoadProfilePictureAsync();
            else
            {
                _originalImage = await ApiServices.DownloadProfilePictureAsync();
                await _profileService.StoreProfilePictureAsync(_originalImage);
            }

            ProfilePicture = ImageSource.FromStream(() => new MemoryStream(_originalImage));
        }

        public override async void RefreshWorkouts(object sender, EventArgs e) => await UpdateRecentWorkoutsAsync();

        protected override void SaveTodayWorkoutFunctionAsync(object obj)
        {
        }

        //PRIVATES
        private async Task UpdateRecentWorkoutsAsync()
        {
            IEnumerable<WeightWorkoutDTO> recentWorkouts = await ApiServices.GetRecentWeightWorkoutsAsync();
            RecentWorkouts = new ObservableCollection<HistoryItemVM>(recentWorkouts.Select(w => new HistoryItemVM(w)));
        }

        private async void WeightWorkoutMenuSelectedFunction(object obj)
        {
            var workouts = new List<WeightWorkoutDTO>(await ApiServices.GetWeightWorkoutsAsync());

            if (workouts.Any(x => x.WorkoutGuid.ToString() == ((string)obj)))
            {
                WeightWorkoutDTO workout = workouts.Single(x => x.WorkoutGuid.ToString() == ((string)obj));

                NewWeightWorkout = new WeightWorkoutVM()
                {
                    Id = workout.Id,
                    WorkoutName = workout.WorkoutName,
                    WorkoutDate = workout.WorkoutDate,
                    TotalWeight = workout.TotalWeight,
                    WorkoutGuid = workout.WorkoutGuid,
                    WorkoutType = workout.WorkoutType,
                    Note = workout.Note,
                    WeightExercises = new ObservableCollection<WeightExerciseVM>(workout.WeightExercisesDto.Select(x => new WeightExerciseVM()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        ExerciseName = x.ExerciseName,
                        ExerciseNote = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        TotalExerciseRounds = x.WeightRoundsDto.Count(),
                        WeightRounds = new ObservableCollection<WeightRoundVM>(x.WeightRoundsDto.Select(y => new WeightRoundVM()
                        {
                            RoundGuid = y.RoundGuid,
                            RoundNumber = y.RoundNumber,
                            Reps = y.Reps,
                            WeightOfExercise = y.WeightOfExercise
                        })),
                    }))
                };

                RecentWorkoutItemSelected?.Invoke(this, new MessageEventArgs(NewWeightWorkout.WorkoutName, NewWeightWorkout.WorkoutGuid.ToString()));
            }
        }

        public async void OnProfileChanged(object sender, EventArgs e) => await InitializeProfilePicture();
        private void ProfileSelectedFunction(object obj) => ProfileSelected?.Invoke(this, EventArgs.Empty);
    }
}
