using System;

namespace TrainingManager.Data.DTO
{
    public class WeigthExerciseDTO
    {
        public int Id { get; set; }
        public Guid ExerciseGuid { get; set; }
        public string ExerciseName { get; set; }
        public int DrillId { get; set; }
    }
}
