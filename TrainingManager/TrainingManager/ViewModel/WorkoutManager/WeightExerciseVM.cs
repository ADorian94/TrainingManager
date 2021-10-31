using System;
using System.Collections.ObjectModel;

namespace TrainingManager.ViewModel
{
    public class WeightExerciseVM : ViewModelBase
    {
        public WeightExerciseVM()
        {
            ExerciseName = string.Empty;
            ExerciseNote = string.Empty;
        }

        public WeightExerciseVM(WeightExerciseVM weightExercise)
        {
            ExerciseName = weightExercise.ExerciseName;
            ExerciseNote = weightExercise.ExerciseNote;
        }

        protected override void InitializeCommands() { }

        //PROPERTIES
        private string _exerciseName;
        public string ExerciseName { get => _exerciseName; set { _exerciseName = value; OnPropertyChanged(); } }

        private Guid _exerciseGuid;
        public Guid ExerciseGuid { get => _exerciseGuid; set { _exerciseGuid = value; OnPropertyChanged(); } }

        private string _exerciseNote;
        public string ExerciseNote { get => _exerciseNote; set { _exerciseNote = value; OnPropertyChanged(); } }

        private double _totalExerciseWeight;
        public double TotalExerciseWeight { get => _totalExerciseWeight; set { _totalExerciseWeight = value; OnPropertyChanged(); } }

        private double _totalExerciseRounds;
        public double TotalExerciseRounds { get => _totalExerciseRounds; set { _totalExerciseRounds = value; OnPropertyChanged(); } }

        private ObservableCollection<WeightRoundVM> _weightRounds;
        public ObservableCollection<WeightRoundVM> WeightRounds { get => _weightRounds; set { _weightRounds = value; OnPropertyChanged(); } }

        public void CountTotalWeightOfExercise()
        {
            if (WeightRounds != null)
            {
                TotalExerciseWeight = 0.0;

                foreach (var round in WeightRounds)
                    TotalExerciseWeight += round.WeightOfExercise * round.Reps;
            }
        }
    }
}
