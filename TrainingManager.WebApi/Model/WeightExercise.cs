using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingManager.WebApi.Model
{
    public class WeightExercise
    {
        public int Id { get; set; }
        public Guid ExerciseGuid { get; set; }
        public int ActivityId { get; set; }
        public double TotalExerciseWeight { get; set; }
        public string Note { get; set; }
        [ForeignKey("Workout")]
        public int WorkoutId { get; set; }
        public string OwnerUserName { get; set; }
        public virtual ICollection<WeightRound> WeightRounds { get; set; }
    }
}
