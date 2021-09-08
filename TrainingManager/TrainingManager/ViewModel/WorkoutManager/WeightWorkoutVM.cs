using System;
using System.Collections.ObjectModel;

namespace TrainingManager.ViewModel
{
    public class WeightWorkoutVM : ViewModelBase
    {
        protected override void InitializeCommands() { }

        //PROPERTIES
        private int _id;
        public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }

        private Guid _workoutGuid;
        public Guid WorkoutGuid { get => _workoutGuid; set { _workoutGuid = value; OnPropertyChanged(); } }

        private string _workoutName;
        public string WorkoutName { get => _workoutName; set { _workoutName = value; OnPropertyChanged(); } }

        private string _note;
        public string Note { get => _note; set { _note = value; OnPropertyChanged(); } }

        private double _totalWeight;
        public double TotalWeight { get => _totalWeight; set { _totalWeight = value; OnPropertyChanged(); } }

        private double _totalExerciseRounds;
        public double TotalExerciseRounds { get => _totalExerciseRounds; set { _totalExerciseRounds = value; OnPropertyChanged(); } }

        private DateTime _workoutDate = DateTime.Now;
        public DateTime WorkoutDate { get => _workoutDate; set { _workoutDate = value; OnPropertyChanged(); } }

        private Data.DTO.WorkoutType _workoutType;
        public Data.DTO.WorkoutType WorkoutType { get => _workoutType; set { _workoutType = value; OnPropertyChanged(); } }

        private ObservableCollection<WeightExerciseVM> _weightExercises;
        public ObservableCollection<WeightExerciseVM> WeightExercises { get => _weightExercises; set { _weightExercises = value; OnPropertyChanged(); } }
    }
}
