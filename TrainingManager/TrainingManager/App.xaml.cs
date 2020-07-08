using TrainingManager.ViewModel.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TrainingManager
{
    public partial class App : Application
    {
        //private OneRepetitionMaximumVM _calculator_VM;
        //private OneRepetitionMaximumCalculatedPage _oneRepetitionMaximumCalculatedPage;
        //private ExerciseTimerVM _exerciseTimerVM;
        //private ExerciseTimer _exerciseTimer;
        //private IntervallTimerManagerVM _intervallTimerVM;
        //private AddIntervallPage _addIntervallPage;
        //private IntervallWorkoutsPage _intervallWorkoutsPage;
        //private AddIntervallWorkoutPage _intervallWorkoutPage;

        //private ActiveIntervallTimerPage _activeIntervallTimerPage;
        //private IntervallTimerPage _intervallTimerTabbedPage;

        //private MasterDetailNavigationPageMasterViewModel _masterDetailNavigationPageMasterVM;
        //private MasterDetailNavigationPage _masterDetailNavigationPage;

        //private NavigationPage _navigationPage;
        //private MainPage _mainPage;

        private PageNavigationManager _pageNavigationManager;

        public App()
        {
            InitializeComponent();
            _pageNavigationManager = new PageNavigationManager();
            //_calculator_VM = new OneRepetitionMaximumVM();
            //_calculator_VM.CalculationStartEvent += OnCalculationStarted;

            //_exerciseTimerVM = new ExerciseTimerVM();
            //_exerciseTimer = new ExerciseTimer();
            //_exerciseTimer.BindingContext = _exerciseTimerVM;

            //_intervallTimerVM = new IntervallTimerManagerVM();
            //_intervallTimerVM.OpenNewIntervallPage += OnNewIntervallPage;
            //_intervallTimerVM.OpenNewIntervallWorkoutPage += OnOpenNewIntervallWorkoutPage;
            //_intervallTimerVM.CloseAddNewIntervallPage += OnCloseIntervallPage;
            //_intervallTimerVM.CloseNewIntervallWorkoutPage += OnCloseNewIntervallWorkoutPage;
            //_intervallTimerVM.WorkoutSelected += OnWorkoutSelected;

            //_intervallTimerTabbedPage = new IntervallTimerPage();
            //_activeIntervallTimerPage = new ActiveIntervallTimerPage();
            //_intervallTimerTabbedPage.BarBackgroundColor = Color.FromHex("#333333");
            //_activeIntervallTimerPage.BindingContext = _intervallTimerVM;
            //_intervallWorkoutsPage = new IntervallWorkoutsPage();
            //_intervallWorkoutsPage.BindingContext = _intervallTimerVM;
            //_activeIntervallTimerPage.Title = "Intervall";
            //_intervallWorkoutsPage.Title = "Workouts";
            //_intervallTimerTabbedPage.Children.Add(_activeIntervallTimerPage);
            //_intervallTimerTabbedPage.Children.Add(_intervallWorkoutsPage);

            //_mainPage = new MainPage();
            //_mainPage.BindingContext = _calculator_VM;

            //_navigationPage = new NavigationPage(_mainPage);
            //_navigationPage.BarBackgroundColor = Color.FromHex("#333333");
            //_navigationPage.BarTextColor = Color.White;
            //_masterDetailNavigationPageMasterVM = new MasterDetailNavigationPageMasterViewModel();
            //_masterDetailNavigationPage = new MasterDetailNavigationPage();
            //_masterDetailNavigationPage.BindingContext = _masterDetailNavigationPageMasterVM;
            //_masterDetailNavigationPage.DetailPageSelected += OnDetailPageSelected;
            //_masterDetailNavigationPage.Detail = _navigationPage;

            MainPage = _pageNavigationManager.GetMainPage();
        }

        //private void OnDetailPageSelected(object sender, EventArgs e)
        //{
        //    var item = ((SelectedItemChangedEventArgs)e).SelectedItem as MasterDetailNavigationPageMenuItem;
        //    if (item == null)
        //        return;

        //    if (item.TargetType == typeof(MainPage))
        //        _navigationPage = new NavigationPage(_mainPage);

        //    if (item.TargetType == typeof(ExerciseTimer))
        //        _navigationPage = new NavigationPage(_exerciseTimer);

        //    if (item.TargetType == typeof(IntervallTimerPage))
        //        _navigationPage = new NavigationPage(_intervallTimerTabbedPage);

        //    _masterDetailNavigationPage.Detail = _navigationPage;
        //    //if (item.TargetType == typeof(IntervallTimerPage))
        //    //    _masterDetailNavigationPage.Detail = _intervallTimerTabbedPage;

        //    _masterDetailNavigationPage.IsPresented = false;
        //}

        //private void OnWorkoutSelected(object sender, EventArgs e)
        //{
        //    _intervallTimerTabbedPage.CurrentPage = _activeIntervallTimerPage;
        //}

        //private void OnOpenNewIntervallWorkoutPage(object sender, EventArgs e)
        //{
        //    _intervallWorkoutPage = new AddIntervallWorkoutPage();
        //    _intervallWorkoutPage.BindingContext = _intervallTimerVM;
        //    _navigationPage.PushAsync(_intervallWorkoutPage);
        //    _masterDetailNavigationPage.Detail = _navigationPage;
        //}

        //private void OnCloseNewIntervallWorkoutPage(object sender, EventArgs e)
        //{
        //    _navigationPage.PopAsync();
        //}

        //private void OnCloseIntervallPage(object sender, EventArgs e)
        //{
        //    _navigationPage.PopAsync();
        //}

        //private void OnNewIntervallPage(object sender, EventArgs e)
        //{
        //    _addIntervallPage = new AddIntervallPage();
        //    _addIntervallPage.BindingContext = _intervallTimerVM;
        //    _navigationPage.PushAsync(_addIntervallPage);
        //    _masterDetailNavigationPage.Detail = _navigationPage;
        //}

        //private void OnCalculationStarted(object sender, EventArgs e)
        //{
        //    _oneRepetitionMaximumCalculatedPage = new OneRepetitionMaximumCalculatedPage();
        //    _oneRepetitionMaximumCalculatedPage.BindingContext = _calculator_VM;
        //    _navigationPage.PushAsync(_oneRepetitionMaximumCalculatedPage);
        //    _masterDetailNavigationPage.Detail = _navigationPage;
        //}

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
