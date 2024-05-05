using System;
using System.Collections.Generic;
using System.Linq;
using TrainingManager.Data;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers.Functions.Interfaces
{
    public interface IStatFunctions
    {
        List<(int year, int month, double weight)> SumMovedWeightsByMonth(IQueryable<WeightWorkout> workouts);
        Muscle TryGetMuscle(WeightActivity x);
        IEnumerable<Tuple<DateTime, double>> CollectMovedWeightsInTheMonth(IQueryable<WeightWorkout> workouts, int year, int month);
        IEnumerable<YearMonthWorkoutGroupDTO> CollectMovedWeightsGroupByMonth(IQueryable<WeightWorkout> workouts);
        IEnumerable<(Muscle muscle, double weight)> CollectRecentMovedWeightsGroupByMuscle(IQueryable<WeightWorkout> workouts, IQueryable<WeightExercise> exercises, IQueryable<WeightActivity> activities);
        IEnumerable<WeightRoundDTO> GetLastRoundsOfActivity(int id, ApplicationUser user, int take);


    }
}