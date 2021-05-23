using System;
using System.Collections.Generic;

namespace TrainingManager.Data.DTO
{
    public class WeigthExerciseDTO
    {
        public int Id { get; set; }
        public Guid ExerciseGuid { get; set; }
        public string ExerciseName { get; set; }
        public double TotalExerciseWeight { get; set; }
        public string Note { get; set; }
        public virtual ICollection<WeightRoundDTO> WeightRounds { get; set; }
    }
}
