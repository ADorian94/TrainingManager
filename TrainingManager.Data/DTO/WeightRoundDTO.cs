using System;

namespace TrainingManager.Data.DTO
{
    public class WeightRoundDTO
    {
        public int Id { get; set; }
        public Guid RoundGuid { get; set; }
        public int RoundNumber { get; set; }
        public double WeightOfExercise { get; set; }
        public int Reps { get; set; }
        public int ExerciseId { get; set; }
    }
}
