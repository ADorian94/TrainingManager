using TrainingManager.ViewModel;

namespace TrainingManager.View.Controls
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryCard : ContentView
    {
        public HistoryCard()
        {
            InitializeComponent();
            ////History.IsVisible = true;
            ////BluePart.Scale = 0;
            ////BluePart.Opacity = 0;
            //Animate();
        }

        //private async void Animate()
        //{
        //    BluePart.FadeTo(1, 400);
        //    await BluePart.ScaleTo(1.2, 800);
        //    await BluePart.ScaleTo(0.9, 400);
        //    await BluePart.ScaleTo(1.1, 200);
        //    await BluePart.ScaleTo(1, 100);
        //}

        public string CloseCardMainLabel
        {
            get { return (string)GetValue(CloseCardMainLabelProperty); }
            set { SetValue(CloseCardMainLabelProperty, value); }
        }

        public static readonly BindableProperty CloseCardMainLabelProperty =
            BindableProperty.Create("CloseCardMainLabel", typeof(string), typeof(HistoryCard), string.Empty);

        public string OpenCardFirstLabel
        {
            get { return (string)GetValue(OpenCardFirstLabelProperty); }
            set { SetValue(OpenCardFirstLabelProperty, value); }
        }

        public static readonly BindableProperty OpenCardFirstLabelProperty =
            BindableProperty.Create("OpenCardFirstLabel", typeof(string), typeof(HistoryCard), string.Empty);

        public string OpenCardFirstValue
        {
            get { return (string)GetValue(OpenCardFirstValueProperty); }
            set { SetValue(OpenCardFirstValueProperty, value); }
        }

        public static readonly BindableProperty OpenCardFirstValueProperty =
            BindableProperty.Create("OpenCardFirstValue", typeof(string), typeof(HistoryCard), string.Empty);

        public static readonly BindableProperty OpenCardSecondValueProperty =
            BindableProperty.Create("OpenCardSecondValue", typeof(string), typeof(HistoryCard), string.Empty);

        public DelegateCommand ButtonCommand
        {
            get { return (DelegateCommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public static readonly BindableProperty ButtonCommandProperty =
            BindableProperty.Create("ButtonCommand", typeof(DelegateCommand), typeof(HistoryCard), defaultValue: default(DelegateCommand), propertyChanged: OnTapCommandPropertyChanged);

        private static void OnTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is HistoryCard headerTemplate && newValue is DelegateCommand command)
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
            BindableProperty.Create("ButtonCommandParameter", typeof(string), typeof(HistoryCard), string.Empty);

        public bool ButtonVisibility
        {
            get { return (bool)GetValue(ButtonVisibilityProperty); }
            set { SetValue(ButtonVisibilityProperty, value); }
        }

        public static readonly BindableProperty ButtonVisibilityProperty =
            BindableProperty.Create("ButtonVisibility", typeof(bool), typeof(HistoryCard), defaultValue: false);

        public string ShortDate
        {
            get { return (string)GetValue(ShortDateProperty); }
            set { SetValue(ShortDateProperty, value); }
        }

        public static readonly BindableProperty ShortDateProperty =
            BindableProperty.Create("ShortDate", typeof(string), typeof(HistoryCard), string.Empty);

        public string LongDate
        {
            get { return (string)GetValue(LongDateProperty); }
            set { SetValue(LongDateProperty, value); }
        }

        public static readonly BindableProperty LongDateProperty =
            BindableProperty.Create("LongDate", typeof(string), typeof(HistoryCard), string.Empty);

    }
}