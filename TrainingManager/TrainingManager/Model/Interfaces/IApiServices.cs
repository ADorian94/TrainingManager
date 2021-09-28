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
    }
}
