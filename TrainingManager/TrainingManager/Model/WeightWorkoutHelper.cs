using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TrainingManager.Data.DTO;
using TrainingManager.ViewModel;

namespace TrainingManager.Model
{
    internal static class WeightWorkoutHelper
    {
        internal static WeightWorkoutVM WorkoutDTOToVM(WeightWorkoutDTO workout)
            => new WeightWorkoutVM()
            {
                Id = workout.Id,
                WorkoutName = workout.WorkoutName,
                WorkoutDate = workout.WorkoutDate,
                TotalWeight = workout.TotalWeight,
                WorkoutGuid = workout.WorkoutGuid,
                WorkoutType = workout.WorkoutType,
                Note = workout.Note,
                WeightExercises = new ObservableCollection<WeightExerciseVM>(workout.WeightExercisesDto.Select(x => new WeightExerciseVM()
                {
                    ExerciseGuid = x.ExerciseGuid,
                    ExerciseName = x.ExerciseName,
                    ExerciseNote = x.Note,
                    TotalExerciseWeight = x.TotalExerciseWeight,
                    TotalExerciseRounds = x.WeightRoundsDto.Count(),
                    ExerciseColor = x.Color,
                    MainMuscle = x.MainMuscleGroup,
                    WeightRounds = new ObservableCollection<WeightRoundVM>(x.WeightRoundsDto.Select(y => new WeightRoundVM()
                    {
                        RoundGuid = y.RoundGuid,
                        RoundNumber = y.RoundNumber,
                        Reps = y.Reps,
                        WeightOfExercise = y.WeightOfExercise,
                        RoundColor = y.Color
                    })),
                }))
            };

        internal static WeightWorkoutDTO WorkoutVMToDTO(WeightWorkoutVM workout)
            => new WeightWorkoutDTO
            {
                WorkoutDate = workout.WorkoutDate,
                TotalWeight = workout.TotalWeight,
                WorkoutName = workout.WorkoutName,
                Note = workout.Note,
                WorkoutType = WorkoutType.WeightWorkout,
                WorkoutImages = null,
                WeightExercisesDto = new List<WeightExerciseDTO>(workout.WeightExercises.Select(x => new WeightExerciseDTO()
                {
                    ExerciseGuid = x.ExerciseGuid,
                    ExerciseName = x.ExerciseName,
                    Note = x.ExerciseNote,
                    TotalExerciseWeight = x.TotalExerciseWeight,
                    Color = x.ExerciseColor,
                    MainMuscleGroup = x.MainMuscle,
                    WeightRoundsDto = new List<WeightRoundDTO>(x.WeightRounds.Select(y => new WeightRoundDTO()
                    {
                        Reps = y.Reps,
                        RoundGuid = y.RoundGuid,
                        RoundNumber = y.RoundNumber,
                        WeightOfExercise = y.WeightOfExercise,
                        Color = y.RoundColor
                    })),
                })),
            };
    }
}
