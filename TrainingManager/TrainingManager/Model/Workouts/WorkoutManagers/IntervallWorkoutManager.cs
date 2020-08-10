using System.Collections.Generic;
using TrainingManager.Model.Workouts.IntervallWorkout;

namespace TrainingManager.Model
{
    public class IntervallWorkoutManager : WorkoutManagerBase<IntervallWorkout, IntervallExercise>
    {
        public IntervallWorkoutManager()
        {
            var workouts = _xmlHandler.LoadWorkoutXmls(WorkoutType.IntervallWorkout);
            _workouts = workouts.Count > 0 ? new List<IntervallWorkout>(workouts) : new List<IntervallWorkout>();
        }
    }
}
