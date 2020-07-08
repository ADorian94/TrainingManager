using System;

namespace TrainingManager.Model.Workouts
{
    public class ExerciseBase
    {
        /// <summary>
        /// Unique id of the exercise.
        /// </summary>
        public Guid ExerciseId { get; set; }
        /// <summary>
        /// Name of the exercise.
        /// </summary>
        public string ExerciseName { get; set; }
        /// <summary>
        /// Optional note for the exercise.
        /// </summary>
        public string Note { get; set; }
    }
}
