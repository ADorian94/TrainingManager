using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardWithTextEditor : ContentView
    {
        public CardWithTextEditor()
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

        public string EditorText
        {
            get { return (string)GetValue(EditorTextProperty); }
            set { SetValue(EditorTextProperty, value); }
        }

        public static readonly BindableProperty EditorTextProperty =
            BindableProperty.Create("EditorText", typeof(string), typeof(SimpleCard), string.Empty);

        public DelegateCommand CardFrameTapped
        {
            get { return (DelegateCommand)GetValue(CardFrameTappedProperty); }
            set { SetValue(CardFrameTappedProperty, value); }
        }

        public static readonly BindableProperty CardFrameTappedProperty =
            BindableProperty.Create("CardFrameTapped", typeof(DelegateCommand), typeof(SimpleCard), defaultValue: default(DelegateCommand), propertyChanged: OnTapCommandPropertyChanged);

        private static void OnTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CardWithTextEditor headerTemplate && newValue is DelegateCommand command)
            {
                headerTemplate.CardFrameTapped = command;
            }
        }

        //public string ButtonCommandParameter
        //{
        //    get { return (string)GetValue(ButtonCommandParameterProperty); }
        //    set { SetValue(ButtonCommandParameterProperty, value); }
        //}

        //public static readonly BindableProperty ButtonCommandParameterProperty =
        //    BindableProperty.Create("ButtonCommandParameter", typeof(string), typeof(SimpleCard), string.Empty);

        //public bool ButtonVisibility
        //{
        //    get { return (bool)GetValue(ButtonVisibilityProperty); }
        //    set { SetValue(ButtonVisibilityProperty, value); }
        //}

        //public static readonly BindableProperty ButtonVisibilityProperty =
        //    BindableProperty.Create("ButtonVisibility", typeof(bool), typeof(SimpleCard), false);
    }
}