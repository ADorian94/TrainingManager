using System;
using TrainingManager.Model;
using TrainingManager.Model.Navigation.MasterDetailPageItem;
using TrainingManager.View;
using TrainingManager.View.MasterDetailNavigationPage;
using TrainingManager.View.Timer.IntervallTimer;
using TrainingManager.View.WeightWorkout;
using TrainingManager.View.WeightWorkout.WeightHistory;
using TrainingManager.View.WeightWorkout.WeightTrainingLog;
using TrainingManager.View.WeightWorkout.WeightWorkoutNotes;
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
        private TodayWeightWorkout _todayWeightWorkout;
        private AddExerciseCaruselPage _addExerciseCaruselPage;
        private AddWeightExercise _addWeightExercise;
        private SavedWEightExercises _savedWEightExercises;
        private NoteEditor _noteEditor;
        private NavigationPage _navigationPage;
        private WeightWorkoutMenu _weightWorkoutMenu;
        private HistoryView _weightHistoryView;
        private WorkoutHistoryDetails _workoutHistoryDetails;

        //VIEWMODELLS
        private MasterDetailNavigationPageMasterViewModel _masterDetailNavigationPageMasterVM;
        private OneRepetitionMaximumVM _oneRepetitionMaximumVM;
        private ExerciseTimerVM _exerciseTimerVM;
        private IntervallTimerManagerVM _intervallTimerVM;
        private WeightWorkoutManagerVM _weightWorkoutManagerVM;
        private WeightWorkoutHistoryManagerVM _weightWorkoutHistoryManagerVM;

        public PageNavigationManager()
        {
            //INITIALIZE VMS
            _masterDetailNavigationPageMasterVM = new MasterDetailNavigationPageMasterViewModel();
            _oneRepetitionMaximumVM = new OneRepetitionMaximumVM();
            _exerciseTimerVM = new ExerciseTimerVM();
            _intervallTimerVM = new IntervallTimerManagerVM();
            _weightWorkoutManagerVM = new WeightWorkoutManagerVM();
            _weightWorkoutHistoryManagerVM = new WeightWorkoutHistoryManagerVM();

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
            _todayWeightWorkout = new TodayWeightWorkout();
            _addExerciseCaruselPage = new AddExerciseCaruselPage();
            _addExerciseCaruselPage.Title = "Workout log";
            _addWeightExercise = new AddWeightExercise();
            _savedWEightExercises = new SavedWEightExercises();
            _addExerciseCaruselPage.Children.Add(_savedWEightExercises);
            _addExerciseCaruselPage.Children.Add(_addWeightExercise);
            _noteEditor = new NoteEditor();
            _weightWorkoutMenu = new WeightWorkoutMenu();
            _weightHistoryView = new HistoryView();
            _workoutHistoryDetails = new WorkoutHistoryDetails();

            _navigationPage = new NavigationPage(_todayWeightWorkout);
            _masterDetailNavigationPage = new MasterDetailNavigationPage();
            _masterDetailNavigationPage.Detail = _navigationPage;

            //BINDINGCONTEXT
            _masterDetailNavigationPage.BindingContext = _masterDetailNavigationPageMasterVM;
            _mainPage.BindingContext = _oneRepetitionMaximumVM;
            _oneRepetitionMaximumCalculatedPage.BindingContext = _oneRepetitionMaximumVM;
            _exerciseTimer.BindingContext = _exerciseTimerVM;
            _activeIntervallTimerPage.BindingContext = _intervallTimerVM;
            _intervallWorkoutsPage.BindingContext = _intervallTimerVM;
            _todayWeightWorkout.BindingContext = _weightWorkoutManagerVM;
            _addWeightExercise.BindingContext = _weightWorkoutManagerVM;
            _noteEditor.BindingContext = _weightWorkoutManagerVM;
            _weightWorkoutMenu.BindingContext = _weightWorkoutManagerVM;
            _weightHistoryView.BindingContext = _weightWorkoutHistoryManagerVM;
            _workoutHistoryDetails.BindingContext = _weightWorkoutHistoryManagerVM;

            //EVENT SUBSCRIBE
            _masterDetailNavigationPage.DetailPageSelected += OnDetailPageSelected;
            _oneRepetitionMaximumVM.CalculationStartEvent += OnCalculationStarted;
            _intervallTimerVM.OpenNewIntervallPage += OnNewIntervallPage;
            _intervallTimerVM.OpenNewIntervallWorkoutPage += OnOpenNewIntervallWorkoutPage;
            _intervallTimerVM.CloseAddNewIntervallPage += OnCloseNavigationPage;
            _intervallTimerVM.CloseNewIntervallWorkoutPage += OnCloseNavigationPage;
            _intervallTimerVM.ExceptionOccured += OnExceptionOccured;
            _intervallTimerVM.MessageApplication += OnMessageApplication;
            _intervallTimerVM.WorkoutMenuSelected += OnWorkoutMenuSelected;
            _intervallTimerVM.WorkoutSelected += OnWorkoutSelected;
            _intervallTimerVM.IntervallMenuSelected += OnIntervallMenuelected;
            _weightWorkoutManagerVM.OpenAddWeightExercise += OnOpenAddWeightExercise;
            _weightWorkoutManagerVM.CloseAddWeightExercise += OnCloseNavigationPage;
            _weightWorkoutManagerVM.OpenNoteEditor += OnOpenNoteEditor;
            _weightWorkoutManagerVM.OpenTrainingLog += OnOpenTrainingLog;
            _weightWorkoutManagerVM.OpenHistoryView += OnOpenHistoryView;
            _weightWorkoutManagerVM.WeightExerciseMenuSelected += OnWeightExerciseMenuSelected;
            _weightWorkoutManagerVM.OpenEditWeightExercise += OnOpenEditWeightExercise;
            _weightWorkoutManagerVM.MessageApplication += OnMessageApplication;
            _weightWorkoutHistoryManagerVM.OpenWorkoutDetails += OnOpenWorkoutDetails;
        }

        private void _intervallTimerVM_WorkoutSelected(object sender, MessageEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void OnWeightExerciseMenuSelected(object sender, MessageEventArgs e)
        {
            string action = await _masterDetailNavigationPage.DisplayActionSheet(e.Message, "Cancel", "Delete", "Edit");

            if (action == "Delete")
                _weightWorkoutManagerVM.DeleteExercise(e.Message);

            if (action == "Edit")
                _weightWorkoutManagerVM.EditExercise(e.Message);
        }

        private async void OnIntervallMenuelected(object sender, MessageEventArgs e)
        {
            string action = await _masterDetailNavigationPage.DisplayActionSheet(e.Message, "Cancel", "Delete", "Edit");

            if (action == "Delete")
                _intervallTimerVM.DeleteIntervall(e.Message);

            if (action == "Edit")
                _intervallTimerVM.EditIntervall(e.Message);
        }

        private void OnWorkoutSelected(object sender, EventArgs e)
        {
            _intervallTimerTabbedPage.CurrentPage = _activeIntervallTimerPage;
        }

        private async void OnWorkoutMenuSelected(object sender, MessageEventArgs e)
        {
            string action = await _masterDetailNavigationPage.DisplayActionSheet(e.Message, "Cancel", "Delete", "Edit");

            if (action == "Delete")
                _intervallTimerVM.DeleteWorkout(e.Message);

            if (action == "Edit")
                _intervallTimerVM.EditWorkout(e.Message);
        }

        private async void OnMessageApplication(object sender, MessageEventArgs e) =>
            await _masterDetailNavigationPage.DisplayAlert(e.Message, e.Message, "Ok");

        /// <summary>
        /// Alapvetá hibakezelés.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExceptionOccured(object sender, Model.ExceptionArgs e)
        {
            _masterDetailNavigationPage.DisplayAlert("Error during start workout.", e.Message, "Ok");
        }

        public Page GetMainPage() => _masterDetailNavigationPage;

        /// <summary>
        /// Alapvető oldal váltás.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            if (item.TargetType == typeof(WeightWorkoutMenu))
                _navigationPage = new NavigationPage(_weightWorkoutMenu);

            _masterDetailNavigationPage.Detail = _navigationPage;

            _masterDetailNavigationPage.IsPresented = false;
        }

        private void OnCalculationStarted(object sender, EventArgs e)
        {
            _navigationPage.PushAsync(_oneRepetitionMaximumCalculatedPage);
            _masterDetailNavigationPage.Detail = _navigationPage;
        }



        private void OnOpenNewIntervallWorkoutPage(object sender, EventArgs e)
        {
            _intervallWorkoutPage = new AddIntervallWorkoutPage();
            _intervallWorkoutPage.BindingContext = _intervallTimerVM;
            _navigationPage.PushAsync(_intervallWorkoutPage);
            _masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnCloseNavigationPage(object sender, EventArgs e)
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

        private void OnOpenAddWeightExercise(object sender, EventArgs e)
        {
            _navigationPage.PushAsync(_addExerciseCaruselPage);
            _masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnOpenEditWeightExercise(object sender, EventArgs e)
        {
            _navigationPage.PushAsync(_addWeightExercise);
            _masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnOpenNoteEditor(object sender, EventArgs e)
        {
            _navigationPage.PushAsync(_noteEditor);
            _masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnOpenTrainingLog(object sender, EventArgs e)
        {
            _navigationPage.PushAsync(_todayWeightWorkout);
            _masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnOpenHistoryView(object sender, EventArgs e)
        {
            _navigationPage.PushAsync(_weightHistoryView);
            _masterDetailNavigationPage.Detail = _navigationPage;
        }

        private void OnOpenWorkoutDetails(object sender, EventArgs e)
        {
            _navigationPage.PushAsync(_workoutHistoryDetails);
            _masterDetailNavigationPage.Detail = _navigationPage;
        }
    }
}
