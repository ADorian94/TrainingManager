using TrainingManager.Data;

namespace TrainingManager.View.Controls
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToggleButtonCard : ContentView
    {
        public ToggleButtonCard()
        {
            InitializeComponent();
        }

        public string CardMainText
        {
            get { return (string)GetValue(CardMainTextProperty); }
            set { SetValue(CardMainTextProperty, value); }
        }

        public static readonly BindableProperty CardMainTextProperty =
            BindableProperty.Create("CardMainText", typeof(string), typeof(ToggleButtonCard), string.Empty);

        public bool ToggleValue
        {
            get { return (bool)GetValue(ToggleValueProperty); }
            set { SetValue(ToggleValueProperty, value); }
        }

        public static readonly BindableProperty ToggleValueProperty =
            BindableProperty.Create("ToggleValue", typeof(bool), typeof(ToggleButtonCard), false, propertyChanged: OnSwitched);

        private static void OnSwitched(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ToggleButtonCard headerTemplate && newValue is bool toggleValue)
                headerTemplate.ToggleValue = toggleValue;
        }

        public MaterialColors CardColor
        {
            get { return (MaterialColors)GetValue(CardColorProperty); }
            set { SetValue(CardColorProperty, value); }
        }

        public static readonly BindableProperty CardColorProperty =
            BindableProperty.Create("CardColor", typeof(MaterialColors), typeof(LowSimpleCard), MaterialColors.Default);
    }
}