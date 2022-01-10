using System.Collections.Generic;
using System.Threading.Tasks;
using TrainingManager.Data.DTO;

namespace TrainingManager.Model
{
    public interface IApiServices
    {
        Task<bool> LoginAsync(string userName, string password);
        Task<bool> RegisterAync(string name, string userName, string userEmail, string password, string confirmPassword);
        Task<bool> LogoutAsync();
        Task<IEnumerable<WeightWorkoutDTO>> GetWeightWorkoutsAsync();
        Task<bool> UploadProfilePicture(byte[] image);
        Task<byte[]> DownloadProfilePicture();
        Task<string> GetNameOfTheUser();
        Task<bool> AddWeightWorkoutAsync(WeightWorkoutDTO weigthWorkoutDto);
        Task<bool> EditWeightWorkoutAsync(WeightWorkoutDTO weigthWorkoutDto);
        Task<bool> DeleteWeightWorkoutAsync(int workoutId);
        Task<IEnumerable<string>> GetWeightActivitiesAsync();
        Task<IEnumerable<WeightExerciseDTO>> GetWeightExercisesAsync();
        Task<bool> AddWeightExerciseAsync(WeightExerciseDTO weigthExerciseDto);
        Task<bool> UpdateWeightExerciseAsync(WeightExerciseDTO weigthExerciseDto);

    }
}
