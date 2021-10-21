using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TrainingManager.Data.DTO;
using TrainingManager.Model;

namespace TrainingManager.ViewModel
{
    public class HomeVM : WorkoutManagerBaseVM
    {
        public HomeVM(IApiServices apiServices)
        {
            InitializeCommands();
            ApiServices = apiServices;
            RecentWorkouts = new ObservableCollection<HistoryItemVM>();
            SetupHomeAsync();
            WeightWorkoutMenuSelectedCommand = new DelegateCommand(WeightWorkoutMenuSelectedFunction);
        }

        //PROPERTIES
        private ObservableCollection<HistoryItemVM> _recentWorkouts;
        public ObservableCollection<HistoryItemVM> RecentWorkouts { get => _recentWorkouts; set { _recentWorkouts = value; OnPropertyChanged(); } }

        //COMMANDS
        public DelegateCommand WeightWorkoutMenuSelectedCommand { get; private set; }

        //EVENTS
        public event EventHandler<MessageEventArgs> RecentWorkoutItemSelected;

        private async void SetupHomeAsync()
        {
            try
            {
                var workouts = new List<WeightWorkoutDTO>(await ApiServices.GetWeightWorkoutsAsync());
                RecentWorkouts = new ObservableCollection<HistoryItemVM>(workouts.OrderBy(x => x.WorkoutDate).Take(5).Select(w => new HistoryItemVM(w)));
            }
            catch (Exception)
            {
                InvokeExceptionAllertEvent(this, new MessageEventArgs("Error", "Can't connect to the server."));
            }
        }

        public override void RefreshWorkouts(object sender, EventArgs e)
        {
        }

        protected override void SaveTodayWorkoutFunctionAsync(object obj)
        {
        }

        //PRIVATES
        private async void WeightWorkoutMenuSelectedFunction(object obj)
        {
            var workouts = new List<WeightWorkoutDTO>(await ApiServices.GetWeightWorkoutsAsync());

            if (workouts.Any(x => x.WorkoutGuid.ToString() == ((string)obj)))
            {
                WeightWorkoutDTO workout = workouts.Single(x => x.WorkoutGuid.ToString() == ((string)obj));

                NewWeightWorkout = new WeightWorkoutVM()
                {
                    Id = workout.Id,
                    WorkoutName = workout.WorkoutName,
                    WorkoutDate = workout.WorkoutDate,
                    //TotalExerciseRounds = workout.WeightExercisesDto.FirstOrDefault(x => x.WorkoutId == workout.Id).WeightRoundsDto.Count,
                    TotalWeight = workout.TotalWeight,
                    WorkoutGuid = workout.WorkoutGuid,
                    WorkoutType = workout.WorkoutType,
                    Note = workout.Note,
                    WeightExercises = new ObservableCollection<WeightExerciseVM>(workout.WeightExercisesDto.Select(x => new WeightExerciseVM()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        ExerciseName = x.ExerciseName,
                        Note = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        TotalExerciseRounds = x.WeightRoundsDto.Count(),
                        WeightRounds = new ObservableCollection<WeightRoundVM>(x.WeightRoundsDto.Select(y => new WeightRoundVM()
                        {
                            RoundGuid = y.RoundGuid,
                            RoundNumber = y.RoundNumber,
                            Reps = y.Reps,
                            WeightOfExercise = y.WeightOfExercise
                        })),
                    }))
                };

                RecentWorkoutItemSelected?.Invoke(this, new MessageEventArgs(NewWeightWorkout.WorkoutName, NewWeightWorkout.WorkoutGuid.ToString()));
            }
        }
    }
}
