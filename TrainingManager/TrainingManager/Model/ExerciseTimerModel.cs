using System;
using TrainingManager.Model.Interfaces;

namespace TrainingManager.Model
{
    public class ExerciseTimerModel : IExerciseTimer
    {
        public event EventHandler ExerciseTimerTicked;
        private Timer _timer;

        public int ElapsedTime { get; private set; }

        public ExerciseTimerModel()
        {
            ElapsedTime = 0;
            _timer = new Timer();
            _timer.TickTimer += new EventHandler(OnTickTimer);
        }

        private void OnTickTimer(object sender, EventArgs e)
        {
            ++ElapsedTime;
            ExerciseTimerTicked?.Invoke(this, EventArgs.Empty);
        }

        public void ResetTimer() => ElapsedTime = 0;

        public void StartTimer() => _timer.StartTimer();

        public int GetElapsedTime() => ElapsedTime;

        public void StopTimer()
        {
            _timer.StopTimer();
            ResetTimer();
            ExerciseTimerTicked?.Invoke(this, EventArgs.Empty);
        }

        public void PauseTimer() => _timer.StopTimer();

        public bool IsTimerStarted() => _timer.TimerStarted;
    }
}
