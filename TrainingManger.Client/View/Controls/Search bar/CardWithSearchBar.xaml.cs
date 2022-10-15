using TrainingManager.ViewModel;

namespace TrainingManager.View.Controls
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardWithSearchBar : ContentView
    {
        public CardWithSearchBar()
        {
            InitializeComponent();
        }

        public string CardMainText
        {
            get { return (string)GetValue(CardMainTextProperty); }
            set { SetValue(CardMainTextProperty, value); }
        }

        public static readonly BindableProperty CardMainTextProperty =
            BindableProperty.Create("CardMainText", typeof(string), typeof(CardWithSearchBar), string.Empty);

        public DelegateCommand CardSearchCommand
        {
            get { return (DelegateCommand)GetValue(CardSearchCommandProperty); }
            set { SetValue(CardSearchCommandProperty, value); }
        }

        public static readonly BindableProperty CardSearchCommandProperty =
            BindableProperty.Create("CardSearchCommand", typeof(DelegateCommand), typeof(CardWithSearchBar), defaultValue: default(DelegateCommand), propertyChanged: OnSearchCommandPropertyChanged);

        private static void OnSearchCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CardWithSearchBar headerTemplate && newValue is DelegateCommand command)
                headerTemplate.CardSearchCommand = command;
        }
    }
}