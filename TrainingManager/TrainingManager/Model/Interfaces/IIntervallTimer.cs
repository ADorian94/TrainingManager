using System;
using System.Collections.Generic;
using TrainingManager.Model.Workouts.IntervallWorkout;

namespace TrainingManager.Model.Interfaces
{
    public interface IIntervallTimerManager
    {
        event EventHandler IntervallTimeChanged;
        event EventHandler IntervallChanged;
        event EventHandler IntervallFinished;
        void SetActiveWorkout(List<IntervallExercise> intervallWorkout);
        void StartIntervallTimer();
        void StopIntervallTimer();
        void PauseIntervallTimer();
        void AddIntervall(IntervallExercise newIntervall);
        string GetNameOfActiveintervall();
        int GetRemainingTimeOfActiveIntervall();
    }
}
