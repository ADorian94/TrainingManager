using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TrainingManager.Data.DTO;
using TrainingManager.Model;
using Xamarin.Forms;
using XamForms.Controls;

namespace TrainingManager.ViewModel
{
    public class WeightHistoryVM : WorkoutManagerBaseVM
    {
        public WeightHistoryVM(IApiServices apiServices)
        {
            ApiServices = apiServices;
            SetupActivitiesAsync();
            SetupHistoryAsync();
            WorkoutDateSelected = new DelegateCommand(WorkoutDateSelectedFunction);
            HistoryWorkoutItemSelectedCommand = new DelegateCommand(HistoryWorkoutItemSelectedFunction);
            SearchCommand = new DelegateCommand(SearchFunction);
        }

        private async void SetupHistoryAsync()
        {
            try
            {
                CurrentDate = DateTime.Now.ToUniversalTime();
                MovedWeightsByMonth = new ObservableCollection<(int Year, int Month, double Weight)>(await ApiServices.GetMovedWorkoutsGroupByMonth());
                SetMovedWeightsInTheMonth();
                var workouts = new List<WeightWorkoutDTO>(await ApiServices.GetWeightWorkoutsAsync());
                WorkoutDates = new ObservableCollection<SpecialDate>();
                var items = new List<HistoryItemVM>();

                foreach (var workout in workouts)
                {
                    WorkoutDates.Add(new SpecialDate(workout.WorkoutDate)
                    {
                        TextColor = Color.FromHex(workout.WorkoutDate.ToUniversalTime() < DateTime.Now.ToUniversalTime() ? "#03A9F9" : "#ff6961"),
                        Selectable = true,
                        FontAttributes = FontAttributes.Bold,
                    });

                    items.Add(new HistoryItemVM(workout));
                }

                HistoryWorkoutItems = new ObservableCollection<HistoryItemVM>(items.OrderByDescending(x => x.WorkoutDate.Date));
            }
            catch (Exception ex)
            {
                OnExeptionOccured(new ExceptionArgs(ex));
            }
        }

        //PROPERTIES
        private ObservableCollection<SpecialDate> _workoutDates;
        public ObservableCollection<SpecialDate> WorkoutDates { get => _workoutDates; set { _workoutDates = value; OnPropertyChanged(); } }

        private ObservableCollection<HistoryItemVM> _historyWorkoutItems;
        public ObservableCollection<HistoryItemVM> HistoryWorkoutItems { get => _historyWorkoutItems; set { _historyWorkoutItems = value; OnPropertyChanged(); } }

        private DateTime _currentDate;
        public DateTime CurrentDate { get => _currentDate; set { _currentDate = value; OnPropertyChanged(); UpdateMonthData(); } }

        private ObservableCollection<(int Year, int Month, double Weight)> _movedWeightsByMonth;
        public ObservableCollection<(int Year, int Month, double Weight)> MovedWeightsByMonth { get => _movedWeightsByMonth; set { _movedWeightsByMonth = value; OnPropertyChanged(); } }

        private double _movedWeightsInTheMonth;
        public double MovedWeightsInTheMonth { get => _movedWeightsInTheMonth; set { _movedWeightsInTheMonth = value; OnPropertyChanged(); } }

        //COMMANDS
        public DelegateCommand WorkoutDateSelected { get; private set; }
        public DelegateCommand HistoryWorkoutItemSelectedCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }

        //EVENTS
        public event EventHandler<DateTime> WeightWorkoutDateSelected;
        public event EventHandler<MessageEventArgs> HistoryWorkoutItemSelected;
        public event EventHandler WorkoutDeleted;

        //COMMAND FUNCTIONS
        private async void WorkoutDateSelectedFunction(object obj)
        {
            DateTime selectedDateUTC = new DateTime(((DateTime)obj).Ticks, DateTimeKind.Utc);
            var workouts = new List<WeightWorkoutDTO>(await ApiServices.GetWeightWorkoutsAsync());

            if (workouts.Any(x => x.WorkoutDate.Year == selectedDateUTC.Year && x.WorkoutDate.DayOfYear == selectedDateUTC.DayOfYear))
            {
                WeightWorkoutDTO workout = workouts.Single(x => x.WorkoutDate.Year == selectedDateUTC.Year &&
                x.WorkoutDate.DayOfYear == selectedDateUTC.DayOfYear);

                NewWeightWorkout = new WeightWorkoutVM()
                {
                    Id = workout.Id,
                    WorkoutName = workout.WorkoutName,
                    WorkoutDate = workout.WorkoutDate,
                    TotalWeight = workout.TotalWeight,
                    WorkoutGuid = workout.WorkoutGuid,
                    WorkoutType = workout.WorkoutType,
                    Note = workout.Note,
                    WeightExercises = new ObservableCollection<WeightExerciseVM>(workout.WeightExercisesDto.Select(x => new WeightExerciseVM()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        ExerciseName = x.ExerciseName,
                        ExerciseNote = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        TotalExerciseRounds = x.WeightRoundsDto.Count(),
                        ExerciseColor = x.Color,
                        MainMuscle = x.MainMuscleGroup,
                        WeightRounds = new ObservableCollection<WeightRoundVM>(x.WeightRoundsDto.Select(y => new WeightRoundVM()
                        {
                            RoundGuid = y.RoundGuid,
                            RoundNumber = y.RoundNumber,
                            Reps = y.Reps,
                            WeightOfExercise = y.WeightOfExercise,
                            RoundColor = y.Color
                        })),
                    }))
                };
            }
            else
            {
                NewWeightWorkout = new WeightWorkoutVM(selectedDateUTC);
            }

            WeightWorkoutBookmark = new WeightWorkoutVM(NewWeightWorkout);
            WeightWorkoutDateSelected?.Invoke(this, selectedDateUTC);
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
                    TotalWeight = workout.TotalWeight,
                    WorkoutGuid = workout.WorkoutGuid,
                    WorkoutType = workout.WorkoutType,
                    Note = workout.Note,
                    WeightExercises = new ObservableCollection<WeightExerciseVM>(workout.WeightExercisesDto.Select(x => new WeightExerciseVM()
                    {
                        ExerciseGuid = x.ExerciseGuid,
                        ExerciseName = x.ExerciseName,
                        ExerciseNote = x.Note,
                        TotalExerciseWeight = x.TotalExerciseWeight,
                        TotalExerciseRounds = x.WeightRoundsDto.Count(),
                        ExerciseColor = x.Color,
                        MainMuscle = x.MainMuscleGroup,
                        WeightRounds = new ObservableCollection<WeightRoundVM>(x.WeightRoundsDto.Select(y => new WeightRoundVM()
                        {
                            RoundGuid = y.RoundGuid,
                            RoundNumber = y.RoundNumber,
                            Reps = y.Reps,
                            WeightOfExercise = y.WeightOfExercise,
                            RoundColor = y.Color
                        })),
                    }))
                };

                WeightWorkoutBookmark = new WeightWorkoutVM(NewWeightWorkout);
                HistoryWorkoutItemSelected?.Invoke(this, new MessageEventArgs(NewWeightWorkout.WorkoutName, NewWeightWorkout.WorkoutGuid.ToString()));
            }
        }

        //PRIVATE
        private void UpdateMonthData() => SetMovedWeightsInTheMonth();

        private void SetMovedWeightsInTheMonth()
        {
            if (MovedWeightsByMonth != null)
            {
                if (MovedWeightsByMonth.Any(x => x.Year == CurrentDate.Year && x.Month == CurrentDate.Month))
                {
                    MovedWeightsInTheMonth = MovedWeightsByMonth.Single(x => x.Year == CurrentDate.Year && x.Month == CurrentDate.Month).Weight;
                    return;
                }
            }

            MovedWeightsInTheMonth = 0;
        }

        //PROTETED
        protected override async void SaveTodayWorkoutFunctionAsync(object obj)
        {
            IEnumerable<WeightWorkoutDTO> weightWorkoutDTOs = await ApiServices.GetWeightWorkoutsAsync();

            var workoutToSave = new WeightWorkoutDTO
            {
                WorkoutDate = NewWeightWorkout.WorkoutDate,
                TotalWeight = NewWeightWorkout.TotalWeight,
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

            if (weightWorkoutDTOs != null && weightWorkoutDTOs.Any(x => x.WorkoutDate.Date == NewWeightWorkout.WorkoutDate.Date))
            {
                workoutToSave.Id = NewWeightWorkout.Id;
                workoutToSave.WorkoutGuid = NewWeightWorkout.WorkoutGuid;
                await ApiServices.EditWeightWorkoutAsync(workoutToSave);
            }
            else
            {
                workoutToSave.WorkoutGuid = Guid.NewGuid();
                await ApiServices.AddWeightWorkoutAsync(workoutToSave);
                NewWeightWorkout.Id = workoutToSave.Id;
            }

            WeightWorkoutBookmark = new WeightWorkoutVM(NewWeightWorkout);
            CheckChangesAndSetResult();
            InvokeWorkoutSavedEvent(this, EventArgs.Empty);
            RefreshWorkouts(this, EventArgs.Empty);
            InvokeClosePageEvent(this, EventArgs.Empty);
        }

        //PUBLIC
        public override void RefreshWorkouts(object sender, EventArgs e) => SetupHistoryAsync();

        public async void DeleteWeightWorkoutByStringGuid(string wokroutGuid)
        {
            try
            {
                var workouts = await ApiServices.GetWeightWorkoutsAsync();
                await ApiServices.DeleteWeightWorkoutAsync(workouts.Single(x => x.WorkoutGuid.ToString() == wokroutGuid).Id);
                WorkoutDeleted?.Invoke(this, EventArgs.Empty);
                SetupHistoryAsync();
            }
            catch (Exception ex)
            {
                OnExeptionOccured(new ExceptionArgs(ex));
            }
        }

        public async void SearchFunction(object obj)
        {
            var keyWords = obj.ToString();
            IEnumerable<WeightWorkoutDTO> workouts = string.IsNullOrEmpty(keyWords) ?
                await ApiServices.GetWeightWorkoutsAsync() :
                await ApiServices.SearchWorkoutAsync(keyWords);

            HistoryWorkoutItems = new ObservableCollection<HistoryItemVM>(workouts.Select(x => new HistoryItemVM(x)));
        }
    }
}
