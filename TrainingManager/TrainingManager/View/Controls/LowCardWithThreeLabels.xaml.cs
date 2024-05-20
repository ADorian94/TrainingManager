using TrainingManager.Data;
using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LowCardWithThreeLabels : ContentView
    {
        public LowCardWithThreeLabels()
        {
            InitializeComponent();
        }

        //BINDABEL PROPERTIES
        public string CardIndex
        {
            get { return (string)GetValue(CardIndexProperty); }
            set { SetValue(CardIndexProperty, value); }
        }

        public static readonly BindableProperty CardIndexProperty =
            BindableProperty.Create("CardIndex", typeof(string), typeof(LowCardWithThreeLabels), string.Empty);

        public bool IsIndexVisible
        {
            get { return (bool)GetValue(IsIndexVisibleProperty); }
            set { SetValue(IsIndexVisibleProperty, value); }
        }

        public static readonly BindableProperty IsIndexVisibleProperty =
            BindableProperty.Create("IsIndexVisible", typeof(bool), typeof(LowCardWithThreeLabels), false);

        public string CardFirstLabel
        {
            get { return (string)GetValue(CardFirstLabelProperty); }
            set { SetValue(CardFirstLabelProperty, value); }
        }

        public static readonly BindableProperty CardFirstLabelProperty =
            BindableProperty.Create("CardFirstLabel", typeof(string), typeof(LowCardWithThreeLabels), string.Empty);

        public string CardFirstContent
        {
            get { return (string)GetValue(CardFirstContentProperty); }
            set { SetValue(CardFirstContentProperty, value); }
        }

        public static readonly BindableProperty CardFirstContentProperty =
            BindableProperty.Create("CardFirstContent", typeof(string), typeof(LowCardWithThreeLabels), string.Empty);

        public string CardSecondLabel
        {
            get { return (string)GetValue(CardSecondLabelProperty); }
            set { SetValue(CardSecondLabelProperty, value); }
        }

        public static readonly BindableProperty CardSecondLabelProperty =
            BindableProperty.Create("CardSecondLabel", typeof(string), typeof(LowCardWithThreeLabels), string.Empty);

        public string CardSecondContent
        {
            get { return (string)GetValue(CardSecondContentProperty); }
            set { SetValue(CardSecondContentProperty, value); }
        }

        public static readonly BindableProperty CardSecondContentProperty =
            BindableProperty.Create("CardSecondContent", typeof(string), typeof(LowCardWithThreeLabels), string.Empty);

        public string CardThirdLabel
        {
            get { return (string)GetValue(CardThirdLabelProperty); }
            set { SetValue(CardThirdLabelProperty, value); }
        }

        public static readonly BindableProperty CardThirdLabelProperty =
            BindableProperty.Create("CardThirdLabel", typeof(string), typeof(LowCardWithThreeLabels), string.Empty);

        public string CardThirdContent
        {
            get { return (string)GetValue(CardThirdContentProperty); }
            set { SetValue(CardThirdContentProperty, value); }
        }

        public static readonly BindableProperty CardThirdContentProperty =
            BindableProperty.Create("CardThirdContent", typeof(string), typeof(LowCardWithThreeLabels), string.Empty);

        public MaterialColors CardColor
        {
            get { return (MaterialColors)GetValue(CardColorProperty); }
            set { SetValue(CardColorProperty, value); }
        }

        public static readonly BindableProperty CardColorProperty =
            BindableProperty.Create("CardColor", typeof(MaterialColors), typeof(LowCardWithThreeLabels), MaterialColors.Default);

        public DelegateCommand ButtonCommand
        {
            get { return (DelegateCommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public static readonly BindableProperty ButtonCommandProperty =
            BindableProperty.Create("ButtonCommand", typeof(DelegateCommand), typeof(LowCardWithThreeLabels), defaultValue: default(DelegateCommand), propertyChanged: OnTapCommandPropertyChanged);

        private static void OnTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LowCardWithThreeLabels headerTemplate && newValue is DelegateCommand command)
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
            BindableProperty.Create("ButtonCommandParameter", typeof(string), typeof(LowCardWithThreeLabels), string.Empty);

        public bool IsButtonVisible
        {
            get { return (bool)GetValue(IsButtonVisibleProperty); }
            set { SetValue(IsButtonVisibleProperty, value); }
        }

        public static readonly BindableProperty IsButtonVisibleProperty =
            BindableProperty.Create("IsButtonVisibles", typeof(bool), typeof(LowCardWithThreeLabels), false);
    }
}
