using System;
using TrainingManager.Model;
using TrainingManager.Model.Services;
using TrainingManager.View;
using Xamarin.Forms;

namespace TrainingManager.ViewModel.Navigation
{
    public class PageNavigationManager
    {
        //TABBED PAGE
        private NavigationPage _mainNavigationPage;
        private NavigationTabbedPage _mainTabbedPage;
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
            _homePage = new HomePage() { Title = "Home" };
            _addNewWeightWorkoutPage = new AddNewWeightWorkoutPage() { Title = "Workout" };
            _settingsPage = new SettingsPage() { Title = "Settings" };
            _exercisesPage = new ExercisesPage() { Title = "Exercises" };
            _oneRepetitionMaximumCalculatorPage = new OneRepetitionMaximumCalculatorPage() { Title = "1RM" };
            _oneRepetitionMaximumCalculatedPage = new OneRepetitionMaximumCalculatedPage();
            _addWeightDrillPage = new AddWeightExercisePage();
            _addSavedWeightExercises = new AddSavedWeightExercises();
            _addNewDrillCaruselPage = new AddNewDrillCaruselPage();
            _addNewDrillCaruselPage.Children.Add(_addWeightDrillPage);
            _addNewDrillCaruselPage.Children.Add(_addSavedWeightExercises);
            _notePage = new NotePage();

            _mainTabbedPage = new NavigationTabbedPage();
            _mainTabbedPage.Children.Add(_homePage);
            _mainTabbedPage.Children.Add(_exercisesPage);
            _mainTabbedPage.Children.Add(_addNewWeightWorkoutPage);
            _mainTabbedPage.Children.Add(_oneRepetitionMaximumCalculatorPage);
            _mainTabbedPage.Children.Add(_settingsPage);

            _mainNavigationPage = new NavigationPage(_mainTabbedPage);

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
            _weightWorkoutManagerVM.WeightExerciseMenuSelected += OnWeightExerciseMenuSelected;
            _weightWorkoutManagerVM.SavedWeightActivitySelected += OnSavedWeightActivitySelected;
            _weightWorkoutManagerVM.OpenEditWeightExercise += OnOpenEditWeightExercise;
            _weightWorkoutManagerVM.MessageApplication += OnMessageApplication;
            _weightWorkoutManagerVM.CloseNoteEditor += OnCloseNavigationPage;
            _weightWorkoutManagerVM.ExerciseRoundSelected += OnExerciseRoundSelected;
        }

        private async void OnExerciseRoundSelected(object sender, string e)
        {
            string action = await _addWeightDrillPage.DisplayActionSheet("Do you want to remove the round?", "Cancel", "Delete");

            if (action == "Delete")
                _weightWorkoutManagerVM.DeleteRoundByStringGuid(e);
        }

        private async void OnWeightExerciseMenuSelected(object sender, MessageEventArgs e)
        {
            string action = await _mainTabbedPage.DisplayActionSheet(e.Title, "Cancel", "Delete", "Edit");

            if (action == "Delete")
                _weightWorkoutManagerVM.DeleteExercise(e.Message);

            if (action == "Edit")
                _weightWorkoutManagerVM.EditExercise(e.Message);
        }

        private void OnSavedWeightActivitySelected(object sender, string e)
        {
            _addNewDrillCaruselPage.CurrentPage = _addNewDrillCaruselPage.Children[0];
        }

        private async void OnMessageApplication(object sender, MessageEventArgs e) => await _mainTabbedPage.DisplayAlert(e.Message, e.Message, "Ok");

        /// <summary>
        /// Alapvető hibakezelés.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExceptionOccured(object sender, ExceptionArgs e) => _mainTabbedPage.DisplayAlert("Error during start workout.", e.Message, "Ok");

        public Page GetMainPage() => _mainNavigationPage;

        private void OnCalculationStarted(object sender, EventArgs e) => _mainNavigationPage.PushAsync(_oneRepetitionMaximumCalculatedPage);

        private void OnCloseNavigationPage(object sender, EventArgs e)
        {
            switch (((ClosePageEventArgs)e).Page)
            {
                case PageType.WightWorkout:
                    _mainNavigationPage.PopAsync();
                    break;
            }
        }

        private void OnOpenAddWeightExercise(object sender, EventArgs e) => _mainNavigationPage.PushAsync(_addNewDrillCaruselPage);

        private void OnOpenEditWeightExercise(object sender, EventArgs e) => _mainNavigationPage.PushAsync(_addWeightDrillPage);

        private void OnOpenNoteEditor(object sender, EventArgs e) => _mainNavigationPage.PushAsync(_notePage);
    }
}
