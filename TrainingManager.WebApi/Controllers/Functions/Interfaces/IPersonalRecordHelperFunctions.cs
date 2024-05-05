using System.Collections.Generic;
using System.Threading.Tasks;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers.Functions.Interfaces
{
    public interface IPersonalRecordHelperFunctions
    {
        Task AddNewPersonalRecordsIfNeeded(WeightWorkout workout);
        Task RemovePersonalRecordsByWorkout(WeightWorkout workout);
        Task UpdatePersonalRecordsByWorkout(WeightWorkout workout);
        PersonalRecordDTO FindMaxMovedWeightsOfActivity(int activityId);
        IEnumerable<(WeightActivityDTO activity, double weight, int reps)> FindMaxMovedWeightsByActivites(ApplicationUser user);
    }
}