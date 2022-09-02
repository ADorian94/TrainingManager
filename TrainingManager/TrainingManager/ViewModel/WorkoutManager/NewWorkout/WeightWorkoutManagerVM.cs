using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TrainingManager.Data.DTO;
using TrainingManager.Model;
using TrainingManager.Model.LogWriter;
using TrainingManager.ViewModel.WorkoutManager;

namespace TrainingManager.ViewModel
{
    public class WeightWorkoutManagerVM : WorkoutManagerBaseVM
    {
        public DelegateCommand SearchCommand { get; private set; }

        public WeightWorkoutManagerVM(IApiServices apiServices)
        {
            NewWeightWorkout = new WeightWorkoutVM;
            ApiServices = apiServices;
            SetupActivitiesAsync();
            SetupManagerAsync();
            SearchCommand = new DelegateCommand(SearchFunction);
        }

        //PRIVATES
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
                var rounds = new ObservableCollection<WeightRoundVM>(
                    exercise.WeightRoundsDto.Select(round => new WeightRoundVM()
                    {
                        Reps = round.Reps,
                        RoundGuid = round.RoundGuid,
                        RoundNumber = round.RoundNumber,
                        WeightOfExercise = round.WeightOfExercise,
                        RoundColor = round.Color
                    }));

                NewWeightWorkout.WeightExercises.Add(new WeightExerciseVM()
                {
                    ExerciseNote = exercise.Note,
                    ExerciseGuid = exercise.ExerciseGuid,
                    ExerciseName = exercise.ExerciseName,
                    TotalExerciseRounds = exercise.WeightRoundsDto.Count,
                    TotalExerciseWeight = exercise.TotalExerciseWeight,
                    ExerciseColor = exercise.Color,
                    MainMuscle = exercise.MainMuscleGroup,
                    WeightRounds = new ObservableCollection<WeightRoundVM>(rounds),
                });
            }

            LogHandler.Instance.Nlog.Info("New workout readed from server.");
        }

        //PROTECTED
        protected override async Task SetupManagerAsync()
        {
            try
            {
                IEnumerable<WeightWorkoutDTO> weightWorkoutDTOs = await ApiServices.GetWeightWorkoutsAsync();

                if (weightWorkoutDTOs != null && weightWorkoutDTOs.Any(x => x.WorkoutDate.Date == DateTime.Now.Date))
                    SetupTodayWeightWorkoutDetails(weightWorkoutDTOs);
                else
                {
                    NewWeightWorkout = new WeightWorkoutVM();
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

        protected override async void SaveWorkoutFunctionAsync(object obj)
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
                    WorkoutDate = DateTime.Now.ToUniversalTime(),
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
                        Color = x.ExerciseColor,
                        MainMuscleGroup = x.MainMuscle,
                        WeightRoundsDto = new List<WeightRoundDTO>(x.WeightRounds.Select(y => new WeightRoundDTO()
                        {
                            Reps = y.Reps,
                            RoundGuid = y.RoundGuid,
                            RoundNumber = y.RoundNumber,
                            WeightOfExercise = y.WeightOfExercise,
                            Color = y.RoundColor
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
                    WorkoutDate = DateTime.Now.ToUniversalTime(),
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
                        Color = x.ExerciseColor,
                        MainMuscleGroup = x.MainMuscle,
                        WeightRoundsDto = new List<WeightRoundDTO>(x.WeightRounds.Select(y => new WeightRoundDTO()
                        {
                            Reps = y.Reps,
                            RoundGuid = y.RoundGuid,
                            RoundNumber = y.RoundNumber,
                            WeightOfExercise = y.WeightOfExercise,
                            Color = y.RoundColor
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

        private async void SearchFunction(object obj)
        {
            var keyWords = obj.ToString();
            IEnumerable<WeightActivityDTO> workouts = string.IsNullOrEmpty(keyWords) ?
                await ApiServices.GetWeightActivitiesAsync() :
                await ApiServices.SearchActivityAsync(keyWords);
            int i = 0;

            SavedActivities = new ObservableCollection<WeightActivityVM>(workouts.Select(x => new WeightActivityVM(x, ++i)));
        }
    }
}
