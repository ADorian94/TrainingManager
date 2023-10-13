using System;
using System.ComponentModel.DataAnnotations.Schema;
using TrainingManager.Data;

namespace LiftIt.WebApi.Model
{
    public class WeightRound
    {
        public int Id { get; set; }
        public Guid RoundGuid { get; set; }
        public int RoundNumber { get; set; }
        public double WeightOfExercise { get; set; }
        public int Reps { get; set; }
        public MaterialColors Color { get; set; }
        [ForeignKey("Exercise")]
        public int ExerciseId { get; set; }
    }
}
