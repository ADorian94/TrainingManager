using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TrainingManager.Data;
using TrainingManager.Data.DTO;
using TrainingManager.Model;
using TrainingManager.ViewModel.WorkoutManager;

namespace TrainingManager.ViewModel
{
    public class WeightActivityManagerVM : ViewModelBase
    {
        //FIELDS
        private IApiServices _apiServices;

        //PROPERTIES
        private DateTime _date;
        public DateTime Date { get => _date; set { _date = value; OnPropertyChanged(); } }

        private ObservableCollection<WeightActivityVM> _activites;
        public ObservableCollection<WeightActivityVM> Activites { get => _activites; set { _activites = value; OnPropertyChanged(); } }

        private WeightActivityVM _selectedActivity;
        public WeightActivityVM SelectedActivity { get => _selectedActivity; set { _selectedActivity = value; OnPropertyChanged(); } }

        private double _selectedActivityWeight;
        public double SelectedActivityWeight { get => _selectedActivityWeight; set { _selectedActivityWeight = value; OnPropertyChanged(); } }

        private int _selectedActivityReps;
        public int SelectedActivityReps { get => _selectedActivityReps; set { _selectedActivityReps = value; OnPropertyChanged(); } }

        private EnumeratorVM<Muscle> _muscleVM;
        public EnumeratorVM<Muscle> MuscleVM { get => _muscleVM; set { _muscleVM = value; OnPropertyChanged(); } }

        private ObservableCollection<WeightRoundVM> _previousRounds;
        public ObservableCollection<WeightRoundVM> PreviousRounds { get => _previousRounds; set { _previousRounds = value; OnPropertyChanged(); } }

        //COMMANDS
        public DelegateCommand ActivitiySelectedCommand { get; private set; }
        public DelegateCommand MuscleSetupCommand { get; private set; }
        public DelegateCommand UpdateExerciseCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }

        //EVENTS
        public event EventHandler WeightActivitySelected;
        public event EventHandler MuscleSetup;
        public event EventHandler NeedToRefresh;

        public WeightActivityManagerVM(IApiServices apiServices)
        {
            Date = DateTime.Now;
            _apiServices = apiServices;
            SetupWeightActivitiesManagerAsync();
        }

        private async void SetupWeightActivitiesManagerAsync()
        {
            var activites = await _apiServices.GetWeightActivitiesAsync();
            int index = 0;
            Activites = new ObservableCollection<WeightActivityVM>(activites.Select(x => new WeightActivityVM(x, ++index)));
        }

        protected override void InitializeCommands()
        {
            ActivitiySelectedCommand = new DelegateCommand(ActivitiySelectedFunction);
            MuscleSetupCommand = new DelegateCommand(MuscleSetupFunctions);
            UpdateExerciseCommand = new DelegateCommand(UpdateExerciseFunction);
            SearchCommand = new DelegateCommand(SearchFunction);
        }

        //COMMAND FUNCTIONS
        private async void UpdateExerciseFunction(object obj)
        {
            await _apiServices.EditWeightActivityAsync(new WeightActivityDTO()
            {
                ActivityGuid = SelectedActivity.Id,
                ActivityName = SelectedActivity.ActivityName,
                MainMuscleGroup = SelectedActivity.MainMuscleGroup,
                IsWatched = SelectedActivity.IsWatched
            });
            SetupWeightActivitiesManagerAsync();
            NeedToRefresh?.Invoke(this, EventArgs.Empty);
        }

        private void MuscleSetupFunctions(object obj)
        {
            InitializeMuscleVM();
            MuscleSetup?.Invoke(this, EventArgs.Empty);
        }

        private async void ActivitiySelectedFunction(object obj)
        {
            var index = int.Parse((string)obj);
            SelectedActivity = new WeightActivityVM(Activites[index - 1]);
            await SetPrOfActvity(SelectedActivity.Id);
            await SetLAtestRoundsOfActivity(SelectedActivity.Id);
            WeightActivitySelected?.Invoke(this, EventArgs.Empty);
        }

        private async Task SetLAtestRoundsOfActivity(Guid id)
        {
            try
            {
                var lastRounds = await _apiServices.GetPreviousRoundsAsync(id, 5);
                PreviousRounds = new ObservableCollection<WeightRoundVM>(lastRounds.Select(x => new WeightRoundVM()
                {
                    Reps = x.Reps,
                    WeightOfExercise = x.WeightOfExercise,
                    RoundNumber = x.RoundNumber,
                    RoundGuid = x.RoundGuid,
                    RoundColor = x.Color
                }));
            }
            catch
            {
                PreviousRounds = new ObservableCollection<WeightRoundVM>();
            }
        }

        private async Task SetPrOfActvity(Guid id)
        {
            try
            {
                var activityDetails = await _apiServices.GetWeightActivityPRAsync(id);
                SelectedActivityReps = activityDetails.RepsOfPersonalRecord;
                SelectedActivityWeight = activityDetails.WeightOfPersonalRecord;
            }
            catch
            {
                SelectedActivityReps = 0;
                SelectedActivityWeight = 0;
            }
        }

        private async void SearchFunction(object obj)
        {
            var keyWords = obj.ToString();
            IEnumerable<WeightActivityDTO> workouts = string.IsNullOrEmpty(keyWords) ?
                await _apiServices.GetWeightActivitiesAsync() :
                await _apiServices.SearchActivityAsync(keyWords);
            int i = 0;

            Activites = new ObservableCollection<WeightActivityVM>(workouts.Select(x => new WeightActivityVM(x, ++i)));
        }

        //PRIVATES
        private void InitializeMuscleVM()
        {
            MuscleVM = new EnumeratorVM<Muscle>(Muscle.Unknown);
            MuscleVM.ItemSelected += OnMuscleSelected;
        }

        //EVENT HANDLERS
        private void OnMuscleSelected(object sender, Muscle muscle)
        {
            SelectedActivity.MainMuscleGroup = muscle;
            MuscleVM.ItemSelected -= OnMuscleSelected;
            MuscleVM = null;
        }

        public async void SetupRecord(PersonalRecordVM record)
        {
            SelectedActivity = new WeightActivityVM(Activites.Single(x => x.Id == record.Id));
            await SetPrOfActvity(SelectedActivity.Id);
            await SetLAtestRoundsOfActivity(SelectedActivity.Id);
            WeightActivitySelected?.Invoke(this, EventArgs.Empty);
        }
    }
}
