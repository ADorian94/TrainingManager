using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TrainingManager.Data.DTO;
using TrainingManager.Model;
using TrainingManager.Model.Services;
using Xamarin.Forms;
using XamForms.Controls;

namespace TrainingManager.ViewModel
{
    public class WeightHistoryVM : WorkoutManagerBaseVM
    {
        public WeightHistoryVM(ApiServices apiServices)
        {
            ApiServices = apiServices;
            SetupActivitiesAsync();
            SetupHistoryAsync();
            InitializeCommands();
            WorkoutDateSelected = new DelegateCommand(WorkoutDateSelectedFunction);
            HistoryWorkoutItemSelectedCommand = new DelegateCommand(HistoryWorkoutItemSelectedFunction);
            SearchCommand = new DelegateCommand(SearchFunction);
        }

        private async void SetupHistoryAsync()
        {
            try
            {
                var workouts = new List<WeightWorkoutDTO>(await ApiServices.GetWeightWorkoutsAsync());
                WorkoutDates = new ObservableCollection<SpecialDate>();
                HistoryWorkoutItems = new ObservableCollection<HistoryItemVM>();

                foreach (var workout in workouts)
                {
                    WorkoutDates.Add(new SpecialDate(workout.WorkoutDate)
                    {
                        TextColor = Color.FromHex("#03A9F9"),
                        Selectable = true,
                        FontAttributes = FontAttributes.Bold,
                    });

                    HistoryWorkoutItems.Add(new HistoryItemVM(workout));
                }
            }
            catch (Exception)
            {
                InvokeExceptionAllertEvent(this, new MessageEventArgs("Error", "Can't connect to the server."));
            }
        }

        //PROPERTIES
        private ObservableCollection<SpecialDate> _workoutDates;
        public ObservableCollection<SpecialDate> WorkoutDates { get => _workoutDates; set { _workoutDates = value; OnPropertyChanged(); } }

        private ObservableCollection<HistoryItemVM> _historyWorkoutItems;
        public ObservableCollection<HistoryItemVM> HistoryWorkoutItems { get => _historyWorkoutItems; set { _historyWorkoutItems = value; OnPropertyChanged(); } }

        //COMMANDS
        public DelegateCommand WorkoutDateSelected { get; private set; }
        public DelegateCommand HistoryWorkoutItemSelectedCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }

        //EVENTS
        public event EventHandler<DateTime> WeightWorkoutDateSelected;
        public event EventHandler<MessageEventArgs> HistoryWorkoutItemSelected;

        //COMMAND FUNCTIONS
        private async void WorkoutDateSelectedFunction(object obj)
        {
            var workouts = new List<WeightWorkoutDTO>(await ApiServices.GetWeightWorkoutsAsync());

            if (workouts.Any(x => x.WorkoutDate.Year == ((DateTimeEventArgs)obj).DateTime.Year && x.WorkoutDate.DayOfYear == ((DateTimeEventArgs)obj).DateTime.DayOfYear))
            {
                WeightWorkoutDTO workout = workouts.Single(x => x.WorkoutDate.Year == ((DateTimeEventArgs)obj).DateTime.Year &&
                x.WorkoutDate.DayOfYear == ((DateTimeEventArgs)obj).DateTime.DayOfYear);

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

                WeightWorkoutDateSelected?.Invoke(this, ((DateTimeEventArgs)obj).DateTime);
            }
        }

        private async void HistoryWorkoutItemSelectedFunction(object obj)
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

                HistoryWorkoutItemSelected?.Invoke(this, new MessageEventArgs(NewWeightWorkout.WorkoutName));
            }
        }

        public void SearchFunction(object obj)
        {

        }

        //PROTETED
        protected override async void SaveTodayWorkoutFunctionAsync(object obj)
        {
            IEnumerable<WeightWorkoutDTO> weightWorkoutDTOs = await ApiServices.GetWeightWorkoutsAsync();

            await ApiServices.EditWeightWorkoutAsync(new WeightWorkoutDTO
            {
                Id = NewWeightWorkout.Id,
                WorkoutDate = NewWeightWorkout.WorkoutDate,
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
                    Note = x.Note,
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

            if (DateTime.Now.Year == NewWeightWorkout.WorkoutDate.Year && DateTime.Now.DayOfYear == NewWeightWorkout.WorkoutDate.DayOfYear)
                InvokeWorkoutSavedEvent(this, null);
        }

        //PUBLIC
        public override void RefreshWorkouts(object sender, EventArgs e) => SetupHistoryAsync();
    }
}
