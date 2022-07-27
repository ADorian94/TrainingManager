using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            NewWeightWorkout = new WeightWorkoutVM
            {
                WeightExercises = new ObservableCollection<WeightExerciseVM>()
            };
            ApiServices = apiServices;
            SetupActivitiesAsync();
            SetupTodayWeightWorkoutAsync();
            SearchCommand = new DelegateCommand(SearchFunction);
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
                        RoundColor = round.Color
                    });
                }

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
            var searchStr = obj.ToString();
            IEnumerable<WeightActivityDTO> foundElements = new ObservableCollection<WeightActivityDTO>();
            IEnumerable<WeightActivityDTO> activities = await ApiServices.GetWeightActivitiesAsync();

            if (!string.IsNullOrEmpty(searchStr))
            {
                string[] searchStrings = searchStr.Trim().Split(' ');
                foundElements = searchStrings.SelectMany(str => activities.Where(x => x.ActivityName.ToUpper().Contains(str.ToUpper())).Select(x => x));
            }
            else
                foundElements = activities.Select(x => x);

            foundElements = foundElements.OrderBy(x => x);
            SavedActivities = new ObservableCollection<WeightActivityVM>();
            int i = 0;

            foreach (var element in foundElements)
                SavedActivities.Add(new WeightActivityVM(element, ++i));
        }
    }
}
