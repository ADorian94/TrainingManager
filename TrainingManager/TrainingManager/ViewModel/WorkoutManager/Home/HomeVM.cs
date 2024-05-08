using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainingManager.Data;
using TrainingManager.Data.DTO;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.LogWriter;
using Xamarin.Forms;

namespace TrainingManager.ViewModel
{
    public class HomeVM : WorkoutManagerBaseVM
    {
        //FIELDS
        private IProfileService _profileService;
        private byte[] _originalImage;
        private Action<PersonalRecordCardVM> _recordSelection;

        public HomeVM(IApiServices apiServices, IProfileService profileService, Action<PersonalRecordCardVM> recordSelection)
        {
            ApiServices = apiServices;
            _profileService = profileService;
            SetupHomeAsync();
            WeightWorkoutMenuSelectedCommand = new DelegateCommand(WeightWorkoutMenuSelectedFunction);
            ProfileSelectedCommand = new DelegateCommand(ProfileSelectedFunction);
            _recordSelection = recordSelection;
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

        private double _weeklyWeight;
        public double WeeklyWeight { get => _weeklyWeight; set { _weeklyWeight = value; OnPropertyChanged(); } }

        private ObservableCollection<(Muscle muscle, double weight)> _movedWeightByMuscle;
        public ObservableCollection<(Muscle muscle, double weight)> MovedWeightByMuscle { get => _movedWeightByMuscle; set { _movedWeightByMuscle = value; OnPropertyChanged(); } }

        private ObservableCollection<PersonalRecordCardVM> _watchedPersonalRecords;
        public ObservableCollection<PersonalRecordCardVM> WatchedPersonalRecords { get => _watchedPersonalRecords; set { _watchedPersonalRecords = value; OnPropertyChanged(); } }

        //ACTIONS
        public Action<Guid> SelectPersonalRecord;

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
                Date = DateTime.Now;
                WellcomeMessage = $"Hello{Environment.NewLine}{await ApiServices.GetNameOfTheUser()}";
                await SetupManagerAsync();
                await InitializeProfilePicture();
                LogHandler.Instance.Nlog.Info("Setup home vm finished.");
            }
            catch (Exception ex)
            {
                OnExeptionOccured(new ExceptionArgs(ex));
            }
        }

        private Task InitPersonalRecords() =>
        Task.Run(async () =>
        {
            var records = await ApiServices.GetWatchedWeightActivitiesAsync();
            WatchedPersonalRecords = new ObservableCollection<PersonalRecordCardVM>(records.Select(x => new PersonalRecordCardVM(x, _recordSelection)));
            LogHandler.Instance.Nlog.Info("Personal records initialized.");
        });

        private Task InitializeWeeklyMuscleDataAsync() =>
        Task.Run(async () =>
        {
            var muslceData = await ApiServices.GetWeeklyMuscleDataAsync();
            MovedWeightByMuscle = new ObservableCollection<(Muscle muscle, double weight)>(muslceData);
            muslceData.Aggregate(0.0, (weight, data) => weight += data.weight, (weight) => WeeklyWeight = weight);
            LogHandler.Instance.Nlog.Info("Weekly muscle data initialized.");
        });

        private Task InitializeProfilePicture() =>
        Task.Run(async () =>
        {
            if (_profileService.IsProfilePictureStored())
            {
                _originalImage = await _profileService.LoadProfilePictureAsync();
                LogHandler.Instance.Nlog.Info("Profile picture loaded from device.");
            }
            else
            {
                _originalImage = await ApiServices.DownloadProfilePictureAsync();
                await _profileService.StoreProfilePictureAsync(_originalImage);
                LogHandler.Instance.Nlog.Info("Profile picture downloaded and stored.");
            }

            ProfilePicture = ImageSource.FromStream(() => new MemoryStream(_originalImage));
        });

        protected override async Task SetupManagerAsync()
        {
            var initializeTasks = new Task[]
            {
                UpdateRecentWorkoutsAsync(),
                InitializeWeeklyMuscleDataAsync(),
                InitPersonalRecords(),
            };

            await Task.WhenAll(initializeTasks);
        }

        protected override void SaveWorkoutFunctionAsync(object obj)
        {
        }

        //PRIVATES
        private Task UpdateRecentWorkoutsAsync() =>
        Task.Run(async () =>
        {
            IEnumerable<WeightWorkoutDTO> recentWorkouts = await ApiServices.GetRecentWeightWorkoutsAsync();
            RecentWorkouts = new ObservableCollection<HistoryItemVM>(recentWorkouts.Select(w => new HistoryItemVM(w)));
        });

        private async void WeightWorkoutMenuSelectedFunction(object obj)
        {
            WeightWorkoutDTO workout = await ApiServices.GetWeightWorkoutAsync((string)obj);
            NewWeightWorkout = WeightWorkoutHelper.WorkoutDTOToVM(workout);
            RecentWorkoutItemSelected?.Invoke(this, new MessageEventArgs(NewWeightWorkout.WorkoutName, NewWeightWorkout.WorkoutGuid.ToString()));
        }

        public async void OnProfileChanged(object sender, EventArgs e) => await InitializeProfilePicture();
        private void ProfileSelectedFunction(object obj) => ProfileSelected?.Invoke(this, EventArgs.Empty);
    }
}
