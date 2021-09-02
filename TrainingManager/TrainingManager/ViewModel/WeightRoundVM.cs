using System;

namespace TrainingManager.ViewModel
{
    public class WeightRoundVM : ViewModelBase
    {
        protected override void InitializeCommands() { }

        //PROPERTIES
        private Guid _roundGuid;
        public Guid RoundGuid { get => _roundGuid; set { _roundGuid = value; OnPropertyChanged(); } }

        private double _weightOfExercise;
        public double WeightOfExercise { get => _weightOfExercise; set { _weightOfExercise = value; OnPropertyChanged(); } }

        private int _reps;
        public int Reps { get => _reps; set { _reps = value; OnPropertyChanged(); } }

        private int _roundNumber;
        public int RoundNumber { get => _roundNumber; set { _roundNumber = value; OnPropertyChanged(); } }
    }
}
