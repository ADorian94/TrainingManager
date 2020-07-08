using System;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;

namespace TrainingManager.ViewModel
{
    public class ExerciseTimerVM : ViewModelBase
    {
        private IExerciseTimer _exerciseTimer;
        public DelegateCommand StartCommand { get; private set; }
        public DelegateCommand StopCommand { get; private set; }
        public DelegateCommand PauseCommand { get; private set; }


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

        private int _elapsedTime;
        public int ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnPropertyChanged();
            }
        }

        public ExerciseTimerVM()
        {
            _exerciseTimer = new ExerciseTimerModel();
            _exerciseTimer.ExerciseTimerTicked += new EventHandler(OnTickTimer);
            TimerStarted = false;
            TimerStopped = true;
            InitializeCommands();
        }

        protected override void InitializeCommands()
        {
            StartCommand = new DelegateCommand(Start);
            StopCommand = new DelegateCommand(Stop);
            PauseCommand = new DelegateCommand(Pause);
        }

        private void OnTickTimer(object sender, EventArgs e) => ElapsedTime = _exerciseTimer.ElapsedTime;

        private void Start(object obj)
        {
            _exerciseTimer.StartTimer();
            TimerStarted = true;
            TimerStopped = false;
        }

        private void Stop(object obj)
        {
            _exerciseTimer.StopTimer();
            TimerStarted = false;
            TimerStopped = true;
        }

        private void Pause(object obj)
        {
            _exerciseTimer.PauseTimer();
            TimerStarted = false;
            TimerStopped = true;
        }
    }
}
