using System;

namespace TrainingManager.Model.Interfaces
{
    public interface IExerciseTimer
    {
        int ElapsedTime { get; }
        event EventHandler ExerciseTimerTicked;
        void ResetTimer();
        void StartTimer();
        void StopTimer();
        void PauseTimer();
        int GetElapsedTime();
        bool IsTimerStarted();
    }
}
