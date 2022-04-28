using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers.Functions
{
    public class StatFunctions
    {
        public List<(int year, int month, double weight)> SumMovedWeightsByMonth(IQueryable<WeightWorkout> workouts)
        {
            var allWorkouts = workouts.GroupBy(w => w.WorkoutDate.Year).ToList();
            var resutWeights = new List<(int, int, double)>();

            foreach (var yearWorkouts in allWorkouts)
            {
                var monthWorkouts = yearWorkouts.GroupBy(m => m.WorkoutDate.Month);

                foreach (var monthWorkout in monthWorkouts)
                {
                    double weightOfTheMonth = 0;

                    foreach (var workout in monthWorkout)
                    {
                        if (workout.WorkoutDate < DateTime.Now)
                            weightOfTheMonth += workout.TotalWeight;
                    }

                    resutWeights.Add((yearWorkouts.Key, monthWorkout.Key, weightOfTheMonth));
                }
            }

            return resutWeights;
        }

        public IEnumerable<Tuple<DateTime, double>> CollectMovedWeightsInTheMonth(IQueryable<WeightWorkout> workouts, int year, int month)
        {
            return workouts.Where(x => x.WorkoutDate.Year == year && x.WorkoutDate.Month == month)
                           .Select(w => new Tuple<DateTime, double>(w.WorkoutDate, w.TotalWeight));
        }
    }
}
