using System;
using TrainingManager.Data;
using TrainingManager.Data.DTO;

namespace TrainingManager.ViewModel
{
    public class PersonalRecordVM : ViewModelBase
    {
        //PROPERTIES
        private string _activityName;
        public string ActivityName { get => _activityName; set { _activityName = value; OnPropertyChanged(); } }
        
        public Muscle _mainMuscleGroup;
        public Muscle MainMuscleGroup { get => _mainMuscleGroup; set { _mainMuscleGroup = value; OnPropertyChanged(); } }
        
        public double _weight;
        public double Weight { get => _weight; set { _weight = value; OnPropertyChanged(); } }
        
        private int _reps;
        public int Reps { get => _reps; set { _reps = value; OnPropertyChanged(); } }

        public PersonalRecordVM((WeightActivityDTO activity, double weight, int reps) personalRecord)
        {
            ActivityName = personalRecord.activity.ActivityName;
            MainMuscleGroup = personalRecord.activity.MainMuscleGroup;
            Weight = personalRecord.weight;
            Reps = personalRecord.reps;
        }

        protected override void InitializeCommands()
        {
        }
    }
}
