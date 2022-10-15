namespace TrainingManager.View.Controls
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardWithTextEntry : ContentView
    {
        public CardWithTextEntry()
        {
            InitializeComponent();
        }

        public string CardMainText
        {
            get { return (string)GetValue(CardMainTextProperty); }
            set { SetValue(CardMainTextProperty, value); }
        }

        public static readonly BindableProperty CardMainTextProperty =
            BindableProperty.Create("CardMainText", typeof(string), typeof(CardWithEntry), string.Empty);

        public string CardEntry
        {
            get { return (string)GetValue(CardEntryProperty); }
            set { SetValue(CardEntryProperty, value); }
        }

        public static readonly BindableProperty CardEntryProperty =
            BindableProperty.Create("CardEntry", typeof(string), typeof(CardWithEntry), string.Empty);
    }
}