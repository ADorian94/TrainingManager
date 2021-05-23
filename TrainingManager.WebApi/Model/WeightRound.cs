using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingManager.WebApi.Model
{
    public class WeightRound
    {
        public int Id { get; set; }
        public Guid RoundGuid { get; set; }
        public int RoundNumber { get; set; }
        public double WeightOfDrill { get; set; }
        public int Reps { get; set; }
        [ForeignKey("Exercise")]
        public int ExerciseId { get; set; }
        public virtual WeightExercise Exercise { get; set; }
    }
}
