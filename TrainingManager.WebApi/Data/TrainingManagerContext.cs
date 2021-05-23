using Microsoft.EntityFrameworkCore;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Data
{
    public class TrainingManagerContext : DbContext
    {
        public TrainingManagerContext(DbContextOptions<TrainingManagerContext> options) : base(options)
        {
        }

        public DbSet<WeightWorkout> WeightWorkouts { get; set; }
        public DbSet<WeightDrill> WeightDrills { get; set; }
        public DbSet<Round> WeightRounds { get; set; }
        public DbSet<WeightExercise> WeightExercises { get; set; }
    }
}
