using System;
using System.Collections.Generic;

namespace TrainingManager.Data.DTO
{
    public class WeightExerciseDTO
    {
        public int Id { get; set; }
        public Guid ExerciseGuid { get; set; }
        public string ExerciseName { get; set; }
        public double TotalExerciseWeight { get; set; }
        public string Note { get; set; }
        public int WorkoutId { get; set; }
        public virtual ICollection<WeightRoundDTO> WeightRoundsDto { get; set; }
    }
}
