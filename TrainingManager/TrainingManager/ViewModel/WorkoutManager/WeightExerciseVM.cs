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

        public static bool operator ==(WeightExerciseVM e1, WeightExerciseVM e2)
        {
            if ((object)e1 == null)
                return (object)e2 == null;

            return e1.Equals(e2);
        }

        public static bool operator !=(WeightExerciseVM e1, WeightExerciseVM e2) => !(e1 == e2);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var e2 = (WeightExerciseVM)obj;

            return ExerciseName == e2.ExerciseName && ExerciseNote == e2.ExerciseNote && TotalExerciseWeight == e2.TotalExerciseWeight &&
                   TotalExerciseRounds == e2.TotalExerciseRounds && AreRoundsEquals(e2);
        }

        public override int GetHashCode() => ExerciseName.GetHashCode() ^ ExerciseNote.GetHashCode() ^ TotalExerciseWeight.GetHashCode() ^ TotalExerciseRounds.GetHashCode();

        private bool AreRoundsEquals(WeightExerciseVM e2)
        {
            bool result = WeightRounds.Count == e2.WeightRounds.Count;
            int index = 0;

            while (result && index < WeightRounds.Count)
            {
                result = WeightRounds[index] == e2.WeightRounds[index];
                ++index;
            }

            return result;
        }
    }
}
