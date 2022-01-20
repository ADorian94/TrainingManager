using System;

namespace TrainingManager.ViewModel
{
    public class WeightRoundVM : ViewModelBase
    {
        protected override void InitializeCommands() { }

        //EVENTS
        public event EventHandler<double> RoundWeightChanged;

        //PROPERTIES
        private Guid _roundGuid;
        public Guid RoundGuid { get => _roundGuid; set { _roundGuid = value; OnPropertyChanged(); } }

        private double _weightOfExercise;
        public double WeightOfExercise { get => _weightOfExercise; set { _weightOfExercise = value; OnPropertyChanged(); RoundWeightChanged?.Invoke(this, WeightOfExercise * Reps); } }

        private int _reps;
        public int Reps { get => _reps; set { _reps = value; OnPropertyChanged(); RoundWeightChanged?.Invoke(this, WeightOfExercise * Reps); } }

        private int _roundNumber;
        public int RoundNumber { get => _roundNumber; set { _roundNumber = value; OnPropertyChanged(); } }

        public static bool operator ==(WeightRoundVM r1, WeightRoundVM r2)
        {
            if ((object)r1 == null)
                return (object)r2 == null;

            return r1.Equals(r2);
        }

        public static bool operator !=(WeightRoundVM r1, WeightRoundVM r2) => !(r1 == r2);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var w2 = (WeightRoundVM)obj;

            return WeightOfExercise == w2.WeightOfExercise && Reps == w2.Reps && RoundNumber == w2.RoundNumber;
        }

        public override int GetHashCode() => WeightOfExercise.GetHashCode() ^ Reps.GetHashCode() ^ RoundNumber.GetHashCode();
    }
}
