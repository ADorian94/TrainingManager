
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardWithEntry : ContentView
    {
        public CardWithEntry()
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

        public double CardEntry
        {
            get { return (double)GetValue(CardEntryProperty); }
            set { SetValue(CardEntryProperty, value); }
        }

        public static readonly BindableProperty CardEntryProperty =
            BindableProperty.Create("CardEntry", typeof(double), typeof(CardWithEntry), 0.0);
    }
}