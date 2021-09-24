using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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

        public string CardPlaceholder
        {
            get { return (string)GetValue(CardPlaceholderProperty); }
            set { SetValue(CardPlaceholderProperty, value); }
        }

        public static readonly BindableProperty CardPlaceholderProperty =
            BindableProperty.Create("CardPlaceholder", typeof(string), typeof(CardWithSearchBar), string.Empty);

        public string CardSearchText
        {
            get { return (string)GetValue(CardSearchTextProperty); }
            set { SetValue(CardSearchTextProperty, value); }
        }

        public static readonly BindableProperty CardSearchTextProperty =
            BindableProperty.Create("CardSearchText", typeof(string), typeof(CardWithSearchBar), string.Empty);

        public DelegateCommand CardSearchCommand
        {
            get { return (DelegateCommand)GetValue(CardSearchCommandProperty); }
            set { SetValue(CardSearchCommandProperty, value); }
        }

        public static readonly BindableProperty CardSearchCommandProperty =
            BindableProperty.Create("CardSearchCommand", typeof(DelegateCommand), typeof(CardWithSearchBar), defaultValue: default(DelegateCommand), propertyChanged: OnFrameTapCommandPropertyChanged);

        private static void OnFrameTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CardWithSearchBar headerTemplate && newValue is DelegateCommand command)
                headerTemplate.CardSearchCommand = command;
        }
    }
}