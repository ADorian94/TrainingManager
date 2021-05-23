//using System;
//using System.Collections.Generic;
//using System.Linq;
//using TrainingManager.Model.Interfaces;
//using TrainingManager.Model.Workouts.WeightWorkout;

//namespace TrainingManager.Model
//{
//    public class WeightWorkoutManager : WorkoutManagerBase<WeightWorkout, WeightExercise>/*, IWeightWorkoutManager*/
//    {
//        public WeightWorkoutManager()
//        {
//            var workouts = _xmlHandler.LoadWorkoutXmls(WorkoutType.WeightWorkout);
//            _workouts = workouts.Count > 0 ? new List<WeightWorkout>(workouts) : new List<WeightWorkout>();
//        }

//        public double GetTotalWeightById(Guid workoutId)
//        {
//            double x = _workouts.Single(y => y.WorkoutId.Equals(workoutId)).Exercises.Aggregate(0.0, (sum, exercise) => sum + (exercise.Reps * exercise.WeightOfExercise));
//            return x;
//        }
//    }
//}
