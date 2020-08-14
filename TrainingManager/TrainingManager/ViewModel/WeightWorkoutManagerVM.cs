using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.Workouts.WeightWorkout;

namespace TrainingManager.ViewModel
{
    public class WeightWorkoutManagerVM : ViewModelBase
    {
        private IWeightWorkoutManager _weightWorkoutManager;

        private Guid _editExerciseGuid;

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
        public string ExerciseNote
        {
            get => _exercieNote;
            set
            {
                _exercieNote = value;
                OnPropertyChanged();
            }
        }

        private double _totalWeight;
        public double TotalWeight
        {
            get => _totalWeight;
            set
            {
                _totalWeight = value;
                OnPropertyChanged();
            }
        }

        private WeightWorkout _todayWeightWorkout;
        public WeightWorkout TodayWeightWorkout
        {
            get => _todayWeightWorkout;
            set
            {
                _todayWeightWorkout = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<WeightExercise> _weightWorkoutList;
        public ObservableCollection<WeightExercise> WeightWorkoutList
        {
            get => _weightWorkoutList;
            set
            {
                _weightWorkoutList = value;
                OnPropertyChanged();
            }
        }

        //COMMANDS
        public DelegateCommand SaveTodayWorkoutCommand { get; private set; }
        public DelegateCommand OpenAddWeightExerciseCommand { get; private set; }
        public DelegateCommand AddWeightExerciseToWorkoutCommand { get; private set; }
        public DelegateCommand OpenNoteEditorCommand { get; private set; }
        public DelegateCommand OpenTrainingLogOpenCommand { get; private set; }
        public DelegateCommand OpenHistoryViewOpenCommand { get; private set; }
        public DelegateCommand WeightExerciseMenuSelectedCommand { get; private set; }

        //EVENTS
        public event EventHandler OpenAddWeightExercise;
        public event EventHandler OpenEditWeightExercise;
        public event EventHandler CloseAddWeightExercise;
        public event EventHandler OpenNoteEditor;
        public event EventHandler OpenTrainingLog;
        public event EventHandler OpenHistoryView;
        public event EventHandler<MessageEventArgs> WeightExerciseMenuSelected;

        public WeightWorkoutManagerVM()
        {
            _weightWorkoutManager = new WeightWorkoutManager();
            InitializeCommands();
            WeightWorkoutList = new ObservableCollection<WeightExercise>();
            SetTodayWeightWorkout();
        }

        private void SetTodayWeightWorkout()
        {
            if (_weightWorkoutManager.GetWorkouts().Any(x => x.WorkoutDate.Date == DateTime.Now.Date))
            {
                UpdateTodayWeightWorkout();
                UpdateWeightWorkout();
            }
            else
                TodayWeightWorkout = new WeightWorkout()
                {
                    WorkoutId = Guid.NewGuid(),
                    WorkoutDate = DateTime.Now,
                    Exercises = new List<WeightExercise>(),
                    TotalWeight = 0.0,
                    WorkoutType = WorkoutType.WeightWorkout,
                    WorkoutName = DateTime.Now.Date.ToString("yyyy.MM.dd"),
                };
        }

        protected override void InitializeCommands()
        {
            SaveTodayWorkoutCommand = new DelegateCommand(SaveTodayWorkoutFunction);
            OpenAddWeightExerciseCommand = new DelegateCommand(OpenAddWeightExerciseFunction);
            AddWeightExerciseToWorkoutCommand = new DelegateCommand(AddWeightExerciseToWorkoutFunction);
            OpenNoteEditorCommand = new DelegateCommand(OpenNoteEditorFuncton);
            OpenTrainingLogOpenCommand = new DelegateCommand(OpenTrainingLogOpenFunction);
            OpenHistoryViewOpenCommand = new DelegateCommand(OpenHistoryViewOpenFunction);
            WeightExerciseMenuSelectedCommand = new DelegateCommand(WeightExerciseMenuSelectedFunction);
        }

        private void SaveTodayWorkoutFunction(object obj)
        {
            if (_weightWorkoutManager.GetWorkouts().Any(x => x.WorkoutDate.Date == DateTime.Now.Date))
            {
                WeightWorkout tmpTodayWorkout = _weightWorkoutManager.GetWorkouts().FirstOrDefault(x => x.WorkoutDate.Date == DateTime.Now.Date);
                _weightWorkoutManager.DeleteWorkoutById(tmpTodayWorkout.WorkoutIdString);
            }

            TodayWeightWorkout.TotalWeight = TotalWeight;
            _weightWorkoutManager.AddNewWorkout(TodayWeightWorkout);
            _weightWorkoutManager.SaveWorkoutById(TodayWeightWorkout.WorkoutId, WorkoutType.WeightWorkout);
        }

        private void AddWeightExerciseToWorkoutFunction(object obj)
        {
            if (!_weightWorkoutManager.IsWorkoutExist(TodayWeightWorkout.WorkoutId))
                _weightWorkoutManager.AddNewWorkout(TodayWeightWorkout);

            if (_weightWorkoutManager.GetWorkoutExercisesById(TodayWeightWorkout.WorkoutId).Any(x => x.ExerciseId == _editExerciseGuid))
                _weightWorkoutManager.DeleteExerciseFromWorkoutById(TodayWeightWorkout.WorkoutId, _editExerciseGuid);

            var newExercise = new WeightExercise()
            {
                ExerciseId = Guid.NewGuid(),
                ExerciseDate = DateTime.Now,
                ExerciseName = ExerciseName,
                WeightOfExercise = ExerciseWeight,
                Reps = ExerciseReps,
                Note = ExerciseNote,
            };

            _weightWorkoutManager.AddExerciseToWorkoutById(TodayWeightWorkout.WorkoutId, newExercise);
            UpdateTodayWeightWorkout();
            TotalWeight = CountTotalWeightOfWorkout();
            UpdateWeightWorkout();
            CloseAddWeightExercise?.Invoke(this, null);
        }

        private void UpdateTodayWeightWorkout()
        {
            //TodayWeightWorkout = new WeightWorkout();
            TodayWeightWorkout = _weightWorkoutManager.GetWorkouts().FirstOrDefault(x => x.WorkoutDate.Date == DateTime.Now.Date);
            TotalWeight = CountTotalWeightOfWorkout();
        }

        internal void DeleteExercise(string stringGuid)
        {
            _weightWorkoutManager.DeleteExerciseFromWorkoutById(TodayWeightWorkout.WorkoutId, new Guid(stringGuid));
            UpdateTodayWeightWorkout();
            UpdateWeightWorkout();
        }

        internal void EditExercise(string stringGuid)
        {
            WeightExercise exercise = _weightWorkoutManager.GetExerciseInWorkoutById(TodayWeightWorkout.WorkoutId, new Guid(stringGuid));
            ExerciseName = exercise.ExerciseName;
            ExerciseWeight = exercise.WeightOfExercise;
            ExerciseReps = exercise.Reps;
            ExerciseNote = exercise.Note;
            _editExerciseGuid = exercise.ExerciseId;
            OpenEditWeightExercise?.Invoke(this, null);
        }

        private void UpdateWeightWorkout() => WeightWorkoutList = new ObservableCollection<WeightExercise>(TodayWeightWorkout.Exercises);

        private void OpenAddWeightExerciseFunction(object obj)
        {
            ExerciseName = string.Empty;
            ExerciseWeight = 0.0;
            ExerciseReps = 0;
            ExerciseNote = string.Empty;
            OpenAddWeightExercise?.Invoke(this, null);
        }

        private void OpenNoteEditorFuncton(object obj) => OpenNoteEditor?.Invoke(this, null);
        private void OpenTrainingLogOpenFunction(object obj) => OpenTrainingLog?.Invoke(this, null);
        private void OpenHistoryViewOpenFunction(object obj) => OpenHistoryView?.Invoke(this, null);
        private void WeightExerciseMenuSelectedFunction(object obj) => WeightExerciseMenuSelected?.Invoke(this, new MessageEventArgs((string)obj));
        private double CountTotalWeightOfWorkout() => _weightWorkoutManager.GetTotalWeightById(TodayWeightWorkout.WorkoutId);
    }
}
