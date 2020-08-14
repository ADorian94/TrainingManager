
using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpenableCard : ContentView
    {
        public OpenableCard()
        {
            InitializeComponent();
            OpenedCard.IsVisible = false;
            ClosedCard.IsVisible = true;
        }

        public string CloseCardMainLabel
        {
            get { return (string)GetValue(CloseCardMainLabelProperty); }
            set { SetValue(CloseCardMainLabelProperty, value); }
        }

        public static readonly BindableProperty CloseCardMainLabelProperty =
            BindableProperty.Create("CloseCardMainLabel", typeof(string), typeof(OpenableCard), string.Empty);

        public string CloseCardMainValue
        {
            get { return (string)GetValue(CloseCardMainValueProperty); }
            set { SetValue(CloseCardMainValueProperty, value); }
        }

        public static readonly BindableProperty CloseCardMainValueProperty =
            BindableProperty.Create("CloseCardMainValue", typeof(string), typeof(OpenableCard), string.Empty);

        public string OpenCardMainLabel
        {
            get { return (string)GetValue(OpenCardMainLabelProperty); }
            set { SetValue(OpenCardMainLabelProperty, value); }
        }

        public static readonly BindableProperty OpenCardMainLabelProperty =
            BindableProperty.Create("OpenCardMainLabel", typeof(string), typeof(OpenableCard), string.Empty);

        public string OpenCardFirstLabel
        {
            get { return (string)GetValue(OpenCardFirstLabelProperty); }
            set { SetValue(OpenCardFirstLabelProperty, value); }
        }

        public static readonly BindableProperty OpenCardFirstLabelProperty =
            BindableProperty.Create("OpenCardFirstLabel", typeof(string), typeof(OpenableCard), string.Empty);

        public string OpenCardFirstValue
        {
            get { return (string)GetValue(OpenCardFirstValueProperty); }
            set { SetValue(OpenCardFirstValueProperty, value); }
        }

        public static readonly BindableProperty OpenCardFirstValueProperty =
            BindableProperty.Create("OpenCardFirstValue", typeof(string), typeof(OpenableCard), string.Empty);

        public string OpenCardSecondLabel
        {
            get { return (string)GetValue(OpenCardSecondLabelProperty); }
            set { SetValue(OpenCardSecondLabelProperty, value); }
        }

        public static readonly BindableProperty OpenCardSecondLabelProperty =
            BindableProperty.Create("OpenCardSecondLabel", typeof(string), typeof(OpenableCard), string.Empty);

        public string OpenCardSecondValue
        {
            get { return (string)GetValue(OpenCardSecondValueProperty); }
            set { SetValue(OpenCardSecondValueProperty, value); }
        }

        public static readonly BindableProperty OpenCardSecondValueProperty =
            BindableProperty.Create("OpenCardSecondValue", typeof(string), typeof(OpenableCard), string.Empty);

        public DelegateCommand ButtonCommand
        {
            get { return (DelegateCommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public static readonly BindableProperty ButtonCommandProperty =
            BindableProperty.Create("ButtonCommand", typeof(DelegateCommand), typeof(OpenableCard), defaultValue: default(DelegateCommand), propertyChanged: OnTapCommandPropertyChanged);

        private static void OnTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is OpenableCard headerTemplate && newValue is DelegateCommand command)
            {
                headerTemplate.ButtonCommand = command;
            }
        }

        public string ButtonCommandParameter
        {
            get { return (string)GetValue(ButtonCommandParameterProperty); }
            set { SetValue(ButtonCommandParameterProperty, value); }
        }

        public static readonly BindableProperty ButtonCommandParameterProperty =
            BindableProperty.Create("ButtonCommandParameter", typeof(string), typeof(OpenableCard), string.Empty);

        public bool ButtonVisibility
        {
            get { return (bool)GetValue(ButtonVisibilityProperty); }
            set { SetValue(ButtonVisibilityProperty, value); }
        }

        public static readonly BindableProperty ButtonVisibilityProperty =
            BindableProperty.Create("ButtonVisibility", typeof(bool), typeof(OpenableCard), defaultValue: false);

        //public DelegateCommand CardFrameTapped
        //{
        //    get { return (DelegateCommand)GetValue(CardFrameTappedProperty); }
        //    set { SetValue(CardFrameTappedProperty, value); }
        //}

        //public static readonly BindableProperty CardFrameTappedProperty =
        //    BindableProperty.Create("CardFrameTapped", typeof(DelegateCommand), typeof(OpenableCard), defaultValue: default(DelegateCommand), propertyChanged: OnFrameTapCommandPropertyChanged);

        //private static void OnFrameTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    if (bindable is OpenableCard headerTemplate && newValue is DelegateCommand command)
        //    {
        //        headerTemplate.CardFrameTapped = command;
        //    }
        //}

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            OpenedCard.IsVisible = false;
            ClosedCard.IsVisible = true;
        }

        private void TapGestureRecognizer_Tapped_1(object sender, System.EventArgs e)
        {
            ClosedCard.IsVisible = false;
            OpenedCard.IsVisible = true;
        }
    }
}