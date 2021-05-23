using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingManager.WebApi.Model
{
    public class WorkoutImage
    {
        public int Id { get; set; }
        [ForeignKey("Workout")]
        public int WorkoutId { get; set; }
        public byte[] ImageSmall { get; set; }
        public byte[] ImageLarge { get; set; }

        public WeightWorkout Workout { get; set; }
    }
}
