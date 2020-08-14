using System;
using System.Collections.Generic;

namespace TrainingManager.Model.Workouts
{
    public class WorkoutBase<ExerciseTemplate> where ExerciseTemplate : ExerciseBase
    {
        /// <summary>
        /// Unique id of the workout.
        /// </summary>
        public Guid WorkoutId { get; set; }
        public string WorkoutIdString { get => WorkoutId.ToString(); }
        /// <summary>
        /// Name of the workout
        /// </summary>
        public string WorkoutName { get; set; }
        /// <summary>
        /// Optional note of the workout.
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Exercises of the workout. For example: intervall, weightexercise
        /// </summary>
        public List<ExerciseTemplate> Exercises { get; set; }
        public WorkoutType WorkoutType { get; set; }
    }
}
