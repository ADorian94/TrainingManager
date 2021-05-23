using System;
using System.Collections.ObjectModel;
using System.Linq;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.Workouts.WeightWorkout;

namespace TrainingManager.ViewModel
{
    public class WeightWorkoutHistoryManagerVM : ViewModelBase
    {
        //FIELDS
        private IWeightWorkoutManager _weightWorkoutManager;

        //EVENTS
        public event EventHandler OpenWorkoutDetails;

        public WeightWorkoutHistoryManagerVM()
        {
            //_weightWorkoutManager = new WeightWorkoutManager();
            //WeightWorkoutsHistory = new ObservableCollection<WeightWorkout>(_weightWorkoutManager.GetWorkouts().Where(x => x.WorkoutDate.Date != DateTime.Now.Date));
            InitializeCommands();
        }

        protected override void InitializeCommands()
        {
            OpenWorkoutDetailsCommand = new DelegateCommand(OpenWorkoutDetailsFunction);
        }

        //PRPERTIES
        private WeightWorkout _workoutDetails;
        public WeightWorkout WorkoutDetails
        {
            get => _workoutDetails;
            set
            {
                _workoutDetails = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<WeightExercise> _weightWorkoutList;
        public ObservableCollection<WeightExercise> WeightWorkoutList
        {
            get => _weightWorkoutList;
            set
            {
                _weightWorkoutList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<WeightWorkout> _weightWorkoutsHistory;
        public ObservableCollection<WeightWorkout> WeightWorkoutsHistory
        {
            get => _weightWorkoutsHistory;
            set
            {
                _weightWorkoutsHistory = value;
                OnPropertyChanged();
            }
        }

        //DELEGATE COMMANDS
        public DelegateCommand OpenWorkoutDetailsCommand { get; set; }

        //DELEGATE COMMAND FUNCTIONS
        private void OpenWorkoutDetailsFunction(object obj)
        {
            WorkoutDetails = _weightWorkoutManager.GetWorkoutById(new Guid((string)obj));
            WeightWorkoutList = new ObservableCollection<WeightExercise>(WorkoutDetails.Exercises);
            OpenWorkoutDetails?.Invoke(this, null);
        }

        //PRIVATES

    }
}
