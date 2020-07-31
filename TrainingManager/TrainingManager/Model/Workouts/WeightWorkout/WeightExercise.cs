using System;

namespace TrainingManager.Model.Workouts.WeightWorkout
{
    public class WeightExercise : ExerciseBase
    {
        public double WeightOfExercise { get; set; }
        public DateTime ExerciseDate { get; set; }
        public int Reps { get; set; }
    }
}
