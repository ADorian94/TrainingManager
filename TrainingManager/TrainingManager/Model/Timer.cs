using System;
using TrainingManager.Model.Interfaces;
using Xamarin.Forms;

namespace TrainingManager.Model
{
    public class Timer : ITimer
    {
        public event EventHandler TickTimer;
        public bool TimerStarted { get; private set; }

        public Timer()
        {
            TimerStarted = false;
            Device.StartTimer(TimeSpan.FromSeconds(1),
                () =>
                {
                    if (TimerStarted)
                        TickTimer?.Invoke(this, EventArgs.Empty);

                    return true;
                });
        }

        public void StartTimer() => TimerStarted = true;

        public void StopTimer() => TimerStarted = false;
    }
}
