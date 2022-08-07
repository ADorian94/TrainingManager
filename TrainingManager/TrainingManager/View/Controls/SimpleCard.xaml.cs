using TrainingManager.ViewModel;
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

        public DelegateCommand ButtonCommand
        {
            get { return (DelegateCommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public static readonly BindableProperty ButtonCommandProperty =
            BindableProperty.Create("ButtonCommand", typeof(DelegateCommand), typeof(SimpleCard), defaultValue: default(DelegateCommand), propertyChanged: OnTapCommandPropertyChanged);

        private static void OnTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LowSimpleCard headerTemplate && newValue is DelegateCommand command)
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
            BindableProperty.Create("ButtonCommandParameter", typeof(string), typeof(SimpleCard), string.Empty);

        public bool ButtonVisibility
        {
            get { return (bool)GetValue(ButtonVisibilityProperty); }
            set { SetValue(ButtonVisibilityProperty, value); }
        }

        public static readonly BindableProperty ButtonVisibilityProperty =
            BindableProperty.Create("ButtonVisibility", typeof(bool), typeof(SimpleCard), false);

        public DelegateCommand CardFrameTapped
        {
            get { return (DelegateCommand)GetValue(CardFrameTappedProperty); }
            set { SetValue(CardFrameTappedProperty, value); }
        }

        public static readonly BindableProperty CardFrameTappedProperty =
            BindableProperty.Create("CardFrameTapped", typeof(DelegateCommand), typeof(SimpleCard), defaultValue: default(DelegateCommand), propertyChanged: OnFrameTapCommandPropertyChanged);

        private static void OnFrameTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SimpleCard headerTemplate && newValue is DelegateCommand command)
                headerTemplate.CardFrameTapped = command;
        }
    }
}