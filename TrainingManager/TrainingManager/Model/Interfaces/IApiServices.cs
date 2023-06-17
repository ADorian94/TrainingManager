using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrainingManager.Data;
using TrainingManager.Data.DTO;

namespace TrainingManager.Model
{
    public interface IApiServices
    {
        Task<bool> LoginAsync(string userName, string password);
        Task<bool> RegisterAync(string name, string userName, string userEmail, string password, string confirmPassword);
        Task<bool> LogoutAsync();
        Task<IEnumerable<WeightWorkoutDTO>> GetWeightWorkoutsAsync();
        Task<WeightWorkoutDTO> GetWeightWorkoutAsync(DateTime date);
        Task<WeightWorkoutDTO> GetWeightWorkoutAsync(int year, int dayOfYear);
        Task<WeightWorkoutDTO> GetWeightWorkoutAsync(string guid);
        Task<bool> IsWeightWorkoutExitsAsync(string guid);
        Task<bool> IsWeightWorkoutExitsAsync(DateTime date);
        Task<bool> IsWeightWorkoutExitsAsync(int year, int dayOfYear);
        Task<IEnumerable<WeightWorkoutDTO>> GetRecentWeightWorkoutsAsync();
        Task<bool> UploadProfilePicture(byte[] image);
        Task<byte[]> DownloadProfilePictureAsync();
        Task<string> GetNameOfTheUser();
        Task<bool> AddWeightWorkoutAsync(WeightWorkoutDTO weigthWorkoutDto);
        Task<bool> EditWeightWorkoutAsync(WeightWorkoutDTO weigthWorkoutDto);
        Task<bool> DeleteWeightWorkoutAsync(int workoutId);
        Task<bool> DeleteWeightWorkoutAsync(string guid);
        Task<IEnumerable<WeightActivityDTO>> GetWeightActivitiesAsync();
        Task<IEnumerable<WeightRoundDTO>> GetPreviousRoundsAsync(Guid id, int take);
        Task<IEnumerable<WeightExerciseDTO>> GetWeightExercisesAsync();
        Task<bool> AddWeightExerciseAsync(WeightExerciseDTO weigthExerciseDto);
        Task<bool> UpdateWeightExerciseAsync(WeightExerciseDTO weigthExerciseDto);
        Task<IEnumerable<(int year, int month, double weight)>> GetMovedWorkoutsGroupByMonth();
        Task<IEnumerable<(DateTime date, double weight)>> GetMovedWeightsInTheMonth(int year, int month);
        Task<IEnumerable<(int year, int month, IEnumerable<(DateTime date, double weight)>)>> GetMovedWeightsGroupByMonth();
        Task<IEnumerable<(Muscle muscle, double weight)>> GetWeeklyMuscleDataAsync();
        Task<IEnumerable<(WeightActivityDTO activity, double weight, int reps)>> GetMaxMovedWeightsByActivites();
        Task<(double weight, int reps)> GetWeightActivityPRAsync(Guid id);
        Task<bool> EditWeightActivityAsync(WeightActivityDTO weigthActivityDto);
        Task<IEnumerable<(WeightActivityDTO activity, double weight, int reps)>> GetWatchedWeightActivitiesAsync();
        Task<IEnumerable<WeightWorkoutDTO>> SearchWorkoutAsync(string keyWords);
        Task<IEnumerable<WeightActivityDTO>> SearchActivityAsync(string keyWords);
    }
}
