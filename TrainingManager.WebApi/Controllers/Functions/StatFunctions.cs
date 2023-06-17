using System;
using System.Collections.Generic;
using System.Linq;
using TrainingManager.Data;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers.Functions
{
    public class StatFunctions
    {
        private readonly TrainingManagerContext _context;

        public StatFunctions(TrainingManagerContext context)
        {
            _context = context;
        }

        public IEnumerable<(WeightActivityDTO activity, double weight, int reps)> FindMaxMovedWeightsByActivites(IQueryable<WeightExercise> exercises, IQueryable<WeightActivity> activities)
        {
            var result = new List<(WeightActivityDTO activity, double weight, int reps)>();

            foreach (var item in activities)
            {
                var relatedExercises = exercises.Where(x => x.ActivityId == item.Id);

                if (relatedExercises.Count() > 0)
                {
                    List<WeightRound> weightRounds = new List<WeightRound>();

                    foreach (var exercise in relatedExercises)
                        weightRounds = weightRounds.Concat(_context.WeightRounds.Where(x => x.ExerciseId == exercise.Id)).ToList();

                    if (weightRounds.Count() > 0)
                    {
                        (double weight, int reps) max = (weightRounds[0].WeightOfExercise, weightRounds[0].Reps);

                        for (int i = 1; i < weightRounds.Count; i++)
                        {
                            if (max.weight < weightRounds[i].WeightOfExercise)
                            {
                                max = (weightRounds[i].WeightOfExercise, weightRounds[i].Reps);
                            }
                        }

                        result.Add((new WeightActivityDTO()
                        {
                            ActivityName = item.ActivityName,
                            MainMuscleGroup = item.MainMuscleGroup,
                            ActivityGuid = item.ActivityGuid,
                            IsWatched = item.IsWatched
                        },
                        max.weight, max.reps));
                    }
                }
            }

            return result.OrderByDescending(x => x.weight);
        }

        public (double weight, int reps) FindMaxMovedWeightsOfActivity(IQueryable<WeightExercise> exercisesOfActivity)
        {
            if (exercisesOfActivity.Count() > 0)
            {
                var weightRounds = new List<WeightRound>();

                foreach (var exercise in exercisesOfActivity)
                    weightRounds = weightRounds.Concat(_context.WeightRounds.Where(x => x.ExerciseId == exercise.Id)).ToList();

                if (weightRounds.Count() > 0)
                {
                    return weightRounds
                        .Select(x => (x.WeightOfExercise, x.Reps))
                        .Aggregate((0.0, 0), (record, next) =>
                            next.WeightOfExercise > record.Item1 ? next : record);
                }
            }

            return (0.0, 0);
        }

        public IEnumerable<(WeightActivityDTO activity, double weight, int reps)> FindWatchedMaxMovedWeightsByActivites(IQueryable<WeightExercise> exercises, IQueryable<WeightActivity> activities)
          => FindMaxMovedWeightsByActivites(exercises, activities).Where(x => x.activity.IsWatched);

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

        public IEnumerable<(int year, int month, IEnumerable<(DateTime, double)> weight)> CollectMovedWeightsGroupByMonth(IQueryable<WeightWorkout> workouts)
        {
            var allWorkouts = workouts.GroupBy(w => w.WorkoutDate.Year).ToList();
            var resutWeights = new List<(int, int, IEnumerable<(DateTime, double)>)>();

            foreach (var yearWorkouts in allWorkouts)
            {
                var monthWorkouts = yearWorkouts.GroupBy(m => m.WorkoutDate.Month);

                foreach (var monthWorkout in monthWorkouts)
                {
                    var workoutsInTheMonth = new List<(DateTime, double)>();

                    foreach (var workout in monthWorkout)
                    {
                        if (workout.WorkoutDate < DateTime.Now)
                            workoutsInTheMonth.Add((workout.WorkoutDate, workout.TotalWeight));
                    }

                    resutWeights.Add((yearWorkouts.Key, monthWorkout.Key, workoutsInTheMonth));
                }
            }

            return resutWeights;
        }

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
