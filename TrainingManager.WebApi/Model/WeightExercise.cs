using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingManager.WebApi.Model
{
    public class WeightExercise
    {
        public int Id { get; set; }
        public Guid ExerciseGuid { get; set; }
        public string ExerciseName { get; set; }
        [ForeignKey("WeightDrill")]
        public int DrillId { get; set; }
        public virtual WeightDrill WeightDrill { get; set; }
    }
}
