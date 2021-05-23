using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.TabbedPageView.AddWeightWorkoutPage.AddNewDrillPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpViewView : ContentView
    {
        public PopUpViewView()
        {
            InitializeComponent();
        }

        public string PopUpText
        {
            get { return (string)GetValue(PopUpTextProperty); }
            set { SetValue(PopUpTextProperty, value); }
        }

        public static readonly BindableProperty PopUpTextProperty =
            BindableProperty.Create("PopUpText", typeof(string), typeof(PopUpViewView), string.Empty);

        public string ExerciseButtonText
        {
            get { return (string)GetValue(ExerciseButtonTextProperty); }
            set { SetValue(ExerciseButtonTextProperty, value); }
        }

        public static readonly BindableProperty ExerciseButtonTextProperty =
            BindableProperty.Create("ExerciseButtonText", typeof(string), typeof(PopUpViewView), string.Empty);

        public string RoundButtonText
        {
            get { return (string)GetValue(RoundButtonTextProperty); }
            set { SetValue(RoundButtonTextProperty, value); }
        }

        public static readonly BindableProperty RoundButtonTextProperty =
            BindableProperty.Create("RoundButtonText", typeof(string), typeof(PopUpViewView), string.Empty);

        public DelegateCommand ExerciseButtonCommand
        {
            get { return (DelegateCommand)GetValue(ExerciseButtonCommandProperty); }
            set { SetValue(ExerciseButtonCommandProperty, value); }
        }

        public static readonly BindableProperty ExerciseButtonCommandProperty =
            BindableProperty.Create("ExerciseButtonCommand", typeof(DelegateCommand), typeof(PopUpViewView), defaultValue: default(DelegateCommand), propertyChanged: OnTapCommandPropertyChanged);

        private static void OnTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PopUpViewView headerTemplate && newValue is DelegateCommand command)
            {
                headerTemplate.ExerciseButtonCommand = command;
            }
        }

        public string RoundButtonCommandParameter
        {
            get { return (string)GetValue(RoundButtonCommandParameterProperty); }
            set { SetValue(RoundButtonCommandParameterProperty, value); }
        }

        public static readonly BindableProperty RoundButtonCommandParameterProperty =
            BindableProperty.Create("RoundButtonCommandParameter", typeof(string), typeof(PopUpViewView), string.Empty);

        public DelegateCommand RoundButtonCommand
        {
            get { return (DelegateCommand)GetValue(RoundButtonCommandProperty); }
            set { SetValue(RoundButtonCommandProperty, value); }
        }

        public static readonly BindableProperty RoundButtonCommandProperty =
            BindableProperty.Create("RoundButtonCommand", typeof(DelegateCommand), typeof(PopUpViewView), defaultValue: default(DelegateCommand), propertyChanged: OnTapRoundCommandPropertyChanged);

        private static void OnTapRoundCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PopUpViewView headerTemplate && newValue is DelegateCommand command)
            {
                headerTemplate.RoundButtonCommand = command;
            }
        }

        public string ExerciseButtonCommandParameter
        {
            get { return (string)GetValue(ExerciseButtonCommandParameterProperty); }
            set { SetValue(ExerciseButtonCommandParameterProperty, value); }
        }

        public static readonly BindableProperty ExerciseButtonCommandParameterProperty =
            BindableProperty.Create("ExerciseButtonCommandParameter", typeof(string), typeof(PopUpViewView), string.Empty);

        public bool IsPopUpVisible
        {
            get { return (bool)GetValue(IsPopUpVisibleProperty); }
            set { SetValue(IsPopUpVisibleProperty, value); }
        }

        public static readonly BindableProperty IsPopUpVisibleProperty =
            BindableProperty.Create("IsPopUpVisible", typeof(bool), typeof(PopUpViewView), false);

        private void ImageButton_Clicked(object sender, System.EventArgs e) => IsPopUpVisible = false;
    }
}