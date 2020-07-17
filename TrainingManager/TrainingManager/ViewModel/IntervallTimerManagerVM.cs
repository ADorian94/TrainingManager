using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.Workouts.IntervallWorkout;

namespace TrainingManager.ViewModel
{
    public class IntervallTimerManagerVM : ViewModelBase
    {
        private IIntervallTimerManager _intervallTimerManager;
        private IWorkoutManager<IntervallWorkout, IntervallExercise> _intervallWorkoutManager;
        public event EventHandler OpenNewIntervallPage;
        public event EventHandler CloseAddNewIntervallPage;
        public event EventHandler OpenNewIntervallWorkoutPage;
        public event EventHandler CloseNewIntervallWorkoutPage;
        public event EventHandler WorkoutSelected;

        private ObservableCollection<IntervallExercise> _activeWorkoutIntervalls;
        public ObservableCollection<IntervallExercise> ActiveWorkoutIntervalls
        {
            get => _activeWorkoutIntervalls;
            set
            {
                _activeWorkoutIntervalls = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<IntervallWorkout> _allIntervallWorkouts;
        public ObservableCollection<IntervallWorkout> AllIntervallWorkouts
        {
            get => _allIntervallWorkouts;
            set
            {
                _allIntervallWorkouts = value;
                OnPropertyChanged();
            }
        }

        private IntervallWorkout _newIntervallWorkout;
        public IntervallWorkout NewIntervallWorkout
        {
            get => _newIntervallWorkout;
            set
            {
                _newIntervallWorkout = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<IntervallExercise> _neweWorkoutIntervalls;
        public ObservableCollection<IntervallExercise> NewWorkoutIntervalls
        {
            get => _neweWorkoutIntervalls;
            set
            {
                _neweWorkoutIntervalls = value;
                OnPropertyChanged();
            }
        }

        private IntervallExercise _newIntervall;
        public IntervallExercise NewIntervall
        {
            get => _newIntervall;
            set
            {
                _newIntervall = value;
                OnPropertyChanged();
            }
        }

        private IntervallWorkout _selectedWorkout;
        public IntervallWorkout SelectedWorkout
        {
            get => _selectedWorkout;
            set
            {
                _selectedWorkout = value;
                OnPropertyChanged();
                WorkoutSelected?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool _timerStarted;
        public bool TimerStarted
        {
            get => _timerStarted;
            set
            {
                _timerStarted = value;
                OnPropertyChanged();
            }
        }

        private bool _timerStoped;
        public bool TimerStopped
        {
            get => _timerStoped;
            set
            {
                _timerStoped = value;
                OnPropertyChanged();
            }
        }

        private string _newIntervallName;
        public string NewIntervallName
        {
            get => _newIntervallName;
            set
            {
                _newIntervallName = value;
                OnPropertyChanged();
            }
        }

        private int _newIntervallTime;
        public int NewIntervallTime
        {
            get => _newIntervallTime;
            set
            {
                _newIntervallTime = value;
                OnPropertyChanged();
            }
        }

        private string _newWorkoutName;
        public string NewWorkoutName
        {
            get => _newWorkoutName;
            set
            {
                _newWorkoutName = value;
                OnPropertyChanged();
            }
        }

        private string _activeIntervallName;
        public string ActiveIntervallName
        {
            get => _activeIntervallName;
            set
            {
                _activeIntervallName = value;
                OnPropertyChanged();
            }
        }

        private int _activeIntervallTime;
        public int ActiveIntervallTime
        {
            get => _activeIntervallTime;
            set
            {
                _activeIntervallTime = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand StartIntervallTimerCommand { get; private set; }
        public DelegateCommand StopIntervallTimerCommand { get; private set; }
        public DelegateCommand PauseIntervallTimerCommand { get; private set; }
        public DelegateCommand OpenNewIntervallPageCommand { get; private set; }
        public DelegateCommand AddNewIntervallCommand { get; private set; }
        public DelegateCommand OpenNewIntervallWorkoutCommand { get; private set; }
        public DelegateCommand AddNewIntervallWorkoutCommand { get; private set; }
        public DelegateCommand CardButtonsCommand { get; private set; }

        public IntervallTimerManagerVM()
        {
            _intervallTimerManager = new IntervallTimerManager();
            _intervallTimerManager.IntervallTimeChanged += new EventHandler(OnIntervallTimeChanged);
            _intervallTimerManager.IntervallChanged += new EventHandler(OnIntervallChanged);
            _intervallTimerManager.IntervallFinished += new EventHandler(OnIntervallFinished);
            TimerStarted = false;
            TimerStopped = true;
            _intervallWorkoutManager = new IntervallWorkoutManager();
            AllIntervallWorkouts = new ObservableCollection<IntervallWorkout>(_intervallWorkoutManager.GetWorkouts());
            NewWorkoutIntervalls = new ObservableCollection<IntervallExercise>();
            InitializeCommands();
        }

        protected override void InitializeCommands()
        {
            StartIntervallTimerCommand = new DelegateCommand(StartIntervallTimer);
            StopIntervallTimerCommand = new DelegateCommand(StopIntervallTimer);
            PauseIntervallTimerCommand = new DelegateCommand(PauseIntervallTimer);
            OpenNewIntervallWorkoutCommand = new DelegateCommand(OpenNewIntervallWorkout);
            AddNewIntervallWorkoutCommand = new DelegateCommand(AddNewIntervallWorkout);
            OpenNewIntervallPageCommand = new DelegateCommand(OpenNewIntervall);
            AddNewIntervallCommand = new DelegateCommand(AddNewIntervall);
            CardButtonsCommand = new DelegateCommand(CardButtons);
        }

        private void CardButtons(object obj) => OnMessageApplication((string)obj);

        private void OnIntervallFinished(object sender, EventArgs e)
        {
            ActiveIntervallTime = _intervallTimerManager.GetRemainingTimeOfActiveIntervall();
            ActiveIntervallName = _intervallTimerManager.GetNameOfActiveintervall();
            TimerStarted = false;
            TimerStopped = true;
        }

        private void OnIntervallChanged(object sender, EventArgs e)
        {
            ActiveIntervallTime = _intervallTimerManager.GetRemainingTimeOfActiveIntervall();
            ActiveIntervallName = _intervallTimerManager.GetNameOfActiveintervall();
        }

        private void OnIntervallTimeChanged(object sender, EventArgs e) => ActiveIntervallTime = _intervallTimerManager.GetRemainingTimeOfActiveIntervall();

        private void StartIntervallTimer(object obj)
        {
            try
            {
                _intervallTimerManager.SetActiveWorkout(SelectedWorkout.Exercises);
                _intervallTimerManager.StartIntervallTimer();
                ActiveIntervallName = _intervallTimerManager.GetNameOfActiveintervall();
                ActiveIntervallTime = _intervallTimerManager.GetRemainingTimeOfActiveIntervall();
                TimerStarted = true;
                TimerStopped = false;
            }
            catch (Exception ex)
            {
                OnExeptionoccured(new ExceptionArgs(ex.Message));
            }
        }

        private void StopIntervallTimer(object obj)
        {
            _intervallTimerManager.StopIntervallTimer();
            TimerStarted = false;
            TimerStopped = true;
        }

        private void PauseIntervallTimer(object obj)
        {
            _intervallTimerManager.PauseIntervallTimer();
            TimerStarted = false;
            TimerStopped = true;
        }

        private void AddNewIntervall(object obj)
        {
            NewIntervall = new IntervallExercise()
            {
                ExerciseId = Guid.NewGuid(),
                ExerciseName = NewIntervallName,
                IntervallTime = NewIntervallTime
            };

            _intervallWorkoutManager.AddExerciseToWorkoutById(NewIntervallWorkout.WorkoutId, NewIntervall);
            NewWorkoutIntervalls.Add(NewIntervall);
            NewIntervallWorkout.Exercises.Add(NewIntervall);
            CloseAddNewIntervallPage?.Invoke(this, EventArgs.Empty);
        }

        private void AddNewIntervallWorkout(object obj)
        {
            NewIntervallWorkout = new IntervallWorkout()
            {
                WorkoutId = NewIntervallWorkout.WorkoutId,
                WorkoutName = NewWorkoutName,
                Exercises = NewIntervallWorkout.Exercises
            };

            _intervallWorkoutManager.SetWorkoutNameById(NewIntervallWorkout.WorkoutId, NewWorkoutName);
            _intervallWorkoutManager.SaveWorkoutById(NewIntervallWorkout.WorkoutId);
            AllIntervallWorkouts.Add(_intervallWorkoutManager.GetWorkoutById(NewIntervallWorkout.WorkoutId));
            CloseNewIntervallWorkoutPage?.Invoke(this, EventArgs.Empty);
        }

        private void OpenNewIntervallWorkout(object obj)
        {
            NewIntervallWorkout = new IntervallWorkout()
            {
                WorkoutId = Guid.NewGuid(),
                WorkoutName = string.Empty,
                Exercises = new List<IntervallExercise>()
            };

            NewWorkoutIntervalls.Clear();
            NewWorkoutName = string.Empty;
            _intervallWorkoutManager.AddNewWorkout(NewIntervallWorkout);
            OpenNewIntervallWorkoutPage?.Invoke(this, EventArgs.Empty);
        }

        private void OpenNewIntervall(object obj)
        {
            NewIntervallName = string.Empty;
            NewIntervallTime = 0;
            OpenNewIntervallPage?.Invoke(this, EventArgs.Empty);
        }
    }
}
