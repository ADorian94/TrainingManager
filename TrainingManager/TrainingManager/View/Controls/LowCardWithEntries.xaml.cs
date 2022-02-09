using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LowCardWithEntries : ContentView
    {
        public LowCardWithEntries()
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
            BindableProperty.Create("CardIndex", typeof(string), typeof(LowCardWithEntries), string.Empty);

        public string CardMainText
        {
            get { return (string)GetValue(CardMainTextProperty); }
            set { SetValue(CardMainTextProperty, value); }
        }

        public static readonly BindableProperty CardMainTextProperty =
            BindableProperty.Create("CardMainText", typeof(string), typeof(LowCardWithEntries), string.Empty);

        public double CardMainEntry
        {
            get { return (double)GetValue(CardMainEntryProperty); }
            set { SetValue(CardMainEntryProperty, value); }
        }

        public static readonly BindableProperty CardMainEntryProperty =
            BindableProperty.Create("CardMainEntry", typeof(double), typeof(LowCardWithEntries), 0.0);

        public string CardSecondaryText
        {
            get { return (string)GetValue(CardSecondaryTextProperty); }
            set { SetValue(CardSecondaryTextProperty, value); }
        }

        public static readonly BindableProperty CardSecondaryTextProperty =
            BindableProperty.Create("CardSecondaryText", typeof(string), typeof(LowCardWithEntries), string.Empty);

        public double CardSecondaryEntry
        {
            get { return (double)GetValue(CardSecondaryEntryProperty); }
            set { SetValue(CardSecondaryEntryProperty, value); }
        }

        public static readonly BindableProperty CardSecondaryEntryProperty =
            BindableProperty.Create("CardSecondaryEntry", typeof(double), typeof(LowCardWithEntries), 0.0);

        public DelegateCommand ButtonCommand
        {
            get { return (DelegateCommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public static readonly BindableProperty ButtonCommandProperty =
            BindableProperty.Create("ButtonCommand", typeof(DelegateCommand), typeof(LowCardWithEntries), defaultValue: default(DelegateCommand), propertyChanged: OnTapCommandPropertyChanged);

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
            BindableProperty.Create("ButtonCommandParameter", typeof(string), typeof(LowCardWithEntries), string.Empty);
    }
}