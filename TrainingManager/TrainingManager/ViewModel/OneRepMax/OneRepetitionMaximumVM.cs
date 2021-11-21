using System;
using System.Collections.ObjectModel;
using TrainingManager.Model;

namespace TrainingManager.ViewModel
{
    public class OneRepetitionMaximumVM : ViewModelBase
    {
        private OneRepetitionMaximumModel _model;
        public event EventHandler CalculationStartEvent;

        public DelegateCommand CalculateMaximumCommand { get; private set; }
        public DelegateCommand SetupCommand { get; private set; }

        public OneRepetitionMaximumVM()
        {
            _model = new OneRepetitionMaximumModel();
            RecomendedMaximums = new ObservableCollection<MaximumMethod>();
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
