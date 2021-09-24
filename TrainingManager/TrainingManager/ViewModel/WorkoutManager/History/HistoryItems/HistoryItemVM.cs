using System;
using TrainingManager.Data.DTO;

namespace TrainingManager.ViewModel
{
    public class HistoryItemVM : ViewModelBase
    {
        //PROPERTIES
        private string _workoutName;
        public string WorkoutName { get => _workoutName; set { _workoutName = value; OnPropertyChanged(); } }

        private double _totalWeight;
        public double TotalWeight { get => _totalWeight; set { _totalWeight = value; OnPropertyChanged(); } }

        private DateTime _workoutDate;
        public DateTime WorkoutDate { get => _workoutDate; set { _workoutDate = value; OnPropertyChanged(); } }

        private Guid _workoutGuid;
        public Guid WorkoutGuid { get => _workoutGuid; set { _workoutGuid = value; OnPropertyChanged(); } }

        protected override void InitializeCommands()
        {
        }

        public HistoryItemVM(WeightWorkoutDTO weightWorkout)
        {
            WorkoutName = weightWorkout.WorkoutName;
            TotalWeight = weightWorkout.TotalWeight;
            WorkoutDate = weightWorkout.WorkoutDate;
            WorkoutGuid = weightWorkout.WorkoutGuid;
        }
    }
}
