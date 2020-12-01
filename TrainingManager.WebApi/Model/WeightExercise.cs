using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingManager.WebApi.Model
{
    public class WeightExercise
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public string Note { get; set; }
        public double WeightOfExercise { get; set; }
        public DateTime ExerciseDate { get; set; }
        public int Reps { get; set; }
    }
}
