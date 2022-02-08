using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TrainingManager.Data.DTO;
using TrainingManager.Model;
using TrainingManager.Model.LogWriter;

namespace TrainingManager.ViewModel
{
    public class WeightWorkoutManagerVM : WorkoutManagerBaseVM
    {
        public WeightWorkoutManagerVM(IApiServices apiServices)
        {
            NewWeightWorkout = new WeightWorkoutVM
            {
                WeightExercises = new ObservableCollection<WeightExerciseVM>()
            };
            ApiServices = apiServices;
            SetupActivitiesAsync();
            SetupTodayWeightWorkoutAsync();
        }

        public override void RefreshWorkouts(object sender, EventArgs e) => SetupTodayWeightWorkoutAsync();

        //PRIVATES
        private async void SetupTodayWeightWorkoutAsync()
        {
            try
            {
                IEnumerable<WeightWorkoutDTO> weightWorkoutDTOs = await ApiServices.GetWeightWorkoutsAsync();

                if (weightWorkoutDTOs != null && weightWorkoutDTOs.Any(x => x.WorkoutDate.Date == DateTime.Now.Date))
                    SetupTodayWeightWorkoutDetails(weightWorkoutDTOs);
                else
                {
                    NewWeightWorkout = new WeightWorkoutVM()
                    {
                        Note = string.Empty,
                        TotalExerciseRounds = 0.0,
                        TotalWeight = 0.0,
                        WorkoutDate = DateTime.Now,
                        WorkoutGuid = Guid.NewGuid(),
                        WorkoutName = string.Empty,
                        WorkoutType = WorkoutType.WeightWorkout,
                        WeightExercises = new ObservableCollection<WeightExerciseVM>(),
                    };

                    LogHandler.Instance.Nlog.Info("Empty new workout created.");
                }

                WeightWorkoutBookmark = new WeightWorkoutVM(NewWeightWorkout);
            }
            catch (Exception ex)
            {
                LogHandler.Instance.Nlog.Error(ex.Message);
                OnExeptionOccured(new ExceptionArgs(ex));
            }
        }

        /// <summary>
        /// A szerverről lekérdezzük a mai naphoz tartozó edzést. 
        /// </summary>
        private void SetupTodayWeightWorkoutDetails(IEnumerable<WeightWorkoutDTO> workoutsFromServer)
        {
            WeightWorkoutDTO weightWorkoutDTO = workoutsFromServer.FirstOrDefault(x => x.WorkoutDate.Date == DateTime.Now.Date);

            NewWeightWorkout = new WeightWorkoutVM()
            {
                Id = weightWorkoutDTO.Id,
                Note = weightWorkoutDTO.Note,
                TotalExerciseRounds = weightWorkoutDTO.WeightExercisesDto.Count,
                TotalWeight = weightWorkoutDTO.TotalWeight,
                WorkoutDate = weightWorkoutDTO.WorkoutDate,
                WorkoutGuid = weightWorkoutDTO.WorkoutGuid,
                WorkoutName = weightWorkoutDTO.WorkoutName,
                WorkoutType = WorkoutType.WeightWorkout,
                WeightExercises = new ObservableCollection<WeightExerciseVM>(),
            };

            foreach (var exercise in weightWorkoutDTO.WeightExercisesDto)
            {
                var rounds = new ObservableCollection<WeightRoundVM>();

                foreach (var round in exercise.WeightRoundsDto)
                {
                    rounds.Add(new WeightRoundVM()
                    {
                        Reps = round.Reps,
                        RoundGuid = round.RoundGuid,
                        RoundNumber = round.RoundNumber,
                        WeightOfExercise = round.WeightOfExercise,
                    });
                }

                NewWeightWorkout.WeightExercises.Add(new WeightExerciseVM()
                {
                    ExerciseNote = exercise.Note,
                    ExerciseGuid = exercise.ExerciseGuid,
                    ExerciseName = exercise.ExerciseName,
                    TotalExerciseRounds = exercise.WeightRoundsDto.Count,
                    TotalExerciseWeight = exercise.TotalExerciseWeight,
                    WeightRounds = new ObservableCollection<WeightRoundVM>(rounds),
                });
            }

            LogHandler.Instance.Nlog.Info("New workout readed from server.");
        }

        //PROTECTED
        protected override async void SaveTodayWorkoutFunctionAsync(object obj)
        {
            if (!IsReadyReadyToSave())
                return;

            IEnumerable<WeightWorkoutDTO> weightWorkoutDTOs = await ApiServices.GetWeightWorkoutsAsync();

            //Meglévő edzés szerkesztése
            if (weightWorkoutDTOs != null && weightWorkoutDTOs.Any(x => x.WorkoutDate.Date == DateTime.Now.Date))
            {
                await ApiServices.EditWeightWorkoutAsync(new WeightWorkoutDTO
                {
                    Id = NewWeightWorkout.Id,
                    WorkoutDate = DateTime.Now,
                    TotalWeight = NewWeightWorkout.TotalWeight,
                    WorkoutName = NewWeightWorkout.WorkoutName,
                    WorkoutGuid = NewWeightWorkout.WorkoutGuid,
                    Note = NewWeightWorkout.Note,
                    WorkoutType = WorkoutType.WeightWorkout,
                    WorkoutImages = null,
                    WeightExercisesDto = new List<WeightExerciseDTO>(NewWeightWorkout.WeightExercises.Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        ExerciseName = x.ExerciseName,
                        Note = x.ExerciseNote,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        WeightRoundsDto = new List<WeightRoundDTO>(x.WeightRounds.Select(y => new WeightRoundDTO()
                        {
                            Reps = y.Reps,
                            RoundGuid = y.RoundGuid,
                            RoundNumber = y.RoundNumber,
                            WeightOfExercise = y.WeightOfExercise
                        })),
                    })),
                });

                LogHandler.Instance.Nlog.Info("Existing workout edited.");
            }
            //Új edzés létrehozása
            else
            {
                //ha még nincs a mai naphoz workout, akkor létrehozzuk és feltöltjük
                var newWorkout = new WeightWorkoutDTO
                {
                    WorkoutDate = DateTime.Now,
                    TotalWeight = NewWeightWorkout.TotalWeight,
                    WorkoutGuid = Guid.NewGuid(),
                    WorkoutName = NewWeightWorkout.WorkoutName,
                    Note = NewWeightWorkout.Note,
                    WorkoutType = WorkoutType.WeightWorkout,
                    WorkoutImages = null,
                    WeightExercisesDto = new List<WeightExerciseDTO>(NewWeightWorkout.WeightExercises.Select(x => new WeightExerciseDTO()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        ExerciseName = x.ExerciseName,
                        Note = x.ExerciseNote,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        WeightRoundsDto = new List<WeightRoundDTO>(x.WeightRounds.Select(y => new WeightRoundDTO()
                        {
                            Reps = y.Reps,
                            RoundGuid = y.RoundGuid,
                            RoundNumber = y.RoundNumber,
                            WeightOfExercise = y.WeightOfExercise
                        })),
                    })),
                };
                await ApiServices.AddWeightWorkoutAsync(newWorkout);
                NewWeightWorkout.Id = newWorkout.Id;
                LogHandler.Instance.Nlog.Info("New workout saved.");
            }

            WeightWorkoutBookmark = new WeightWorkoutVM(NewWeightWorkout);
            CheckChangesAndSetResult();
            InvokeWorkoutSavedEvent(this, null);
        }
    }
}
