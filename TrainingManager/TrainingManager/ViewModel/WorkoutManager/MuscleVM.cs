using System;
using System.Collections.ObjectModel;
using TrainingManager.Data;

namespace TrainingManager.ViewModel
{
    public class MuscleVM : ViewModelBase
    {
        //PROPERTIES
        private ObservableCollection<Muscle> _muscles;
        public ObservableCollection<Muscle> Muscles { get => _muscles; set { _muscles = value; OnPropertyChanged(); } }

        //COMMAND
        public DelegateCommand MuscleSelectedCommand { get; set; }

        //EVENT
        public event EventHandler<Muscle> MuscleSelected;

        public MuscleVM()
        {
            Muscles = new ObservableCollection<Muscle>();

            foreach (Muscle muscle in Enum.GetValues(typeof(Muscle)))
                Muscles.Add(muscle);
        }

        protected override void InitializeCommands()
        {
            MuscleSelectedCommand = new DelegateCommand(MuscleSelectedFunction);
        }

        private void MuscleSelectedFunction(object obj)
        {
            bool result = Enum.TryParse((string)obj, out Muscle Parsedcolor);
            Muscle muscle = result ? Parsedcolor : Muscle.Unknown;
            MuscleSelected?.Invoke(this, muscle);
        }
    }
}
