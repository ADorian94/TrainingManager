using System;
using System.Collections.Generic;
using System.Text;
using TrainingManager.Data;
using TrainingManager.Data.DTO;

namespace TrainingManager.ViewModel.WorkoutManager
{
    public class VeightActivityVM : ViewModelBase
    {
        //PROPERTIES
        private string _activityName;
        public string ActivityName { get => _activityName; set { _activityName = value; OnPropertyChanged(); } }

        public Muscle _mainMuscleGroup;
        public Muscle MainMuscleGroup { get => _mainMuscleGroup; set { _mainMuscleGroup = value; OnPropertyChanged(); } }

        private int _index;
        public int Index { get => _index; set { _index = value; OnPropertyChanged(); } }

        public VeightActivityVM(WeightActivityDTO activity, int index)
        {
            ActivityName = activity.ActivityName;
            MainMuscleGroup = activity.MainMuscleGroup;
            Index = index;
        }

        protected override void InitializeCommands()
        {
        }
    }
}
