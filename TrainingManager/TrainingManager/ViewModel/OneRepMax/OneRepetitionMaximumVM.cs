using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TrainingManager.Data.DTO;
using TrainingManager.Model;

namespace TrainingManager.ViewModel
{
    public class OneRepetitionMaximumVM : ViewModelBase
    {
        //FIELDS
        private IApiServices _apiServices;
        private OneRepetitionMaximumModel _model;
        private Action<PersonalRecordCardVM> _recordSelection;

        //EVENTS
        public event EventHandler CalculationStartEvent;

        //PROPERTIES
        private ObservableCollection<ObservableCollection<(DateTime date, double Weight)>> _groupedWorkouts;
        public ObservableCollection<ObservableCollection<(DateTime date, double Weight)>> GroupedWorkouts { get => _groupedWorkouts; set { _groupedWorkouts = value; OnPropertyChanged(); } }

        private DateTime _date;
        public DateTime Date { get => _date; set { _date = value; OnPropertyChanged(); } }

        private ObservableCollection<PersonalRecordCardVM> _personalRecords;
        public ObservableCollection<PersonalRecordCardVM> PersonalRecords { get => _personalRecords; set { _personalRecords = value; OnPropertyChanged(); } }

        //COMMANDS
        public DelegateCommand CalculateMaximumCommand { get; private set; }

        public OneRepetitionMaximumVM(IApiServices apiServices, Action<PersonalRecordCardVM> recordSelection)
        {
            _model = new OneRepetitionMaximumModel();
            _apiServices = apiServices;
            RecomendedMaximums = new ObservableCollection<MaximumMethod>();
            GetWorkoutDetailsFromServer();
            InitPersonalRecords();
            Date = DateTime.Now;
            _recordSelection = recordSelection;
        }

        protected override void InitializeCommands()
        {
            CalculateMaximumCommand = new DelegateCommand(CalculateMaximum);
        }

        private double _weight = 0;
        public double Weight { get => _weight; set { _weight = value; OnPropertyChanged(); } }

        private int _reps = 0;
        public int Reps { get => _reps; set { _reps = value; OnPropertyChanged(); } }

        private ObservableCollection<MaximumMethod> _recomendedMaximums;
        public ObservableCollection<MaximumMethod> RecomendedMaximums { get => _recomendedMaximums; set { _recomendedMaximums = value; OnPropertyChanged(); } }

        public void Refresh(object sender, EventArgs e)
        {
            GetWorkoutDetailsFromServer();
            InitPersonalRecords();
        }

        //PRIVATES
        private async void GetWorkoutDetailsFromServer()
        {
            IEnumerable<YearMonthWorkoutGroupDTO> workouts = (await _apiServices.GetMovedWeightsGroupByMonth());
            GroupedWorkouts = new ObservableCollection<ObservableCollection<(DateTime date, double Weight)>>(
                workouts.Select(
                    workout => new ObservableCollection<(DateTime date, double Weight)>(workout.WorkoutsInMonth.Select(w => (w.Date, w.Weight)))
                ));
        }

        private void CalculateMaximum(object obj)
        {
            if (!IsReadyToCalculate())
                return;

            if (Reps > 10)
                SendPopUpMessage(Messages.MayInvalidReps);

            RecomendedMaximums = new ObservableCollection<MaximumMethod>(_model.CalculateOneRepMaximums(_weight, _reps));
            CalculationStartEvent?.Invoke(this, EventArgs.Empty);
        }

        private bool IsReadyToCalculate()
        {
            if (Weight <= 0)
            {
                SendPopUpMessage(Messages.InvalidWeight);
                return false;
            }

            if (Reps <= 0)
            {
                SendPopUpMessage(Messages.InvalidReps);
                return false;
            }

            return true;
        }

        private async void InitPersonalRecords()
        {
            var records = await _apiServices.GetMaxMovedWeightsByActivites();
            PersonalRecords = new ObservableCollection<PersonalRecordCardVM>(records.Select(x => new PersonalRecordCardVM(x, _recordSelection)));
        }
    }
}
