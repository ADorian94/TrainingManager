using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Data
{
    public class TrainingManagerContext : DbContext
    {
        public TrainingManagerContext (DbContextOptions<TrainingManagerContext> options)
            : base(options)
        {
        }

        public DbSet<WeightWorkout> WeightWorkout { get; set; }

        public DbSet<WeightExercise> WeightExercise { get; set; }
    }
}
