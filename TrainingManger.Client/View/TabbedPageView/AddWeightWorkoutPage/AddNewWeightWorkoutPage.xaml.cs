namespace TrainingManager.View
{
    ////[XamlCompilation(XamlCompilationOptions.Compile)]
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class AddNewWeightWorkoutPage : ContentPage
    {
        public AddNewWeightWorkoutPage()
        {
            InitializeComponent();
        }

        public AddNewWeightWorkoutPage(string viewMainTitle)
        {
            ViewMainTitle = viewMainTitle;
            InitializeComponent();
        }

        public string ViewMainTitle
        {
            get { return (string)GetValue(ViewMainTitleProperty); }
            set { SetValue(ViewMainTitleProperty, value); }
        }

        public static readonly BindableProperty ViewMainTitleProperty =
            BindableProperty.Create("ViewMainTitle", typeof(string), typeof(AddNewWeightWorkoutPage), string.Empty);
    }
}