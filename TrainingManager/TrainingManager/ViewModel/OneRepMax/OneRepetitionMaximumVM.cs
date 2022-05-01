using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TrainingManager.Model;

namespace TrainingManager.ViewModel
{
    public class OneRepetitionMaximumVM : ViewModelBase
    {
        //FIELDS
        private IApiServices _apiServices;
        private OneRepetitionMaximumModel _model;

        //EVENTS
        public event EventHandler CalculationStartEvent;

        //PROPERTIES
        public ObservableCollection<ObservableCollection<(DateTime date, double Weight)>> GroupedWorkouts { get; set; }

        //COMMANDS
        public DelegateCommand CalculateMaximumCommand { get; private set; }
        public DelegateCommand SetupCommand { get; private set; }

        public OneRepetitionMaximumVM(IApiServices apiServices)
        {
            _model = new OneRepetitionMaximumModel();
            _apiServices = apiServices;
            RecomendedMaximums = new ObservableCollection<MaximumMethod>();
            GetWorkoutDetailsFromServer();
        }

        protected override void InitializeCommands()
        {
            CalculateMaximumCommand = new DelegateCommand(CalculateMaximum);
        }

        private double _weight = 0;
        public double Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged();
            }
        }

        private int _reps = 0;
        public int Reps
        {
            get => _reps;
            set
            {
                _reps = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MaximumMethod> _recomendedMaximums;
        public ObservableCollection<MaximumMethod> RecomendedMaximums
        {
            get => _recomendedMaximums;
            set
            {
                _recomendedMaximums = value;
                OnPropertyChanged();
            }
        }

        private async void GetWorkoutDetailsFromServer()
        {
            IEnumerable<(int year, int month, IEnumerable<(DateTime date, double weight)>)> workouts = await _apiServices.GetMovedWeightsGroupByMonth();
            GroupedWorkouts = new ObservableCollection<ObservableCollection<(DateTime date, double Weight)>>();

            foreach (var workout in workouts)
                GroupedWorkouts.Add(new ObservableCollection<(DateTime date, double Weight)>(workout.Item3));
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
    }
}
