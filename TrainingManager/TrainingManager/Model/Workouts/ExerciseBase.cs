using System;

namespace TrainingManager.Model.Workouts
{
    public abstract class ExerciseBase
    {
        /// <summary>
        /// Unique id of the exercise.
        /// </summary>
        public int Id { get; set; }
        public Guid ExerciseGuid { get; set; }
        //public string ExerciseIdString { get => ExerciseId.ToString(); }
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
