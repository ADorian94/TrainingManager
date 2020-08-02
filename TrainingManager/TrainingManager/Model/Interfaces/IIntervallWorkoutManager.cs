using System;
using System.Collections.Generic;

namespace TrainingManager.Model.Interfaces
{
    public interface IIntervallWorkoutManager
    {
        void SaveWorkoutById(Guid workoutId);
    }

    public interface IWeightWorkoutManager
    {
    }

    public interface IWorkoutManager<WorkoutTemplate, ExerciseTemplate>
    {
        void AddNewWorkout(WorkoutTemplate workout);
        WorkoutTemplate GetWorkoutById(Guid workoutId);
        void SetWorkoutNameById(Guid workoutId, string workoutName);
        void AddExerciseToWorkoutById(Guid workoutId, ExerciseTemplate exercise);
        List<ExerciseTemplate> GetWorkoutExercisesById(Guid workoutId);
        ExerciseTemplate GetExerciseInWorkoutById(Guid workoutId, Guid exerciseId);
        void SaveWorkoutById(Guid workoutId, WorkoutType workoutType);
        List<WorkoutTemplate> GetWorkouts();
        void DeleteWorkoutById(string stringGuid);
        void DeleteExerciseFromWorkoutById(Guid workoutId, Guid exerciseId);
    }
}
