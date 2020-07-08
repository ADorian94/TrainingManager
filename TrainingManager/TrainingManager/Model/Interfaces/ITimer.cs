using System;

namespace TrainingManager.Model.Interfaces
{
    public interface ITimer
    {
        event EventHandler TickTimer;

        bool TimerStarted { get; }
        void StartTimer();
        void StopTimer();
    }
}
