using System.Collections.Generic;
using TrainingManager.Model.Workouts.WeightWorkout;

namespace TrainingManager.Model
{
    public class WeightWorkoutManager : WorkoutManagerBase<WeightWorkout, WeightExercise>
    {
        public WeightWorkoutManager()
        {
            _workouts = new List<WeightWorkout>();
        }

    }
}
