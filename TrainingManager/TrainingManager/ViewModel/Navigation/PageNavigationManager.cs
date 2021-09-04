using System;
using TrainingManager.Model;
using TrainingManager.Model.Services;
using TrainingManager.View;
using Xamarin.Forms;

namespace TrainingManager.ViewModel.Navigation
{
    public class PageNavigationManager
    {
        //NAVIGATION PAGES
        private NavigationPage _homeNavigationPage;
        private NavigationPage _oneRepNavigationPage;
        private NavigationPage _addNewWeightWorkoutNavigationPage;
        private NavigationPage _settignsNavigationPage;
        private NavigationPage _exercicsesNavigationPage;

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
        private WeightWorkoutManagerVM _weightWorkoutManagerVM;

        public PageNavigationManager(ApiServices apiServices)
        {
            //INITIALIZE VMS
            _oneRepetitionMaximumVM = new OneRepetitionMaximumVM();
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(apiServices);

            //INITIALIZE PAGES
            _oneRepetitionMaximumCalculatorPage = new OneRepetitionMaximumCalculatorPage();
            _oneRepetitionMaximumCalculatedPage = new OneRepetitionMaximumCalculatedPage();

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
            _homeNavigationPage = new NavigationPage(_homePage)
            {
                Title = "Home"
            };
            _exercicsesNavigationPage = new NavigationPage(_exercisesPage)
            {
                Title = "Exercises"
            };
            _addNewWeightWorkoutNavigationPage = new NavigationPage(_addNewWeightWorkoutPage)
            {
                Title = "Workout"
            };
            _oneRepNavigationPage = new NavigationPage(_oneRepetitionMaximumCalculatorPage)
            {
                Title = "1RM"
            };
            _settignsNavigationPage = new NavigationPage(_settingsPage)
            {
                Title = "Settings"
            };

            _navigationTabbedPage = new NavigationTabbedPage();
            _navigationTabbedPage.Children.Add(_homeNavigationPage);
            _navigationTabbedPage.Children.Add(_exercicsesNavigationPage);
            _navigationTabbedPage.Children.Add(_addNewWeightWorkoutNavigationPage);
            _navigationTabbedPage.Children.Add(_oneRepNavigationPage);
            _navigationTabbedPage.Children.Add(_settignsNavigationPage);

            //BINDINGCONTEXT
            _oneRepetitionMaximumCalculatorPage.BindingContext = _oneRepetitionMaximumVM;
            _oneRepetitionMaximumCalculatedPage.BindingContext = _oneRepetitionMaximumVM;
            _addNewWeightWorkoutPage.BindingContext = _weightWorkoutManagerVM;
            _addWeightDrillPage.BindingContext = _weightWorkoutManagerVM;
            _addSavedWeightExercises.BindingContext = _weightWorkoutManagerVM;
            _notePage.BindingContext = _weightWorkoutManagerVM;

            //EVENT SUBSCRIBE
            _oneRepetitionMaximumVM.CalculationStartEvent += OnCalculationStarted;
            _weightWorkoutManagerVM.OpenAddWeightExercise += OnOpenAddWeightExercise;
            _weightWorkoutManagerVM.CloseAddWeightExercise += OnCloseNavigationPage;
            _weightWorkoutManagerVM.OpenNoteEditor += OnOpenNoteEditor;
            _weightWorkoutManagerVM.OpenTrainingLog += OnOpenTrainingLog;
            _weightWorkoutManagerVM.OpenHistoryView += OnOpenHistoryView;
            _weightWorkoutManagerVM.WeightExerciseMenuSelected += OnWeightExerciseMenuSelected;
            _weightWorkoutManagerVM.SavedWeightActivitySelected += OnSavedWeightActivitySelected;
            _weightWorkoutManagerVM.OpenEditWeightExercise += OnOpenEditWeightExercise;
            _weightWorkoutManagerVM.MessageApplication += OnMessageApplication;
            _weightWorkoutManagerVM.CloseNoteEditor += OnCloseNavigationPage;
        }

        private async void OnWeightExerciseMenuSelected(object sender, MessageEventArgs e)
        {
            string action = await _navigationTabbedPage.DisplayActionSheet(e.Title, "Cancel", "Delete", "Edit");

            if (action == "Delete")
                _weightWorkoutManagerVM.DeleteExercise(e.Message);

            if (action == "Edit")
                _weightWorkoutManagerVM.EditExercise(e.Message);
        }

        private void OnSavedWeightActivitySelected(object sender, string e)
        {
            _addNewDrillCaruselPage.CurrentPage = _addNewDrillCaruselPage.Children[0];
        }

        private async void OnMessageApplication(object sender, MessageEventArgs e) => await _navigationTabbedPage.DisplayAlert(e.Message, e.Message, "Ok");

        /// <summary>
        /// Alapvető hibakezelés.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExceptionOccured(object sender, ExceptionArgs e) => _navigationTabbedPage.DisplayAlert("Error during start workout.", e.Message, "Ok");

        public Page GetMainPage() => _navigationTabbedPage;

        private void OnCalculationStarted(object sender, EventArgs e) => _oneRepNavigationPage.PushAsync(_oneRepetitionMaximumCalculatedPage);

        private void OnCloseNavigationPage(object sender, EventArgs e)
        {
            switch (((ClosePageEventArgs)e).Page)
            {
                case PageType.WightWorkout:
                    _addNewWeightWorkoutNavigationPage.PopAsync();
                    break;
            }
        }

        private void OnOpenAddWeightExercise(object sender, EventArgs e) => _addNewWeightWorkoutNavigationPage.PushAsync(_addNewDrillCaruselPage);

        private void OnOpenEditWeightExercise(object sender, EventArgs e) => _addNewWeightWorkoutNavigationPage.PushAsync(_addWeightDrillPage);

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
