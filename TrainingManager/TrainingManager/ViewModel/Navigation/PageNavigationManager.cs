using System;
using System.Linq;
using TrainingManager.Model;
using TrainingManager.Model.Services;
using TrainingManager.View;
using TrainingManager.View.TabbedPageView.History;
using TrainingManager.View.TabbedPageView.History.HistoryPages;
using Xamarin.Forms;

namespace TrainingManager.ViewModel.Navigation
{
#warning Refact required: PageNavigationManager
    public class PageNavigationManager
    {
        //TABBED PAGE
        private readonly NavigationPage _mainNavigationPage;
        private readonly NavigationTabbedPage _mainTabbedPage;
        private readonly HomePage _homePage;
        private readonly AddNewWeightWorkoutPage _addNewWeightWorkoutPage;
        private readonly AddNewDrillCaruselPage _addNewDrillCaruselPage;
        private readonly AddSavedWeightExercises _addSavedWeightExercises;
        private readonly AddWeightExercisePage _addWeightDrillPage;
        private readonly OneRepetitionMaximumCalculatorPage _oneRepetitionMaximumCalculatorPage;
        private readonly OneRepetitionMaximumCalculatedPage _oneRepetitionMaximumCalculatedPage;
        private readonly SettingsPage _settingsPage;
        private readonly ExercisesPage _exercisesPage;
        private readonly NotePage _notePage;
        private readonly NotePage _notePageHistory;

        private readonly HistoryCaruselPage _historyCaruselPage;
        private readonly CalendarPage _calendarHistoryPage;
        private readonly SearchPage _searchHistoryPage;
        private readonly AddNewWeightWorkoutPage _addNewWeightWorkoutPageHistory;
        private readonly AddNewDrillCaruselPage _addNewDrillCaruselPageHistory;
        private readonly AddSavedWeightExercises _addSavedWeightExercisesHistory;
        private readonly AddWeightExercisePage _addWeightDrillPageHistory;

        //VIEWMODELLS
        private readonly OneRepetitionMaximumVM _oneRepetitionMaximumVM;
        private readonly WeightWorkoutManagerVM _weightWorkoutManagerVM;
        private readonly WeightHistoryVM _weightHistoryVM;

        public PageNavigationManager(ApiServices apiServices)
        {
            //INITIALIZE VMS
            _oneRepetitionMaximumVM = new OneRepetitionMaximumVM();
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM(apiServices);
            _weightHistoryVM = new WeightHistoryVM(apiServices);

            //INITIALIZE PAGES
            _homePage = new HomePage() { Title = "Home" };
            _addNewWeightWorkoutPage = new AddNewWeightWorkoutPage("New Workout") { Title = "Workout" };
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
            _calendarHistoryPage = new CalendarPage();
            _searchHistoryPage = new SearchPage();
            _historyCaruselPage = new HistoryCaruselPage() { Title = "History" };
            _historyCaruselPage.Children.Add(_calendarHistoryPage);
            _historyCaruselPage.Children.Add(_searchHistoryPage);
            _addNewWeightWorkoutPageHistory = new AddNewWeightWorkoutPage("Recent Workout") { Title = "Workout" };
            _addNewDrillCaruselPageHistory = new AddNewDrillCaruselPage();
            _addSavedWeightExercisesHistory = new AddSavedWeightExercises();
            _addWeightDrillPageHistory = new AddWeightExercisePage();
            _addNewDrillCaruselPageHistory.Children.Add(_addWeightDrillPageHistory);
            _addNewDrillCaruselPageHistory.Children.Add(_addSavedWeightExercisesHistory);
            _notePageHistory = new NotePage();

            _mainTabbedPage = new NavigationTabbedPage();
            _mainTabbedPage.Children.Add(_homePage);
            _mainTabbedPage.Children.Add(_addNewWeightWorkoutPage);
            _mainTabbedPage.Children.Add(_historyCaruselPage);
            _mainTabbedPage.Children.Add(_exercisesPage);
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
            _calendarHistoryPage.BindingContext = _weightHistoryVM;
            _addSavedWeightExercisesHistory.BindingContext = _weightHistoryVM;
            _addWeightDrillPageHistory.BindingContext = _weightHistoryVM;
            _addSavedWeightExercisesHistory.BindingContext = _weightHistoryVM;
            _addNewWeightWorkoutPageHistory.BindingContext = _weightHistoryVM;
            _searchHistoryPage.BindingContext = _weightHistoryVM;

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
            _weightWorkoutManagerVM.ExceptionAllert += OnExceptionOccured;
            _weightWorkoutManagerVM.WorkoutSaved += _weightHistoryVM.RefreshWorkouts;

            _weightHistoryVM.WeightWorkoutDateSelected += OnWeightWorkoutDateSelected;
            _weightHistoryVM.OpenAddWeightExercise += OnOpenAddWeightExerciseHistory;
            _weightHistoryVM.CloseAddWeightExercise += OnCloseNavigationPage;
            _weightHistoryVM.OpenNoteEditor += OnOpenNoteEditorHistory;
            _weightHistoryVM.WeightExerciseMenuSelected += OnWeightExerciseMenuSelectedHistory;
            _weightHistoryVM.SavedWeightActivitySelected += OnSavedWeightActivitySelectedHistory;
            _weightHistoryVM.OpenEditWeightExercise += OnOpenEditWeightExerciseHistory;
            _weightHistoryVM.MessageApplication += OnMessageApplication;
            _weightHistoryVM.CloseNoteEditor += OnCloseNavigationPage;
            _weightHistoryVM.ExerciseRoundSelected += OnExerciseRoundSelectedHistory;
            _weightHistoryVM.ExceptionAllert += OnExceptionOccured;
            _weightHistoryVM.WorkoutSaved += _weightWorkoutManagerVM.RefreshWorkouts;
            _weightHistoryVM.HistoryWorkoutItemSelected += OnHistoryWorkoutItemSelected;
        }
        public Page GetMainPage() => _mainNavigationPage;

        //EVENT HANDLERS

        private async void OnHistoryWorkoutItemSelected(object sender, MessageEventArgs e)
        {
            string action = await _mainTabbedPage.DisplayActionSheet(e.Title, "Cancel", "Delete", "Edit");

            if (action == "Delete")
                _weightHistoryVM.DeleteWeightWorkoutByStringGuid(e.Message);

            if (action == "Edit")
                await _mainNavigationPage.PushAsync(_addNewWeightWorkoutPageHistory);

        }

        private void OnWeightWorkoutDateSelected(object sender, DateTime e)
        {
            _mainNavigationPage.PushAsync(_addNewWeightWorkoutPageHistory);
        }

        private async void OnExerciseRoundSelected(object sender, string e)
        {
            string action = await _addWeightDrillPage.DisplayActionSheet("Do you want to remove the round?", "Cancel", "Delete");

            if (action == "Delete")
                _weightWorkoutManagerVM.DeleteRoundByStringGuid(e);
        }

        private async void OnExerciseRoundSelectedHistory(object sender, string e)
        {
            string action = await _addWeightDrillPage.DisplayActionSheet("Do you want to remove the round?", "Cancel", "Delete");

            if (action == "Delete")
                _weightHistoryVM.DeleteRoundByStringGuid(e);
        }

        private async void OnWeightExerciseMenuSelected(object sender, MessageEventArgs e)
        {
            string action = await _mainTabbedPage.DisplayActionSheet(e.Title, "Cancel", "Delete", "Edit");

            if (action == "Delete")
                _weightWorkoutManagerVM.DeleteExercise(e.Message);

            if (action == "Edit")
                _weightWorkoutManagerVM.EditExercise(e.Message);
        }

        private async void OnWeightExerciseMenuSelectedHistory(object sender, MessageEventArgs e)
        {
            string action = await _addNewWeightWorkoutPageHistory.DisplayActionSheet(e.Title, "Cancel", "Delete", "Edit");

            if (action == "Delete")
                _weightHistoryVM.DeleteExercise(e.Message);

            if (action == "Edit")
                _weightHistoryVM.EditExercise(e.Message);
        }

        private void OnSavedWeightActivitySelected(object sender, string e) => _addNewDrillCaruselPage.CurrentPage = _addNewDrillCaruselPage.Children.First();
        private void OnSavedWeightActivitySelectedHistory(object sender, string e) => _addNewDrillCaruselPageHistory.CurrentPage = _addNewDrillCaruselPageHistory.Children.First();
        private async void OnMessageApplication(object sender, MessageEventArgs e) => await _mainTabbedPage.DisplayAlert(e.Message, e.Message, "Ok");
        /// <summary>
        /// Alapvető hibakezelés.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExceptionOccured(object sender, MessageEventArgs e) => _mainTabbedPage.DisplayAlert(e.Title, e.Message, "Ok");


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
        private void OnOpenAddWeightExerciseHistory(object sender, EventArgs e) => _mainNavigationPage.PushAsync(_addNewDrillCaruselPageHistory);
        private void OnOpenEditWeightExercise(object sender, EventArgs e) => _mainNavigationPage.PushAsync(_addWeightDrillPage);
        private void OnOpenEditWeightExerciseHistory(object sender, EventArgs e) => _mainNavigationPage.PushAsync(_addWeightDrillPageHistory);
        private void OnOpenNoteEditor(object sender, EventArgs e) => _mainNavigationPage.PushAsync(_notePage);
        private void OnOpenNoteEditorHistory(object sender, EventArgs e) => _mainNavigationPage.PushAsync(_notePageHistory);
    }
}
