using LiftIt.WebApi.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class TrainingManagerContext : IdentityDbContext<ApplicationUser>
{
    public TrainingManagerContext(DbContextOptions<TrainingManagerContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().ToTable("Users");
    }

    public DbSet<WeightWorkout> WeightWorkouts { get; set; }
    public DbSet<WeightExercise> WeightExercises { get; set; }
    public DbSet<WeightActivity> WeightActivities { get; set; }
    public DbSet<WeightRound> WeightRounds { get; set; }
    public DbSet<WorkoutImage> WorkoutImages { get; set; }
}
