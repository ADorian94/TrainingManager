using System;
using System.Collections.ObjectModel;

namespace TrainingManager.ViewModel
{
    public class WeightExerciseVM : ViewModelBase
    {
        //FIELDS
        private int _workoutId;
        private int _roundId;

        public WeightExerciseVM()
        {
            ExerciseName = string.Empty;
            Note = string.Empty;
        }

        public WeightExerciseVM(WeightExerciseVM weightExercise)
        {
            ExerciseName = weightExercise.ExerciseName;
            Note = weightExercise.Note;
        }

        protected override void InitializeCommands() { }

        //PROPERTIES
        private string _exerciseName;
        public string ExerciseName { get => _exerciseName; set { _exerciseName = value; OnPropertyChanged(); } }

        private Guid _exerciseGuid;
        public Guid ExerciseGuid { get => _exerciseGuid; set { _exerciseGuid = value; OnPropertyChanged(); } }

        private string _note;
        public string Note { get => _note; set { _note = value; OnPropertyChanged(); } }

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
