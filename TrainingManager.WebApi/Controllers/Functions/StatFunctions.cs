using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TrainingManager.Data;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Controllers.Functions.Interfaces;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers.Functions
{
    public class StatFunctions : IStatFunctions
    {
        private readonly TrainingManagerContext _context;

        public StatFunctions(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<TrainingManagerContext>();
        }

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

        public Muscle TryGetMuscle(WeightActivity x)
        {
            try
            {
                return x.MainMuscleGroup;
            }
            catch
            {
                return Muscle.Unknown;
            }
        }

        public IEnumerable<Tuple<DateTime, double>> CollectMovedWeightsInTheMonth(IQueryable<WeightWorkout> workouts, int year, int month)
        {
            return workouts.Where(x => x.WorkoutDate.Year == year && x.WorkoutDate.Month == month)
                           .Select(w => new Tuple<DateTime, double>(w.WorkoutDate, w.TotalWeight));
        }

        public IEnumerable<YearMonthWorkoutGroupDTO> CollectMovedWeightsGroupByMonth(IQueryable<WeightWorkout> workouts)
            => workouts.GroupBy(w => new { w.WorkoutDate.Year, w.WorkoutDate.Month })
                .OrderByDescending(x => x.Key.Year)
                .ThenByDescending(x => x.Key.Month)
                .Take(5)
                .Select(r => new YearMonthWorkoutGroupDTO()
                {
                    Year = r.Key.Year,
                    Month = r.Key.Month,
                    WorkoutsInMonth = r.Where(w => w.WorkoutDate < DateTime.Now)
                            .Select(i => new MovedWeightsInMonthDTO()
                            {
                                Date = i.WorkoutDate,
                                Weight = i.TotalWeight
                            })
                });

        public IEnumerable<(Muscle muscle, double weight)> CollectRecentMovedWeightsGroupByMuscle(IQueryable<WeightWorkout> workouts, IQueryable<WeightExercise> exercises, IQueryable<WeightActivity> activities)
        {
            var thisWeeksWorkoutsIds = workouts.Where(x => IsThisWorkoutInThisWeek(x.WorkoutDate.ToUniversalTime())).Select(x => x.Id);
            var relatedExercises = exercises.Where(x => thisWeeksWorkoutsIds.Contains(x.WorkoutId));
            var resutWeights = new List<(Muscle muscle, double weight)>();

            foreach (var exercise in relatedExercises)
            {
                var muscle = activities.FirstOrDefault(x => x.Id == exercise.ActivityId).MainMuscleGroup;

                if (resutWeights.Any(x => x.muscle == muscle))
                {
                    var element = resutWeights.Single(x => x.muscle == muscle);
                    resutWeights.Remove(element);
                    element.weight += exercise.TotalExerciseWeight;
                    resutWeights.Add(element);
                }
                else
                    resutWeights.Add((muscle, exercise.TotalExerciseWeight));
            }

            return resutWeights;
        }

        private bool IsThisWorkoutInThisWeek(DateTime workoutDate)
        {
            var actualDate = DateTime.Now.ToUniversalTime();
            var actualDay = actualDate.DayOfWeek;

            if (actualDate.Year == workoutDate.Year)
            {
                var days = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
                int dayIndex = days.IndexOf(actualDay);

                if (actualDate.DayOfYear - (dayIndex + 1) < workoutDate.DayOfYear &&
                    actualDate.DayOfYear + (days.Count - 1 - dayIndex) > workoutDate.DayOfYear)
                    return true;
            }

            return false;
        }

        public IEnumerable<WeightRoundDTO> GetLastRoundsOfActivity(int id, ApplicationUser user, int take)
        {
            IEnumerable<Tuple<int, DateTime?>> exerciseWithDate =
                         _context.WeightExercises.
                         Where(x => x.OwnerUserName == user.UserName && x.ActivityId == id).
                         ToList().
                         Select(x => new Tuple<int, DateTime?>(x.Id, GetLastWorkoutIdsOfExercise(x.WorkoutId))).
                         OrderByDescending(x => x.Item2).
                         Where(x => x.Item2 != null).
                         Take(take);

            var result = new List<WeightRoundDTO>();

            foreach (var exercise in exerciseWithDate)
            {
                var rounds = _context.WeightRounds.Where(x => x.ExerciseId == exercise.Item1);

                foreach (var round in rounds)
                {
                    result.Add(new WeightRoundDTO()
                    {
                        Id = round.Id,
                        Color = round.Color,
                        ExerciseId = round.ExerciseId,
                        Reps = round.Reps,
                        RoundGuid = round.RoundGuid,
                        RoundNumber = round.RoundNumber,
                        WeightOfExercise = round.WeightOfExercise
                    });
                }
            }

            return result;
        }

        private DateTime? GetLastWorkoutIdsOfExercise(int workoutId) =>
            _context.WeightWorkouts.
                     FirstOrDefault(x => x.Id == workoutId)?.
                     WorkoutDate;
    }
}
