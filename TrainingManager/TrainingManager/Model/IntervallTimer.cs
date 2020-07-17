using System;
using System.Collections.Generic;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.Workouts.IntervallWorkout;

namespace TrainingManager.Model
{
    public class IntervallTimerManager : IIntervallTimerManager
    {
        private IntervallExercise _activeIntervall;
        private List<IntervallExercise> _intervalls;
        private Queue<IntervallExercise> _intervallQueue;
        private IntervallTimerStates _intervallTimerState;
        private ITimer _timer;
        public event EventHandler IntervallTimeChanged;
        public event EventHandler IntervallChanged;
        public event EventHandler IntervallFinished;

        public IntervallTimerManager()
        {
            _intervalls = new List<IntervallExercise>();
            _timer = new Timer();
            _timer.TickTimer += new EventHandler(OnTickTimer);
            _intervallTimerState = IntervallTimerStates.IntervallTimerStopped;
        }

        public void SetActiveWorkout(List<IntervallExercise> intervallWorkout) => _intervalls = intervallWorkout;

        public void StartIntervallTimer()
        {
            switch (_intervallTimerState)
            {
                case IntervallTimerStates.IntervallTimerStopped:
                    _intervallQueue = new Queue<IntervallExercise>(_intervalls);
                    _activeIntervall = _intervallQueue.Dequeue();
                    break;
                case IntervallTimerStates.IntervallTimerPaused:
                    if (!(_activeIntervall.IntervallTime > 0))
                        _activeIntervall = _intervallQueue.Dequeue();
                    break;
                case IntervallTimerStates.IntervallTimerStarted:
                default:
                    throw new NotImplementedException();
            }

            _intervallTimerState = IntervallTimerStates.IntervallTimerStarted;
            _timer.StartTimer();
        }

        public void StopIntervallTimer()
        {
            _timer.StopTimer();
            _intervallTimerState = IntervallTimerStates.IntervallTimerStopped;
        }

        public void PauseIntervallTimer()
        {
            _timer.StopTimer();
            _intervallTimerState = IntervallTimerStates.IntervallTimerPaused;

        }

        public void AddIntervall(IntervallExercise newIntervall) => _intervalls.Add(newIntervall);
        public string GetNameOfActiveintervall() => _activeIntervall.ExerciseName;
        public int GetRemainingTimeOfActiveIntervall() => _activeIntervall.IntervallTime;

        private void OnTickTimer(object sender, EventArgs e)
        {
            if (_intervallTimerState == IntervallTimerStates.IntervallTimerStarted)
            {
                if (_activeIntervall.IntervallTime > 0)
                {
                    --_activeIntervall.IntervallTime;
                    IntervallTimeChanged?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    if (_intervallQueue.Count == 0)
                    {
                        _timer.StopTimer();
                        _activeIntervall.ExerciseName = string.Empty;
                        _activeIntervall.IntervallTime = 0;
                        IntervallFinished?.Invoke(this, EventArgs.Empty);
                        _intervallTimerState = IntervallTimerStates.IntervallTimerStopped;
                    }
                    else
                    {
                        _activeIntervall = _intervallQueue.Dequeue();
                        IntervallChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
