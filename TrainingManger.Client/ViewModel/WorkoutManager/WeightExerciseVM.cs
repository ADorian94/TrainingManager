using System.Collections.ObjectModel;
using TrainingManager.Data;
using TrainingManager.ViewModel.WorkoutManager;

namespace TrainingManager.ViewModel
{
    public class WeightExerciseVM : ViewModelBase
    {
        public WeightExerciseVM()
        {
            ExerciseName = string.Empty;
            ExerciseNote = string.Empty;
            MainMuscle = Muscle.Unknown;
            InitializeColorVM();
            InitializeMuscleVM();
        }

        public WeightExerciseVM(WeightExerciseVM weightExercise)
        {
            ExerciseName = weightExercise.ExerciseName;
            ExerciseNote = weightExercise.ExerciseNote;
            MainMuscle = weightExercise.MainMuscle;
            InitializeColorVM();
            InitializeMuscleVM();
        }

        protected override void InitializeCommands() { }

        private void InitializeColorVM()
        {
            ColorVM = new EnumeratorVM<MaterialColors>(MaterialColors.None);
            ColorVM.ItemSelected += OnColorSelected;
        }

        private void InitializeMuscleVM()
        {
            MuscleVM = new EnumeratorVM<Muscle>(Muscle.Unknown);
            MuscleVM.ItemSelected += OnMuscleSelected;
        }

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

        private MaterialColors _exerciseColor;
        public MaterialColors ExerciseColor { get => _exerciseColor; set { _exerciseColor = value; OnPropertyChanged(); } }

        private EnumeratorVM<MaterialColors> _colorVM;
        public EnumeratorVM<MaterialColors> ColorVM { get => _colorVM; set { _colorVM = value; OnPropertyChanged(); } }

        private Muscle _mainMuscle;
        public Muscle MainMuscle { get => _mainMuscle; set { _mainMuscle = value; OnPropertyChanged(); } }

        private EnumeratorVM<Muscle> _muscleVM;
        public EnumeratorVM<Muscle> MuscleVM { get => _muscleVM; set { _muscleVM = value; OnPropertyChanged(); } }

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
                   TotalExerciseRounds == e2.TotalExerciseRounds && ExerciseColor == e2.ExerciseColor && AreRoundsEquals(e2);
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

        //EVENT HANDLERS
        private void OnColorSelected(object sender, MaterialColors color) => ExerciseColor = color;
        private void OnMuscleSelected(object sender, Muscle muscle) => MainMuscle = muscle;
    }
}
