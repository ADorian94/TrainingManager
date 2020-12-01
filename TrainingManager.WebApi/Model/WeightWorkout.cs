using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static TrainingManager.WebApi.Model.StructAndEnums;

namespace TrainingManager.WebApi.Model
{
    public class WeightWorkout
    {
        public int Id { get; set; }
        public string WorkoutName { get; set; }
        public string Note { get; set; }
        public ICollection<WeightExercise> Exercises { get; set; }
        public WorkoutType WorkoutType { get; set; }
        public DateTime WorkoutDate { get; set; }
        public double TotalWeight { get; set; }
    }
}
