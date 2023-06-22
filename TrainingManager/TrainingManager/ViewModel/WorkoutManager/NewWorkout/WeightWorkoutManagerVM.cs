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
            NewWeightWorkout = new WeightWorkoutVM();
            ApiServices = apiServices;
            SetupActivitiesAsync();
            SetupManagerAsync();
            SearchCommand = new DelegateCommand(SearchFunction);
        }

        //PRIVATES
        /// <summary>
        /// A szerverről lekérdezzük a mai naphoz tartozó edzést. 
        /// </summary>
        private async void SetupTodayWeightWorkoutDetails(int year, int dayOfYear)
        {
            var weightWorkoutDTO = await ApiServices.GetWeightWorkoutAsync(year, dayOfYear);

            NewWeightWorkout = new WeightWorkoutVM()
            {
                Id = weightWorkoutDTO.Id,
                Note = weightWorkoutDTO.Note,
                TotalExerciseRounds = weightWorkoutDTO.WeightExercisesDto.Count,
                TotalWeight = weightWorkoutDTO.TotalWeight,
                WorkoutDate = weightWorkoutDTO.WorkoutDate,
                WorkoutGuid = Guid.Empty.ToString() == weightWorkoutDTO.WorkoutGuid.ToString() ? Guid.NewGuid() : weightWorkoutDTO.WorkoutGuid,
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
                var today = DateTime.Now.ToUniversalTime();

                if (await ApiServices.IsWeightWorkoutExitsAsync(today.Year, today.DayOfYear))
                    SetupTodayWeightWorkoutDetails(today.Year, today.DayOfYear);
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

            var workoutToSave = WeightWorkoutHelper.WorkoutVMToDTO(NewWeightWorkout);
            bool isWorkoutExitsToday = await ApiServices.IsWeightWorkoutExitsAsync(workoutToSave.WorkoutDate.Year, workoutToSave.WorkoutDate.DayOfYear);

            //Meglévő edzés szerkesztése
            if (isWorkoutExitsToday)
            {
                workoutToSave.Id = NewWeightWorkout.Id;
                await ApiServices.EditWeightWorkoutAsync(workoutToSave);
                LogHandler.Instance.Nlog.Info("Existing workout edited.");
            }
            //Új edzés létrehozása
            else
            {
                //ha még nincs a mai naphoz workout, akkor létrehozzuk és feltöltjük
                await ApiServices.AddWeightWorkoutAsync(workoutToSave);
                NewWeightWorkout.Id = workoutToSave.Id;
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

            SavedActivities = new ObservableCollection<WeightActivityVM>(workouts.Select(x => new WeightActivityVM(x, i++)));
        }

    }
}
