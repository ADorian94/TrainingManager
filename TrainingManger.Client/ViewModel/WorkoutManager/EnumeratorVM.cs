using System.Collections.ObjectModel;

namespace TrainingManager.ViewModel.WorkoutManager
{
    public class EnumeratorVM<T> : ViewModelBase where T : struct
    {
        //PROPERTIES
        private ObservableCollection<T> _enumContainer;
        public ObservableCollection<T> EnumContainer { get => _enumContainer; set { _enumContainer = value; OnPropertyChanged(); } }

        public T ExtremalItem { get; private set; }

        //COMMAND
        public DelegateCommand ItemSelectedCommand { get; set; }

        //EVENT
        public event EventHandler<T> ItemSelected;

        public EnumeratorVM(T extremalItem)
        {
            EnumContainer = new ObservableCollection<T>();
            ExtremalItem = extremalItem;

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (!item.Equals(ExtremalItem))
                    EnumContainer.Add(item);
            }
        }

        protected override void InitializeCommands()
        {
            ItemSelectedCommand = new DelegateCommand(ItemSelectedFunction);
        }

        private void ItemSelectedFunction(object obj)
        {
            bool result = Enum.TryParse((string)obj, out T parsedValue);
            T item = result ? parsedValue : ExtremalItem;
            ItemSelected?.Invoke(this, item);
        }
    }
}
