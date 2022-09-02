using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace TrainingManager.ViewModel
{
    public class WeightWorkoutVM : ViewModelBase
    {
        protected override void InitializeCommands() { }

        public WeightWorkoutVM()
        {
            Note = string.Empty;
            TotalExerciseRounds = 0.0;
            TotalWeight = 0.0;
            WorkoutDate = DateTime.Now.ToUniversalTime();
            WorkoutGuid = Guid.NewGuid();
            WorkoutName = string.Empty;
            WorkoutType = Data.DTO.WorkoutType.WeightWorkout;
            WeightExercises = new ObservableCollection<WeightExerciseVM>();
        }

        public WeightWorkoutVM(DateTime dateTime)
        {
            Note = string.Empty;
            TotalExerciseRounds = 0.0;
            TotalWeight = 0.0;
            WorkoutDate = dateTime.ToUniversalTime();
            WorkoutGuid = Guid.NewGuid();
            WorkoutName = string.Empty;
            WorkoutType = Data.DTO.WorkoutType.WeightWorkout;
            WeightExercises = new ObservableCollection<WeightExerciseVM>();
        }

        public WeightWorkoutVM(WeightWorkoutVM baseWorkout)
        {
            Id = baseWorkout.Id;
            Note = baseWorkout.Note;
            TotalExerciseRounds = baseWorkout.WeightExercises.Count;
            TotalWeight = baseWorkout.TotalWeight;
            WorkoutDate = baseWorkout.WorkoutDate.ToUniversalTime();
            WorkoutGuid = baseWorkout.WorkoutGuid;
            WorkoutName = baseWorkout.WorkoutName;
            WorkoutType = baseWorkout.WorkoutType;
            WeightExercises = new ObservableCollection<WeightExerciseVM>(
                baseWorkout.WeightExercises.Select(exercise => new WeightExerciseVM()
                {
                    ExerciseNote = exercise.ExerciseNote,
                    ExerciseGuid = exercise.ExerciseGuid,
                    ExerciseName = exercise.ExerciseName,
                    TotalExerciseRounds = exercise.WeightRounds.Count,
                    TotalExerciseWeight = exercise.TotalExerciseWeight,
                    ExerciseColor = exercise.ExerciseColor,
                    WeightRounds = new ObservableCollection<WeightRoundVM>(
                        exercise.WeightRounds.Select(round => new WeightRoundVM()
                        {
                            Reps = round.Reps,
                            RoundGuid = round.RoundGuid,
                            RoundNumber = round.RoundNumber,
                            WeightOfExercise = round.WeightOfExercise,
                            RoundColor = round.RoundColor
                        }))
                }));
        }

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

        public static bool operator ==(WeightWorkoutVM w1, WeightWorkoutVM w2)
        {
            if ((object)w1 == null)
                return (object)w2 == null;

            return w1.Equals(w2);
        }

        public static bool operator !=(WeightWorkoutVM w1, WeightWorkoutVM w2) => !(w1 == w2);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var w2 = (WeightWorkoutVM)obj;

            return WorkoutName == w2.WorkoutName && Note == w2.Note && TotalWeight == w2.TotalWeight &&
                WorkoutDate.Date == w2.WorkoutDate.Date && WorkoutType == w2.WorkoutType && AreExercisesEquals(w2);
        }

        public override int GetHashCode() =>
            WorkoutName.GetHashCode() ^ Note.GetHashCode() ^ TotalWeight.GetHashCode() ^ TotalExerciseRounds.GetHashCode() ^
            WorkoutDate.GetHashCode() ^ WorkoutType.GetHashCode() ^ WeightExercises.GetHashCode();

        private bool AreExercisesEquals(WeightWorkoutVM w2)
        {
            bool result = WeightExercises.Count == w2.WeightExercises.Count;
            int index = 0;

            while (result && index < WeightExercises.Count)
            {
                result = WeightExercises[index] == w2.WeightExercises[index];
                ++index;
            }

            return result;
        }
    }
}
