using System;
using System.Collections.ObjectModel;
using TrainingManager.Model;
using TrainingManager.Model.Services;

namespace TrainingManager.ViewModel
{
    public class OneRepetitionMaximumVM : ViewModelBase
    {
        private OneRepetitionMaximumModel _model;
        public event EventHandler CalculationStartEvent;
        public event EventHandler OpenMasterEvent;

        public DelegateCommand CalculateMaximumCommand { get; private set; }
        public DelegateCommand SetupCommand { get; private set; }

        public OneRepetitionMaximumVM()
        {
            _model = new OneRepetitionMaximumModel();
            RecomendedMaximums = new ObservableCollection<MaximumMethod>();
            InitializeCommands();
        }

        protected override void InitializeCommands()
        {
            CalculateMaximumCommand = new DelegateCommand(CalculateMaximum);
        }

        private void OpenMaster(object obj)
        {
            OpenMasterEvent?.Invoke(this, EventArgs.Empty);
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
            RecomendedMaximums.Clear();
            RecomendedMaximums = _model.CalculateOneRepMaximums(_weight, _reps);
            CalculationStartEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
