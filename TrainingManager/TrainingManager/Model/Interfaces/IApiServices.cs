using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.Data.DTO;

namespace TrainingManager.Model
{
    public interface IApiServices
    {
          Task<IEnumerable<WeightWorkoutDTO>> GetWeightWorkoutsAsync();
          Task<bool> AddWeightWorkoutAsync(WeightWorkoutDTO weigthWorkoutDto);
          Task<bool> EditWeightWorkoutAsync(WeightWorkoutDTO weigthWorkoutDto);
          Task<bool> DeleteWeightWorkoutAsync(int workoutId);
          Task<IEnumerable<string>> GetWeightActivitiesAsync();
          Task<IEnumerable<WeightExerciseDTO>> GetWeightExercisesAsync();
          Task<bool> AddWeightExerciseAsync(WeightExerciseDTO weigthExerciseDto);
          Task<bool> UpdateWeightExerciseAsync(WeightExerciseDTO weigthExerciseDto);

    }
}
