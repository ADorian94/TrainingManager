using System;
using System.Collections.Generic;

namespace TrainingManager.Data.DTO
{
    public class WeightWorkoutDTO
    {
        public int Id { get; set; }
        public Guid WorkoutGuid { get; set; }
        public string WorkoutName { get; set; }
        public string Note { get; set; }
        public WorkoutType WorkoutType { get; set; }
        public DateTime WorkoutDate { get; set; }
        public double TotalWeight { get; set; }
        public ICollection<WeigthExerciseDTO> WeightExercises { get; set; }
        public ICollection<ImageDTO> WorkoutImages { get; set; }
    }
}
