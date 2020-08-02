using System;
using System.Collections.ObjectModel;
using TrainingManager.Model.Workouts.WeightWorkout;

namespace TrainingManager.ViewModel
{
    public class WeightWorkoutManagerVM : ViewModelBase
    {
        private string _today;
        public string Today
        {
            get => _today;
            set
            {
                _today = value;
                OnPropertyChanged();
            }
        }

        private string _exerciseName;
        public string ExerciseName
        {
            get => _exerciseName;
            set
            {
                _exerciseName = value;
                OnPropertyChanged();
            }
        }

        private double _exerciseWeight;
        public double ExerciseWeight
        {
            get => _exerciseWeight;
            set
            {
                _exerciseWeight = value;
                OnPropertyChanged();
            }
        }

        private int _exerciseReps;
        public int ExerciseReps
        {
            get => _exerciseReps;
            set
            {
                _exerciseReps = value;
                OnPropertyChanged();
            }
        }

        private string _exercieNote;
        public string ExercieNote
        {
            get => _exercieNote;
            set
            {
                _exercieNote = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<WeightExercise> _weightWorkout;
        public ObservableCollection<WeightExercise> WeightWorkout
        {
            get => _weightWorkout;
            set
            {
                _weightWorkout = value;
                OnPropertyChanged();
            }
        }

        //COMMANDS
        public DelegateCommand OpenAddWeightExerciseCommand { get; private set; }
        public DelegateCommand AddWeightExerciseToWorkoutCommand { get; private set; }
        public DelegateCommand OpenNoteEditorCommand { get; private set; }
        public DelegateCommand OpenTrainingLogOpenCommand { get; private set; }


        //EVENTS
        public event EventHandler OpenAddWeightExercise;
        public event EventHandler CloseAddWeightExercise;
        public event EventHandler OpenNoteEditor;
        public event EventHandler OpenTrainingLog;

        public WeightWorkoutManagerVM()
        {
            InitializeCommands();
            Today = DateTime.Now.Date.ToString("yyyy.MM.dd");
            WeightWorkout = new ObservableCollection<WeightExercise>();
        }

        protected override void InitializeCommands()
        {
            OpenAddWeightExerciseCommand = new DelegateCommand(OpenAddWeightExerciseFunction);
            AddWeightExerciseToWorkoutCommand = new DelegateCommand(AddWeightExerciseToWorkoutFunction);
            OpenNoteEditorCommand = new DelegateCommand(OpenNoteEditorFuncton);
            OpenTrainingLogOpenCommand = new DelegateCommand(OpenTrainingLogOpenFunction);
        }

        private void AddWeightExerciseToWorkoutFunction(object obj)
        {
            var newExercise = new WeightExercise()
            {
                ExerciseDate = DateTime.Now,
                ExerciseName = ExerciseName,
                WeightOfExercise = ExerciseWeight,
                Reps = ExerciseReps,
                Note = ExercieNote,
            };

            WeightWorkout.Add(newExercise);
            CloseAddWeightExercise?.Invoke(this, null);
        }

        private void OpenAddWeightExerciseFunction(object obj) => OpenAddWeightExercise?.Invoke(this, null);
        private void OpenNoteEditorFuncton(object obj) => OpenNoteEditor?.Invoke(this, null);
        private void OpenTrainingLogOpenFunction(object obj) => OpenTrainingLog?.Invoke(this, null);
    }
}
