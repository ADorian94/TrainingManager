using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Utility
{
    public static class PersonalRecordUpdater
    {
        private static TrainingManagerContext _context;

        public async static void Update(IApplicationBuilder builder)
        {
            var scope = builder.ApplicationServices.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<TrainingManagerContext>();
            var workoutsByUsers = _context.WeightWorkouts.GroupBy(x => x.OwnerUserName);

            foreach (var workoutsByUser in workoutsByUsers)
            {
                var workouts = workoutsByUser.OrderBy(x => x.WorkoutDate);
                await UpdateUsersPersonalRecords(workouts, workoutsByUser.Key);
            }
        }

        private static async Task UpdateUsersPersonalRecords(IOrderedEnumerable<WeightWorkout> workouts, string ownerUser)
        {
            if (IsUpdateRequired(workouts.Any(), ownerUser))
            {
                foreach (var workout in workouts)
                {
                    var workoutExercises = _context.WeightExercises.Where(x => x.WorkoutId == workout.Id);

                    foreach (var exercise in workoutExercises)
                    {
                        await AddActivityToPersonalRecordTable(exercise, workout.Id, workout.OwnerUserName, workout.WorkoutDate);
                    }
                }
            }
        }

        private static async Task AddActivityToPersonalRecordTable(WeightExercise exercise, int wrokoutId, string ownerUser, DateTime workoutDate)
        {
            if (_context.WeightActivities.Any(x => x.Id == exercise.ActivityId))
            {
                WeightActivity activity = _context.WeightActivities.Single(x => x.Id == exercise.ActivityId);
                IList<WeightRound> rounds = _context.WeightRounds.Where(x => x.ExerciseId == exercise.Id).ToList();

                if (rounds.Count() > 0)
                {
                    WeightRound roundWithMaxweight = GetRoundByMaxWeight(rounds);

                    if (_context.PersonalRecords.Any(x => x.ActivityId == activity.Id))
                    {
                        var record = _context.PersonalRecords
                            .Where(x => x.ActivityId == activity.Id)
                            .OrderByDescending(y => y.WeightOfPersonalRecord)
                            .FirstOrDefault();

                        if (record.WeightOfPersonalRecord < roundWithMaxweight.WeightOfExercise ||
                            (record.WeightOfPersonalRecord == roundWithMaxweight.WeightOfExercise &&
                            record.RepsOfPersonalRecord < roundWithMaxweight.Reps))
                        {
                            await AddAndSavePersonslRecordToDBAsync(wrokoutId, activity.Id, activity.ActivityGuid, ownerUser, workoutDate, roundWithMaxweight.Reps, roundWithMaxweight.WeightOfExercise);
                        }
                    }
                    else
                        await AddAndSavePersonslRecordToDBAsync(wrokoutId, activity.Id, activity.ActivityGuid, ownerUser, workoutDate, roundWithMaxweight.Reps, roundWithMaxweight.WeightOfExercise);
                }
            }
        }

        private static bool IsUpdateRequired(bool hasAnyWorkouts, string workoutOwner) => hasAnyWorkouts && !_context.PersonalRecords.Where(x => x.OwnerUserName == workoutOwner).Any();

        private static async Task AddAndSavePersonslRecordToDBAsync(int workoutId, int activityId, Guid activityGuid, string ownerUserName, DateTime workoutDate, int reps, double weightOfExercise)
        {
            await _context.PersonalRecords.AddAsync(new PersonalRecord()
            {
                WorkoutId = workoutId,
                ActivityId = activityId,
                ActivityGuid = activityGuid,
                PersonalRecordGuid = Guid.NewGuid(),
                OwnerUserName = ownerUserName,
                PersonalRecordDate = workoutDate,
                RepsOfPersonalRecord = reps,
                WeightOfPersonalRecord = weightOfExercise
            });

            await _context.SaveChangesAsync();
        }

        private static WeightRound GetRoundByMaxWeight(IList<WeightRound> rounds)
        {
            WeightRound result = rounds[0];

            for (int i = 1; i < rounds.Count(); ++i)
            {
                if (result.WeightOfExercise < rounds[i].WeightOfExercise)
                    result = rounds[i];
            }

            return result;
        }
    }
}
