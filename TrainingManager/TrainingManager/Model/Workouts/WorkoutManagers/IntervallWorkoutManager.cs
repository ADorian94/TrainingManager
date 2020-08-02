using System.Collections.Generic;
using System.Linq;
using TrainingManager.Model.Workouts.IntervallWorkout;

namespace TrainingManager.Model
{
    public class IntervallWorkoutManager : WorkoutManagerBase<IntervallWorkout, IntervallExercise>
    {
        public IntervallWorkoutManager()
        {
            var workouts = _xmlHandler.LoadWorkoutXmls(WorkoutType.IntervallWorkout);

            if (workouts.Count() > 0)
                _workouts = new List<IntervallWorkout>(workouts);
            else
                _workouts = new List<IntervallWorkout>();
        }
    }
}
