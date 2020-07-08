using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleCard : ContentView
    {
        public SimpleCard()
        {
            InitializeComponent();
        }

        public string CardMainText
        {
            get { return (string)GetValue(CardMainTextProperty); }
            set { SetValue(CardMainTextProperty, value); }
        }

        public static readonly BindableProperty CardMainTextProperty =
            BindableProperty.Create("CardMainText", typeof(string), typeof(SimpleCard), string.Empty);

        public string CardSecondaryText
        {
            get { return (string)GetValue(CardSecondaryTextProperty); }
            set { SetValue(CardSecondaryTextProperty, value); }
        }

        public static readonly BindableProperty CardSecondaryTextProperty =
            BindableProperty.Create("CardSecondaryText", typeof(string), typeof(SimpleCard), string.Empty);

        //public TapGestureRecognizer CardTappedCommand
        //{
        //    get { return (TapGestureRecognizer)GetValue(CardTappedCommandProperty); }
        //    set { SetValue(CardTappedCommandProperty, value); }
        //}

        //public static readonly BindableProperty CardTappedCommandProperty =
        //    BindableProperty.Create("CardTappedCommand", typeof(TapGestureRecognizer), typeof(SimpleCard), null);

        //public DelegateCommand CardTappedCommand
        //{
        //    get { return (DelegateCommand)GetValue(CardTappedCommandProperty); }
        //    set { SetValue(CardTappedCommandProperty, value); }
        //}

        //public static readonly BindableProperty CardTappedCommandProperty =
        //    BindableProperty.Create("CardTappedCommand", typeof(DelegateCommand), typeof(SimpleCard), null);
    }
}