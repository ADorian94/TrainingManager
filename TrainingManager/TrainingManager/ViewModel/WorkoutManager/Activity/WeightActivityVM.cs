using System;
using TrainingManager.Data;
using TrainingManager.Data.DTO;

namespace TrainingManager.ViewModel.WorkoutManager
{
    public class WeightActivityVM : ViewModelBase
    {
        //PROPERTIES
        public Guid Id { get; private set; } 

        private string _activityName;
        public string ActivityName { get => _activityName; set { _activityName = value; OnPropertyChanged(); } }

        public Muscle _mainMuscleGroup;
        public Muscle MainMuscleGroup { get => _mainMuscleGroup; set { _mainMuscleGroup = value; OnPropertyChanged(); } }

        private int _index;
        public int Index { get => _index; set { _index = value; OnPropertyChanged(); } }

        private bool _isWatched;
        public bool IsWatched { get => _isWatched; set { _isWatched = value; OnPropertyChanged(); } }

        public WeightActivityVM(WeightActivityDTO activity, int index)
        {
            Id = activity.ActivityGuid;
            ActivityName = activity.ActivityName;
            MainMuscleGroup = activity.MainMuscleGroup;
            Index = index;
            IsWatched = activity.IsWatched;
        }

        public WeightActivityVM(WeightActivityVM activity)
        {
            Id = activity.Id;
            ActivityName = activity.ActivityName;
            MainMuscleGroup = activity.MainMuscleGroup;
            Index = activity.Index;
            IsWatched = activity.IsWatched;
        }

        protected override void InitializeCommands()
        {
        }
    }
}
