using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingManager.WebApi.Model
{
    public class Round
    {
        public int Id { get; set; }
        public Guid RoundGuid { get; set; }
        public string RoundName { get; set; }
        public string Note { get; set; }
        public int Reps { get; set; }
        [ForeignKey("Workout")]
        public int WorkoutId { get; set; }
        public virtual WeightWorkout Workout { get; set; }
        public virtual ICollection<WeightDrill> WeightDrills { get; set; }
    }
}
