using System;
using System.Collections.Generic;
using TrainingManager.Data.DTO;

namespace TrainingManager.WebApi.Model
{
    public class WeightWorkout
    {
        public int Id { get; set; }
        public Guid WorkoutGuid { get; set; }
        public string WorkoutName { get; set; }
        public string Note { get; set; }
        public WorkoutType WorkoutType { get; set; }
        public DateTime WorkoutDate { get; set; }
        public double TotalWeight { get; set; }
        public virtual ICollection<Round> Rounds { get; set; }
        public virtual ICollection<WeightDrill> WeightDrills { get; set; }
    }
}
