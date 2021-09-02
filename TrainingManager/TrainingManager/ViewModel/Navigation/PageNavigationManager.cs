using System;
using TrainingManager.Model;
using TrainingManager.Model.Services;
using TrainingManager.View;
using TrainingManager.View.Controls;
using Xamarin.Forms;

namespace TrainingManager.ViewModel.Navigation
{
    public class PageNavigationManager
    {
        //PAGES
        //private ExerciseTimer _exerciseTimer;
        //private AddIntervallPage _addIntervallPage;
        //private IntervallWorkoutsPage _intervallWorkoutsPage;
        //private AddIntervallWorkoutPage _intervallWorkoutPage;
        //private ActiveIntervallTimerPage _activeIntervallTimerPage;
        //private IntervallTimerPage _intervallTimerTabbedPage;
        //private HistoryView _weightHistoryView;

        //NAVIGATION PAGES
        private NavigationPage _homeNavigationPage;
        private NavigationPage _oneRepNavigationPage;
        private NavigationPage _addNewWeightWorkoutNavigationPage;
        private NavigationPage _settignsNavigationPage;
        private NavigationPage _exercicsesNavigationPage;
        private NavigationPage _addNewDrillNavigationPage;
        //TABBED PAGE
        private NavigationTabbedPage _navigationTabbedPage;
        private HomePage _homePage;
        private AddNewWeightWorkoutPage _addNewWeightWorkoutPage;
        private AddNewDrillCaruselPage _addNewDrillCaruselPage;
        private AddSavedWeightExercises _addSavedWeightExercises;
        private AddWeightExercisePage _addWeightDrillPage;
        private OneRepetitionMaximumCalculatorPage _oneRepetitionMaximumCalculatorPage;
        private OneRepetitionMaximumCalculatedPage _oneRepetitionMaximumCalculatedPage;
        private SettingsPage _settingsPage;
        private ExercisesPage _exercisesPage;
        private NotePage _notePage;

        //VIEWMODELLS
        private OneRepetitionMaximumVM _oneRepetitionMaximumVM;
        //private ExerciseTimerVM _exerciseTimerVM;
        private IntervallTimerManagerVM _intervallTimerVM;
        private WeightWorkoutManagerVM _weightWorkoutManagerVM;
        private WeightWorkoutHistoryManagerVM _weightWorkoutHistoryManagerVM;

        public PageNavigationManager(ApiServices apiServices)
        {
            //INITIALIZE VMS
            _oneRepetitionMaximumVM = new OneRepetitionMaximumVM();
            //_exerciseTimerVM = new ExerciseTimerVM();
            _intervallTimerVM = new IntervallTimerManagerVM();
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(apiServices);
            _weightWorkoutHistoryManagerVM = new WeightWorkoutHistoryManagerVM();

            //INITIALIZE PAGES
            _oneRepetitionMaximumCalculatorPage = new OneRepetitionMaximumCalculatorPage();
            _oneRepetitionMaximumCalculatedPage = new OneRepetitionMaximumCalculatedPage();
            //_exerciseTimer = new ExerciseTimer();
            //_activeIntervallTimerPage = new ActiveIntervallTimerPage();
            //_activeIntervallTimerPage.Title = "Intervall";
            //_intervallWorkoutsPage = new IntervallWorkoutsPage();
            //_intervallWorkoutsPage.Title = "Workouts";
            //_intervallTimerTabbedPage = new IntervallTimerPage();
            //_intervallTimerTabbedPage.Title = "Intervall timer";
            //_intervallTimerTabbedPage.Children.Add(_activeIntervallTimerPage);
            //_intervallTimerTabbedPage.Children.Add(_intervallWorkoutsPage);

            _homePage = new HomePage();
            _addNewWeightWorkoutPage = new AddNewWeightWorkoutPage();
            _settingsPage = new SettingsPage();
            _exercisesPage = new ExercisesPage();
            _addWeightDrillPage = new AddWeightExercisePage();
            _addSavedWeightExercises = new AddSavedWeightExercises();
            _addNewDrillCaruselPage = new AddNewDrillCaruselPage();
            _addNewDrillCaruselPage.Children.Add(_addWeightDrillPage);
            _addNewDrillCaruselPage.Children.Add(_addSavedWeightExercises);
            _notePage = new NotePage();

            //NAVIGATION PAGES
            _homeNavigationPage = new NavigationPage(_homePage);
            _homeNavigationPage.Title = "Home";
            _exercicsesNavigationPage = new NavigationPage(_exercisesPage);
            _exercicsesNavigationPage.Title = "Exercises";
            _addNewWeightWorkoutNavigationPage = new NavigationPage(_addNewWeightWorkoutPage);
            _addNewWeightWorkoutNavigationPage.Title = "Workout";
            _oneRepNavigationPage = new NavigationPage(_oneRepetitionMaximumCalculatorPage);
            _oneRepNavigationPage.Title = "1RM";
            _settignsNavigationPage = new NavigationPage(_settingsPage);
            _settignsNavigationPage.Title = "Settings";

            _addNewDrillNavigationPage = new NavigationPage(_addNewDrillCaruselPage);

            _navigationTabbedPage = new NavigationTabbedPage();
            _navigationTabbedPage.Children.Add(_homeNavigationPage);
            _navigationTabbedPage.Children.Add(_exercicsesNavigationPage);
            _navigationTabbedPage.Children.Add(_addNewWeightWorkoutNavigationPage);
            _navigationTabbedPage.Children.Add(_oneRepNavigationPage);
            _navigationTabbedPage.Children.Add(_settignsNavigationPage);

            //BINDINGCONTEXT
            _oneRepetitionMaximumCalculatorPage.BindingContext = _oneRepetitionMaximumVM;
            _oneRepetitionMaximumCalculatedPage.BindingContext = _oneRepetitionMaximumVM;
            //_exerciseTimer.BindingContext = _exerciseTimerVM;
            //_activeIntervallTimerPage.BindingContext = _intervallTimerVM;
            //_intervallWorkoutsPage.BindingContext = _intervallTimerVM;
            _addNewWeightWorkoutPage.BindingContext = _weightWorkoutManagerVM;
            _addWeightDrillPage.BindingContext = _weightWorkoutManagerVM;
            _notePage.BindingContext = _weightWorkoutManagerVM;

            //EVENT SUBSCRIBE
            _oneRepetitionMaximumVM.CalculationStartEvent += OnCalculationStarted;
            //_intervallTimerVM.OpenNewIntervallPage += OnNewIntervallPage;
            //_intervallTimerVM.OpenNewIntervallWorkoutPage += OnOpenNewIntervallWorkoutPage;
            _intervallTimerVM.CloseAddNewIntervallPage += OnCloseNavigationPage;
            _intervallTimerVM.CloseNewIntervallWorkoutPage += OnCloseNavigationPage;
            _intervallTimerVM.ExceptionOccured += OnExceptionOccured;
            _intervallTimerVM.MessageApplication += OnMessageApplication;
            _intervallTimerVM.WorkoutMenuSelected += OnWorkoutMenuSelected;
            //_intervallTimerVM.WorkoutSelected += OnWorkoutSelected;
            _intervallTimerVM.IntervallMenuSelected += OnIntervallMenuelected;
            _weightWorkoutManagerVM.OpenAddWeightExercise += OnOpenAddWeightExercise;
            _weightWorkoutManagerVM.CloseAddWeightExercise += OnCloseNavigationPage;
            _weightWorkoutManagerVM.OpenNoteEditor += OnOpenNoteEditor;
            _weightWorkoutManagerVM.OpenTrainingLog += OnOpenTrainingLog;
            _weightWorkoutManagerVM.OpenHistoryView += OnOpenHistoryView;
            _weightWorkoutManagerVM.WeightExerciseMenuSelected += OnWeightExerciseMenuSelected;
            _weightWorkoutManagerVM.OpenEditWeightExercise += OnOpenEditWeightExercise;
            _weightWorkoutManagerVM.MessageApplication += OnMessageApplication;
            _weightWorkoutManagerVM.CloseNoteEditor += OnCloseNavigationPage;
            _weightWorkoutHistoryManagerVM.OpenWorkoutDetails += OnOpenWorkoutDetails;
        }



        private async void OnWeightExerciseMenuSelected(object sender, MessageEventArgs e)
        {
            string action = await _navigationTabbedPage.DisplayActionSheet(e.Message, "Cancel", "Delete", "Edit");

            if (action == "Delete")
                _weightWorkoutManagerVM.DeleteExercise(e.Message);

            if (action == "Edit")
                _weightWorkoutManagerVM.EditExercise(e.Message);
        }

        private async void OnIntervallMenuelected(object sender, MessageEventArgs e)
        {
            string action = await _navigationTabbedPage.DisplayActionSheet(e.Message, "Cancel", "Delete", "Edit");

            if (action == "Delete")
                _intervallTimerVM.DeleteIntervall(e.Message);

            if (action == "Edit")
                _intervallTimerVM.EditIntervall(e.Message);
        }

        //private void OnWorkoutSelected(object sender, EventArgs e) => _intervallTimerTabbedPage.CurrentPage = _activeIntervallTimerPage;

        private async void OnWorkoutMenuSelected(object sender, MessageEventArgs e)
        {
            string action = await _navigationTabbedPage.DisplayActionSheet(e.Message, "Cancel", "Delete", "Edit");

            if (action == "Delete")
                _intervallTimerVM.DeleteWorkout(e.Message);

            if (action == "Edit")
                _intervallTimerVM.EditWorkout(e.Message);
        }

        private async void OnMessageApplication(object sender, MessageEventArgs e) => await _navigationTabbedPage.DisplayAlert(e.Message, e.Message, "Ok");

        /// <summary>
        /// Alapvető hibakezelés.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExceptionOccured(object sender, Model.ExceptionArgs e) => _navigationTabbedPage.DisplayAlert("Error during start workout.", e.Message, "Ok");

        public Page GetMainPage() => _navigationTabbedPage;

        private void OnCalculationStarted(object sender, EventArgs e) => _oneRepNavigationPage.PushAsync(_oneRepetitionMaximumCalculatedPage);

        //private void OnOpenNewIntervallWorkoutPage(object sender, EventArgs e)
        //{
        //    _intervallWorkoutPage = new AddIntervallWorkoutPage();
        //    _intervallWorkoutPage.BindingContext = _intervallTimerVM;
        //    //_navigationPage.PushAsync(_intervallWorkoutPage);
        //    //_masterDetailNavigationPage.Detail = _navigationPage;
        //}

        private void OnCloseNavigationPage(object sender, EventArgs e)
        {
            switch (((ClosePageEventArgs)e).Page)
            {
                case PageType.WightWorkout:
                    _addNewWeightWorkoutNavigationPage.PopAsync();
                    break;
            }
        }

        //private void OnNewIntervallPage(object sender, EventArgs e)
        //{
        //    _addIntervallPage = new AddIntervallPage();
        //    _addIntervallPage.BindingContext = _intervallTimerVM;
        //    //_navigationPage.PushAsync(_addIntervallPage);
        //    //_masterDetailNavigationPage.Detail = _navigationPage;
        //}

        private void OnOpenAddWeightExercise(object sender, EventArgs e) => _addNewWeightWorkoutNavigationPage.PushAsync(_addNewDrillCaruselPage);

        private void OnOpenEditWeightExercise(object sender, EventArgs e)
        {
            //_navigationPage.PushAsync(_addWeightExercise);
            //_masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnOpenNoteEditor(object sender, EventArgs e) => _addNewWeightWorkoutNavigationPage.PushAsync(_notePage);

        private void OnOpenTrainingLog(object sender, EventArgs e)
        {
            //_navigationPage.PushAsync(_todayWeightWorkout);
            //_masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnOpenHistoryView(object sender, EventArgs e)
        {
            //_navigationPage.PushAsync(_weightHistoryView);
            //_masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnOpenWorkoutDetails(object sender, EventArgs e)
        {
            //_navigationPage.PushAsync(_workoutHistoryDetails);
            //_masterDetailNavigationPage.Detail = _navigationPage;
        }
    }
}
