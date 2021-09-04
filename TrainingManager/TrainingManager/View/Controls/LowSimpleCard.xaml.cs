
using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LowSimpleCard : ContentView
    {
        public LowSimpleCard()
        {
            InitializeComponent();
        }

        public string CardMainText
        {
            get { return (string)GetValue(CardMainTextProperty); }
            set { SetValue(CardMainTextProperty, value); }
        }

        public static readonly BindableProperty CardMainTextProperty =
            BindableProperty.Create("CardMainText", typeof(string), typeof(LowSimpleCard), string.Empty);

        public bool IsButtonVisible
        {
            get { return (bool)GetValue(IsButtonVisibleProperty); }
            set { SetValue(IsButtonVisibleProperty, value); }
        }

        public static readonly BindableProperty IsButtonVisibleProperty =
            BindableProperty.Create("IsButtonVisibles", typeof(bool), typeof(LowSimpleCard), false);

        public DelegateCommand ButtonCommand
        {
            get { return (DelegateCommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public static readonly BindableProperty ButtonCommandProperty =
            BindableProperty.Create("ButtonCommand", typeof(DelegateCommand), typeof(LowSimpleCard), defaultValue: default(DelegateCommand), propertyChanged: OnTapCommandPropertyChanged);

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
            BindableProperty.Create("ButtonCommandParameter", typeof(string), typeof(LowSimpleCard), string.Empty);

        public DelegateCommand CardFrameTapped
        {
            get { return (DelegateCommand)GetValue(CardFrameTappedProperty); }
            set { SetValue(CardFrameTappedProperty, value); }
        }

        public static readonly BindableProperty CardFrameTappedProperty =
            BindableProperty.Create("CardFrameTapped", typeof(DelegateCommand), typeof(LowSimpleCard), defaultValue: default(DelegateCommand), propertyChanged: OnFrameTapCommandPropertyChanged);

        private static void OnFrameTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LowSimpleCard headerTemplate && newValue is DelegateCommand command)
            {
                headerTemplate.CardFrameTapped = command;
            }
        }

        public string CardFrameTappedParameter
        {
            get { return (string)GetValue(CardFrameTappedParameterProperty); }
            set { SetValue(CardFrameTappedParameterProperty, value); }
        }

        public static readonly BindableProperty CardFrameTappedParameterProperty =
            BindableProperty.Create("CardFrameTappedParameter", typeof(string), typeof(LowSimpleCard), string.Empty);
    }
}