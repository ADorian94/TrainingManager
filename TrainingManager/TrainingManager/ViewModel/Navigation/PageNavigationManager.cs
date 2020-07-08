using System;
using TrainingManager.Model.Navigation.MasterDetailPageItem;
using TrainingManager.View;
using TrainingManager.View.MasterDetailNavigationPage;
using TrainingManager.View.Timer.IntervallTimer;
using Xamarin.Forms;

namespace TrainingManager.ViewModel.Navigation
{
    public class PageNavigationManager
    {
        //PAGES
        private MasterDetailNavigationPage _masterDetailNavigationPage;
        private MainPage _mainPage;
        private OneRepetitionMaximumCalculatedPage _oneRepetitionMaximumCalculatedPage;
        private ExerciseTimer _exerciseTimer;
        private AddIntervallPage _addIntervallPage;
        private IntervallWorkoutsPage _intervallWorkoutsPage;
        private AddIntervallWorkoutPage _intervallWorkoutPage;
        private ActiveIntervallTimerPage _activeIntervallTimerPage;
        private IntervallTimerPage _intervallTimerTabbedPage;
        private NavigationPage _navigationPage;

        //VIEWMODELLS
        private MasterDetailNavigationPageMasterViewModel _masterDetailNavigationPageMasterVM;
        private OneRepetitionMaximumVM _oneRepetitionMaximumVM;
        private ExerciseTimerVM _exerciseTimerVM;
        private IntervallTimerManagerVM _intervallTimerVM;

        public PageNavigationManager()
        {
            //INITIALIZE VMS
            _masterDetailNavigationPageMasterVM = new MasterDetailNavigationPageMasterViewModel();
            _oneRepetitionMaximumVM = new OneRepetitionMaximumVM();
            _exerciseTimerVM = new ExerciseTimerVM();
            _intervallTimerVM = new IntervallTimerManagerVM();

            //INITIALIZE PAGES
            _mainPage = new MainPage();
            _oneRepetitionMaximumCalculatedPage = new OneRepetitionMaximumCalculatedPage();
            _exerciseTimer = new ExerciseTimer();
            _activeIntervallTimerPage = new ActiveIntervallTimerPage();
            _activeIntervallTimerPage.Title = "Intervall";
            _intervallWorkoutsPage = new IntervallWorkoutsPage();
            _intervallWorkoutsPage.Title = "Workouts";
            _intervallTimerTabbedPage = new IntervallTimerPage();
            _intervallTimerTabbedPage.Title = "Intervall timer";
            _intervallTimerTabbedPage.Children.Add(_activeIntervallTimerPage);
            _intervallTimerTabbedPage.Children.Add(_intervallWorkoutsPage);

            _navigationPage = new NavigationPage(_mainPage);
            _masterDetailNavigationPage = new MasterDetailNavigationPage();
            _masterDetailNavigationPage.Detail = _navigationPage;

            //BINDINGCONTEXT
            _masterDetailNavigationPage.BindingContext = _masterDetailNavigationPageMasterVM;
            _mainPage.BindingContext = _oneRepetitionMaximumVM;
            _oneRepetitionMaximumCalculatedPage.BindingContext = _oneRepetitionMaximumVM;
            _exerciseTimer.BindingContext = _exerciseTimerVM;
            _activeIntervallTimerPage.BindingContext = _intervallTimerVM;
            _intervallWorkoutsPage.BindingContext = _intervallTimerVM;

            //EVENT SUBSCRIBE
            _masterDetailNavigationPage.DetailPageSelected += OnDetailPageSelected;
            _oneRepetitionMaximumVM.CalculationStartEvent += OnCalculationStarted;
            _intervallTimerVM.OpenNewIntervallPage += OnNewIntervallPage;
            _intervallTimerVM.OpenNewIntervallWorkoutPage += OnOpenNewIntervallWorkoutPage;
            _intervallTimerVM.CloseAddNewIntervallPage += OnCliseNavigationPage;
            _intervallTimerVM.CloseNewIntervallWorkoutPage += OnCliseNavigationPage;
            _intervallTimerVM.WorkoutSelected += OnWorkoutSelected;
        }

        public Page GetMainPage() => _masterDetailNavigationPage;

        private void OnDetailPageSelected(object sender, EventArgs e)
        {
            var item = ((SelectedItemChangedEventArgs)e).SelectedItem as MasterDetailNavigationPageMenuItem;
            if (item == null)
                return;

            if (item.TargetType == typeof(MainPage))
                _navigationPage = new NavigationPage(_mainPage);

            if (item.TargetType == typeof(ExerciseTimer))
                _navigationPage = new NavigationPage(_exerciseTimer);

            if (item.TargetType == typeof(IntervallTimerPage))
                _navigationPage = new NavigationPage(_intervallTimerTabbedPage);

            _masterDetailNavigationPage.Detail = _navigationPage;
            //if (item.TargetType == typeof(IntervallTimerPage))
            //    _masterDetailNavigationPage.Detail = _intervallTimerTabbedPage;

            _masterDetailNavigationPage.IsPresented = false;
        }

        private void OnCalculationStarted(object sender, EventArgs e)
        {
            _navigationPage.PushAsync(_oneRepetitionMaximumCalculatedPage);
            _masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnWorkoutSelected(object sender, EventArgs e)
        {
            _intervallTimerTabbedPage.CurrentPage = _activeIntervallTimerPage;
        }

        private void OnOpenNewIntervallWorkoutPage(object sender, EventArgs e)
        {
            _intervallWorkoutPage = new AddIntervallWorkoutPage();
            _intervallWorkoutPage.BindingContext = _intervallTimerVM;
            _navigationPage.PushAsync(_intervallWorkoutPage);
            _masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnCliseNavigationPage(object sender, EventArgs e)
        {
            _navigationPage.PopAsync();
            _masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnNewIntervallPage(object sender, EventArgs e)
        {
            _addIntervallPage = new AddIntervallPage();
            _addIntervallPage.BindingContext = _intervallTimerVM;
            _navigationPage.PushAsync(_addIntervallPage);
            _masterDetailNavigationPage.Detail = _navigationPage;
        }
    }
}
