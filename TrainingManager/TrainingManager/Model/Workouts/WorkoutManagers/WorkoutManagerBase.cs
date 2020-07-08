using System;
using System.Collections.Generic;
using System.Linq;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.Persistence;
using TrainingManager.Model.Workouts;

namespace TrainingManager.Model
{
    public class WorkoutManagerBase<WorkoutTemplate, ExerciseTemplate> : IWorkoutManager<WorkoutTemplate, ExerciseTemplate>
        where WorkoutTemplate : WorkoutBase<ExerciseTemplate>
        where ExerciseTemplate : ExerciseBase
    {
        public List<WorkoutTemplate> _workouts;
        public IXmlHandler<WorkoutTemplate> _xmlHandler;

        public WorkoutManagerBase()
        {
            _workouts = new List<WorkoutTemplate>();
            _xmlHandler = new XmlHandler<WorkoutTemplate, ExerciseTemplate>();
        }

        /// <summary>
        /// Add new exercise the the workout list.
        /// </summary>
        /// <param name="newWorkout">New workout.</param>
        public void AddNewWorkout(WorkoutTemplate newWorkout) => _workouts.Add(newWorkout);
        /// <summary>
        /// Get all added workouts.
        /// </summary>
        /// <returns>Workout list.</returns>
        public List<WorkoutTemplate> GetWorkouts() => _workouts;
        /// <summary>
        /// Add new exercise to a specified workout.
        /// </summary>
        /// <param name="workoutId">Unique if of the workout.</param>
        /// <param name="exercise">New exercise</param>
        public void AddExerciseToWorkoutById(Guid workoutId, ExerciseTemplate exercise) =>
            _workouts.Single(w => w.WorkoutId == workoutId).Exercises.Add(exercise);

        /// <summary>
        /// Set the name of the secified workout.
        /// </summary>
        /// <param name="workoutId">Unique workout id</param>
        /// <param name="workoutName">Name of the workout</param>
        public void SetWorkoutNameById(Guid workoutId, string workoutName)
        {
            //WorkoutTemplate temporaryWorkout = _workouts.Single(x => x.WorkoutId == workoutId);
            //int index = _workouts.FindIndex(x => x.WorkoutId == workoutId);
            //_workouts.RemoveAt(index);

            //_workouts.Add(new WorkoutTemplate()
            //{
            //    WorkoutId = temporaryWorkout.WorkoutId,
            //    WorkoutName = workoutName,
            //    Exercises = temporaryWorkout.Exercises
            //});

            _workouts.Single(x => x.WorkoutId == workoutId).WorkoutName = workoutName;
        }
        /// <summary>
        /// Get the specified workout.
        /// </summary>
        /// <param name="workoutId">Unique if of the searched workout.</param>
        /// <returns>Selected workout.</returns>
        public WorkoutTemplate GetWorkoutById(Guid workoutId) => _workouts.Single(x => x.WorkoutId == workoutId);

        /// <summary>
        /// Get the exercises of the specified workout.
        /// </summary>
        /// <param name="workoutId">Unique id of the workout.</param>
        /// <returns>Exercises of the selected workout.</returns>
        public List<ExerciseTemplate> GetWorkoutExercisesById(Guid workoutId) => _workouts.Single(x => x.WorkoutId == workoutId).Exercises;

        /// <summary>
        /// Save data to xml.
        /// </summary>
        /// <param name="workoutId">Workout id</param>
        public void SaveWorkoutById(Guid workoutId) => _xmlHandler.SaveToXml(_workouts.Single(x => x.WorkoutId == workoutId));
    }
}
