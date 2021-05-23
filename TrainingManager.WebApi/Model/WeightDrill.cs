using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingManager.WebApi.Model
{
    public class WeightDrill
    {
        public int Id { get; set; }
        public Guid DrillGuid { get; set; }
        public string DrillName { get; set; }
        public string Note { get; set; }
        public double WeightOfDrill { get; set; }
        public DateTime DrillDate { get; set; }
        public int Reps { get; set; }
        [ForeignKey("Workout")]
        public int WorkoutId { get; set; }
        public virtual WeightWorkout Workout { get; set; }
        public int RoundId { get; set; }
        public virtual Round Round { get; set; }
    }
}
