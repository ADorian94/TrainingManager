using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TrainingManager.Data.DTO;
using TrainingManager.Model;
using Xamarin.Forms;
using XamForms.Controls;

namespace TrainingManager.ViewModel
{
    public class WeightHistoryVM : WorkoutManagerBaseVM
    {
        private Dictionary<DateTime, Guid> _workoutGuids;
        int _batch = 0, _numberOfElements = 20;
        bool _isBusy = false;

        public WeightHistoryVM(IApiServices apiServices)
        {
            ApiServices = apiServices;
            SetupActivitiesAsync();
            SetupManagerAsync();
            WorkoutDateSelected = new DelegateCommand(WorkoutDateSelectedFunction);
            HistoryWorkoutItemSelectedCommand = new DelegateCommand(HistoryWorkoutItemSelectedFunction);
            LoadMoreHistoryItemsCommand = new DelegateCommand(LoadMoreHistoryItemsFunction);
            SearchCommand = new DelegateCommand(SearchFunction);
        }

        protected override async Task SetupManagerAsync()
        {
            try
            {
                CurrentDate = DateTime.Now.ToUniversalTime();
                MovedWeightsByMonth = new ObservableCollection<(int Year, int Month, double Weight)>(await ApiServices.GetMovedWorkoutsGroupByMonth());
                SetMovedWeightsInTheMonth();
                var workouts = new List<WeightWorkoutDTO>(await ApiServices.GetWeightWorkoutsAsync());
                WorkoutDates = new ObservableCollection<SpecialDate>();
                _workoutGuids = new Dictionary<DateTime, Guid>();
                var items = new List<HistoryItemVM>();

                await InitializeHistoryItems();

                foreach (var workout in workouts)
                {
                    var date = new DateTime(workout.WorkoutDate.Year, workout.WorkoutDate.Month, workout.WorkoutDate.Day);
                    WorkoutDates.Add(new SpecialDate(date)
                    {
                        TextColor = Color.FromHex(workout.WorkoutDate.ToUniversalTime() < DateTime.Now.ToUniversalTime() ? "#03A9F9" : "#ff6961"),
                        Selectable = true,
                        FontAttributes = FontAttributes.Bold,
                    });

                    _workoutGuids.Add(date, workout.WorkoutGuid);
                    items.Add(new HistoryItemVM(workout));
                }

                //HistoryWorkoutItems = new ObservableCollection<HistoryItemVM>(items.OrderByDescending(x => x.WorkoutDate.Date));
            }
            catch (Exception ex)
            {
                OnExeptionOccured(new ExceptionArgs(ex));
            }
        }

        private Task InitializeHistoryItems() =>
            Task.Run(async () =>
            {
                var recent = await ApiServices.LoadMoreWorkouts(_batch, _numberOfElements);
                _batch += _numberOfElements;
                HistoryWorkoutItems = new ObservableCollection<HistoryItemVM>(recent.Select(x => new HistoryItemVM(x)));
            });

        private async void LoadMoreHistoryItemsFunction(object obj)
        {
            if (_isBusy)
                return;

            _isBusy = true;
            var recent = await ApiServices.LoadMoreWorkouts(_batch, _numberOfElements);

            foreach (var item in recent)
                HistoryWorkoutItems.Add(new HistoryItemVM(item));

            _batch += _numberOfElements;
            _isBusy = false;
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
        public DelegateCommand LoadMoreHistoryItemsCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }

        //EVENTS
        public event EventHandler<DateTime> WeightWorkoutDateSelected;
        public event EventHandler<MessageEventArgs> HistoryWorkoutItemSelected;
        public event EventHandler WorkoutDeleted;

        //COMMAND FUNCTIONS
        private async void WorkoutDateSelectedFunction(object obj)
        {
            try
            {

                DateTime selectedDateUTC = new DateTime(((DateTime)obj).Ticks, DateTimeKind.Utc);

                if (WorkoutDates.Any(x => x.Date.Year == selectedDateUTC.Year && x.Date.DayOfYear == selectedDateUTC.DayOfYear))
                {
                    WeightWorkoutDTO workout = await ApiServices.GetWeightWorkoutAsync(_workoutGuids[selectedDateUTC].ToString());
                    NewWeightWorkout = WeightWorkoutHelper.WorkoutDTOToVM(workout);
                }
                else
                {
                    NewWeightWorkout = new WeightWorkoutVM(selectedDateUTC);
                }

                WeightWorkoutBookmark = new WeightWorkoutVM(NewWeightWorkout);
                WeightWorkoutDateSelected?.Invoke(this, selectedDateUTC);
            }
            catch (Exception ex)
            {
                OnExeptionOccured(new ExceptionArgs(ex));
            }
        }

        private async void HistoryWorkoutItemSelectedFunction(object obj)
        {
            string guid = (string)obj;

            if (!string.IsNullOrEmpty(guid))
            {
                if (await ApiServices.IsWeightWorkoutExitsAsync(guid))
                {
                    WeightWorkoutDTO workout = await ApiServices.GetWeightWorkoutAsync(guid);
                    NewWeightWorkout = WeightWorkoutHelper.WorkoutDTOToVM(workout);
                    WeightWorkoutBookmark = new WeightWorkoutVM(NewWeightWorkout);
                    HistoryWorkoutItemSelected?.Invoke(this, new MessageEventArgs(NewWeightWorkout.WorkoutName, NewWeightWorkout.WorkoutGuid.ToString()));
                }
            }
        }

        //PRIVATE
        private void UpdateMonthData() => SetMovedWeightsInTheMonth();

        private void SetMovedWeightsInTheMonth() =>
            MovedWeightsInTheMonth = MovedWeightsByMonth != null && MovedWeightsByMonth.Any(x => x.Year == CurrentDate.Year && x.Month == CurrentDate.Month) ?
                MovedWeightsByMonth.Single(x => x.Year == CurrentDate.Year && x.Month == CurrentDate.Month).Weight :
                0.0;

        //PROTETED
        protected override async void SaveWorkoutFunctionAsync(object obj)
        {
            var workoutToSave = WeightWorkoutHelper.WorkoutVMToDTO(NewWeightWorkout);

            if (await ApiServices.IsWeightWorkoutExitsAsync(NewWeightWorkout.WorkoutDate.Year, NewWeightWorkout.WorkoutDate.DayOfYear))
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
            OnRefreshWorkouts(this, EventArgs.Empty);
            InvokeClosePageEvent(this, EventArgs.Empty);
        }

        //PUBLIC
        public async void DeleteWeightWorkoutByStringGuid(string wokroutGuid)
        {
            try
            {
                await ApiServices.DeleteWeightWorkoutAsync(wokroutGuid);
                WorkoutDeleted?.Invoke(this, EventArgs.Empty);
                await SetupManagerAsync();
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
