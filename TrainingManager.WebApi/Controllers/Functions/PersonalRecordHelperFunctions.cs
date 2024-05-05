using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingManager.Data.DTO;
using TrainingManager.WebApi.Controllers.Functions.Interfaces;
using TrainingManager.WebApi.Data;
using TrainingManager.WebApi.Model;

namespace TrainingManager.WebApi.Controllers.Functions
{
    public class PersonalRecordHelperFunctions : IPersonalRecordHelperFunctions
    {
        private readonly TrainingManagerContext _context;

        public PersonalRecordHelperFunctions(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<TrainingManagerContext>();
        }

        public async Task AddNewPersonalRecordsIfNeeded(WeightWorkout workout)
        {
            var weightExercises = _context.WeightExercises.Where(x => x.WorkoutId == workout.Id);

            foreach (var exercise in weightExercises)
            {
                WeightActivity actualActivity = _context.WeightActivities.Single(x => x.Id == exercise.ActivityId);

                if (IsPersonalRecord(exercise, actualActivity))
                {
                    WeightRound maxWeightRound = GetRoundByMaxWeight(_context.WeightRounds.Where(x => x.ExerciseId == exercise.Id).ToList());

                    await _context.PersonalRecords.AddAsync(new PersonalRecord()
                    {
                        WorkoutId = workout.Id,
                        ActivityId = actualActivity.Id,
                        PersonalRecordGuid = Guid.NewGuid(),
                        OwnerUserName = workout.OwnerUserName,
                        PersonalRecordDate = workout.WorkoutDate,
                        RepsOfPersonalRecord = maxWeightRound.Reps,
                        WeightOfPersonalRecord = maxWeightRound.WeightOfExercise
                    });

                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemovePersonalRecordsByWorkout(WeightWorkout workout)
        {
            var workoutRecords = _context.PersonalRecords.Where(x => x.WorkoutId == workout.Id);

            foreach (var record in workoutRecords)
            {
                _context.PersonalRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePersonalRecordsByWorkout(WeightWorkout workout)
        {
            await RemovePersonalRecordsByWorkout(workout);
            await AddNewPersonalRecordsIfNeeded(workout);
        }

        public PersonalRecordDTO FindMaxMovedWeightsOfActivity(int activityId)
        {
            if (_context.PersonalRecords.Any(x => x.ActivityId == activityId))
            {
                var personalRecord = _context.PersonalRecords.Where(x => x.ActivityId == activityId).OrderByDescending(x => x.PersonalRecordDate).FirstOrDefault();
                return new PersonalRecordDTO()
                {
                    Id = personalRecord.Id,
                    ActivityId = personalRecord.ActivityId,
                    OwnerUserName = personalRecord.OwnerUserName,
                    PersonalRecordDate = personalRecord.PersonalRecordDate,
                    PersonalRecordGuid = personalRecord.PersonalRecordGuid,
                    WorkoutId = personalRecord.WorkoutId,
                    RepsOfPersonalRecord = personalRecord.RepsOfPersonalRecord,
                    WeightOfPersonalRecord = personalRecord.WeightOfPersonalRecord
                };
            }
            else
                return new PersonalRecordDTO() { RepsOfPersonalRecord = 0, WeightOfPersonalRecord = 0.0 };
        }

        public IEnumerable<(WeightActivityDTO activity, double weight, int reps)> FindMaxMovedWeightsByActivites(ApplicationUser user)
        {
            var activityGroups = _context.PersonalRecords.Where(u => u.OwnerUserName == user.UserName).GroupBy(x => x.ActivityId);

            foreach (var activityGroup in activityGroups)
            {
                var userActivites = _context.WeightActivities.Where(u => u.OwnerUserName == user.UserName);

                if (userActivites.Any(x => x.Id == activityGroup.Key))
                {
                    PersonalRecord personalRecod = activityGroup.OrderByDescending(x => x.PersonalRecordDate).FirstOrDefault();
                    WeightActivity activity = userActivites.Single(x => x.Id == activityGroup.Key);

                    if (activity.IsWatched)
                        yield return
                            (new WeightActivityDTO
                            {
                                ActivityGuid = activity.ActivityGuid,
                                ActivityName = activity.ActivityName,
                                IsWatched = activity.IsWatched,
                                MainMuscleGroup = activity.MainMuscleGroup
                            },
                            personalRecod.WeightOfPersonalRecord,
                            personalRecod.RepsOfPersonalRecord);
                }
            }
        }

        private bool IsPersonalRecord(WeightExercise exercise, WeightActivity activity)
        {
            if (_context.PersonalRecords.Any(x => x.ActivityId == activity.Id))
            {
                WeightRound maxWeightRound = GetRoundByMaxWeight(_context.WeightRounds.Where(x => x.ExerciseId == exercise.Id).ToList());

                var record = _context.PersonalRecords
                            .Where(x => x.ActivityId == activity.Id)
                            .OrderByDescending(y => y.WeightOfPersonalRecord)
                            .FirstOrDefault();

                return record.WeightOfPersonalRecord < maxWeightRound.WeightOfExercise ||
                       (record.WeightOfPersonalRecord == maxWeightRound.WeightOfExercise &&
                       record.RepsOfPersonalRecord < maxWeightRound.Reps);
            }
            else
                return true;
        }

        private WeightRound GetRoundByMaxWeight(IList<WeightRound> rounds)
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
