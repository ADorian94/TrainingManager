using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrainingManager.Data;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.LogWriter;
using TrainingManager.View;
using TrainingManager.View.TabbedPageView;
using TrainingManager.View.TabbedPageView.History;
using TrainingManager.View.TabbedPageView.History.HistoryPages;
using TrainingManager.ViewModel.WorkoutManager;
using TrainingManager.ViewModel.WorkoutManager.Settigns;
using Xamarin.Forms;

namespace TrainingManager.ViewModel.Navigation
{
#warning Refact required: PageNavigationManager
    public class PageNavigationManager
    {
        //FIELDS
        private IApiServices _apiServices;
        private IAuthService _authService;
        private IProfileService _profileService;
        private int _initialAttempts = 0;

        //PROPERTIES
        public Page MainPage { get; private set; }

        //EVENT
        public event EventHandler MainPageChanged;
        public event EventHandler Logout;

        //TABBED PAGE
        private NavigationPage _mainNavigationPage;
        private NavigationTabbedPage _mainTabbedPage;
        private HomePage _homePage;
        private AddNewWeightWorkoutPage _addNewWeightWorkoutPageHome;
        private RecentWorkoutDetails _recentWorkoutDetailsPage;

        private AddNewWeightWorkoutPage _addNewWeightWorkoutPage;
        private AddNewDrillCaruselPage _addNewDrillCaruselPage;
        private AddSavedWeightExercises _addSavedWeightExercises;
        private AddWeightExercisePage _addWeightDrillPage;
        private OneRepetitionMaximumCalculatorPage _oneRepetitionMaximumCalculatorPage;
        private OneRepetitionMaximumCalculatedPage _oneRepetitionMaximumCalculatedPage;
        private SettingsPage _settingsPage;
        private NotePage _notePage;
        private NotePage _notePageHistory;
        private ColorSelectPage _colorSelectPage;
        private MusclePage _muscleSelectPage;
        private HistoryTabbedPage _historyTabbedPage;
        private CalendarPage _calendarHistoryPage;
        private SearchPage _searchHistoryPage;
        private ActivitiesPage _activitiesPage;
        private AddNewWeightWorkoutPage _addNewWeightWorkoutPageHistory;
        private AddNewDrillCaruselPage _addNewDrillCaruselPageHistory;
        private AddSavedWeightExercises _addSavedWeightExercisesHistory;
        private AddWeightExercisePage _addWeightDrillPageHistory;
        private ActivityDetailsPage _activityDetailsPage;

        //VIEWMODELLS
        private OneRepetitionMaximumVM _oneRepetitionMaximumVM;
        private WeightWorkoutManagerVM _weightWorkoutManagerVM;
        private WeightHistoryVM _weightHistoryVM;
        private HomeVM _homeVM;
        private SettingsVM _settingsVM;
        private WeightActivityManagerVM _weightActivityManagerVM;

        public PageNavigationManager(IApiServices apiServices, IAuthService authService, IProfileService profileService)
        {
            _apiServices = apiServices;
            _authService = authService;
            _profileService = profileService;
            LogHandler.Instance.Nlog.Info("PageNavigation manager initialization succeed.");
        }

        public Task InitializeAfterAuthenticationAsync()
        {
            return Task.Run(async () =>
            {
                LogHandler.Instance.Nlog.Info("PageNavigation manager initialization after authentication started.");

                try
                {
                    InitializePages();
                    LogHandler.Instance.Nlog.Info("Page initialization succeed.");
                }
                catch (Exception ex)
                {
                    OnExceptionOccured(this, new ExceptionArgs(ex.Message));
                }

                //CREATE VMs INIT TEHEM CONCURENTLY
                var vmInitializations = new Task[]
                {
                    CreateWeightWorkoutManagerVM(),
                    CreateWeightHistoryVM(),
                    CreateHomeVM(),
                    CreateSettingsVM(),
                    CreateActivitiesVM(),
                    CreateColorVM(),
                    CreateOneRepMaximumVM()
                };

                await Task.WhenAll(vmInitializations);

                //EVENT SUBSCRIBE
                _weightWorkoutManagerVM.WorkoutSaved += _weightHistoryVM.OnRefreshWorkouts;
                _weightWorkoutManagerVM.WorkoutSaved += _homeVM.OnRefreshWorkouts;
                _weightWorkoutManagerVM.WorkoutSaved += _oneRepetitionMaximumVM.Refresh;
                _settingsVM.ProfileChanged += _homeVM.OnProfileChanged;
                _weightHistoryVM.WorkoutDeleted += _homeVM.OnRefreshWorkouts;
                _weightHistoryVM.WorkoutDeleted += _weightWorkoutManagerVM.OnRefreshWorkouts;
                _weightHistoryVM.WorkoutSaved += _homeVM.OnRefreshWorkouts;
                _weightHistoryVM.WorkoutSaved += _oneRepetitionMaximumVM.Refresh;
                _weightActivityManagerVM.NeedToRefresh += _homeVM.OnRefreshWorkouts;
                _weightActivityManagerVM.NeedToRefresh += _weightHistoryVM.OnRefreshWorkouts;
                _weightActivityManagerVM.NeedToRefresh += _weightWorkoutManagerVM.OnRefreshWorkouts;
                _weightActivityManagerVM.NeedToRefresh += _oneRepetitionMaximumVM.Refresh;
                MainPage = _mainNavigationPage;
                MainPageChanged?.Invoke(this, EventArgs.Empty);
                LogHandler.Instance.Nlog.Info("PageNavigation manager initialization after authentication finished.");
            });
        }

        private void InitializePages()
        {
            try
            {
                _homePage = new HomePage();
                _addNewWeightWorkoutPageHome = new AddNewWeightWorkoutPage("Recent");
                _recentWorkoutDetailsPage = new RecentWorkoutDetails();
                _addNewWeightWorkoutPage = new AddNewWeightWorkoutPage("Today Workout");
                _settingsPage = new SettingsPage() { Title = "Settings" };
                _oneRepetitionMaximumCalculatorPage = new OneRepetitionMaximumCalculatorPage();
                _oneRepetitionMaximumCalculatedPage = new OneRepetitionMaximumCalculatedPage();
                _addWeightDrillPage = new AddWeightExercisePage();
                _addSavedWeightExercises = new AddSavedWeightExercises();
                _addNewDrillCaruselPage = new AddNewDrillCaruselPage();
                _addNewDrillCaruselPage.Children.Add(_addWeightDrillPage);
                _addNewDrillCaruselPage.Children.Add(_addSavedWeightExercises);
                _notePage = new NotePage();
                _calendarHistoryPage = new CalendarPage() { Title = "Calendar" };
                _searchHistoryPage = new SearchPage() { Title = "History" };
                _activitiesPage = new ActivitiesPage() { Title = "Exercises" };
                _historyTabbedPage = new HistoryTabbedPage();
                _historyTabbedPage.Children.Add(_calendarHistoryPage);
                _historyTabbedPage.Children.Add(_searchHistoryPage);
                _historyTabbedPage.Children.Add(_activitiesPage);
                _addNewWeightWorkoutPageHistory = new AddNewWeightWorkoutPage("Workout");
                _addNewDrillCaruselPageHistory = new AddNewDrillCaruselPage();
                _addSavedWeightExercisesHistory = new AddSavedWeightExercises();
                _addWeightDrillPageHistory = new AddWeightExercisePage();
                _addNewDrillCaruselPageHistory.Children.Add(_addWeightDrillPageHistory);
                _addNewDrillCaruselPageHistory.Children.Add(_addSavedWeightExercisesHistory);
                _notePageHistory = new NotePage();
                _activityDetailsPage = new ActivityDetailsPage();

                _mainTabbedPage = new NavigationTabbedPage();
                _mainTabbedPage.SelectedTabColor = Color.White;
                _mainTabbedPage.Children.Add(_homePage);
                _mainTabbedPage.Children.Add(_addNewWeightWorkoutPage);
                _mainTabbedPage.Children.Add(_historyTabbedPage);
                _mainTabbedPage.Children.Add(_oneRepetitionMaximumCalculatorPage);
                _mainNavigationPage = new NavigationPage(_mainTabbedPage);
            }
            catch (Exception ex)
            {
                LogHandler.Instance.Nlog.Error($"Error during the page initialization process. Initialization attempt: {_initialAttempts + 1}");
                LogHandler.Instance.Nlog.Error(ex.Message);

                if (_initialAttempts > 3)
                    throw;

                Thread.Sleep(500);
                ++_initialAttempts;
                InitializePages();
            }
        }

        private Task CreateSettingsVM()
        {
            return Task.Run(() =>
            {
                _settingsVM = new SettingsVM(_apiServices, _authService, _profileService);

                _settingsPage.BindingContext = _settingsVM;

                _settingsVM.LogoutSuccess += OnLogoutSuccess;
                _settingsVM.LogoutFailed += OnMessageApplication;
                _settingsVM.PopUpMessageWithCallBack += OnPopUpMessage;
            });
        }

        private Task CreateActivitiesVM()
        {
            return Task.Run(() =>
            {
                _weightActivityManagerVM = new WeightActivityManagerVM(_apiServices);
                _weightActivityManagerVM.WeightActivitySelected += OnWeightActivitySelected;
                _weightActivityManagerVM.MuscleSetup += OnMuscleSetup;
                _weightActivityManagerVM.NeedToRefresh += OnCloseNavigationPage;
                _activitiesPage.BindingContext = _weightActivityManagerVM;
                _activityDetailsPage.BindingContext = _weightActivityManagerVM;
            });
        }

        private Task CreateHomeVM()
        {
            return Task.Run(() =>
            {
                _homeVM = new HomeVM(_apiServices, _profileService, WeightRecordSelected);

                _homePage.BindingContext = _homeVM;
                _addNewWeightWorkoutPageHome.BindingContext = _homeVM;
                _recentWorkoutDetailsPage.BindingContext = _homeVM;

                _homeVM.RecentWorkoutItemSelected += OnRecentWorkoutItemSelected;
                _homeVM.ProfileSelected += OnProfileSelected;
            });
        }

        private Task CreateWeightHistoryVM()
        {
            return Task.Run(() =>
            {
                _weightHistoryVM = new WeightHistoryVM(_apiServices);

                _calendarHistoryPage.BindingContext = _weightHistoryVM;
                _addSavedWeightExercisesHistory.BindingContext = _weightHistoryVM;
                _addWeightDrillPageHistory.BindingContext = _weightHistoryVM;
                _addSavedWeightExercisesHistory.BindingContext = _weightHistoryVM;
                _addNewWeightWorkoutPageHistory.BindingContext = _weightHistoryVM;
                _searchHistoryPage.BindingContext = _weightHistoryVM;
                _notePageHistory.BindingContext = _weightHistoryVM;

                _weightHistoryVM.WeightWorkoutDateSelected += OnWeightWorkoutDateSelected;
                _weightHistoryVM.OpenAddWeightExercise += OnOpenAddWeightExerciseHistory;
                _weightHistoryVM.OpenNoteEditor += OnOpenNoteEditorHistory;
                _weightHistoryVM.WeightExerciseMenuSelected += OnWeightExerciseMenuSelectedHistory;
                _weightHistoryVM.SavedWeightActivitySelected += OnSavedWeightActivitySelectedHistory;
                _weightHistoryVM.OpenEditWeightExercise += OnOpenEditWeightExerciseHistory;
                _weightHistoryVM.MessageApplication += OnMessageApplication;
                _weightHistoryVM.ExerciseRoundSelected += OnExerciseRoundSelectedHistory;
                _weightHistoryVM.ExceptionOccured += OnExceptionOccured;
                _weightHistoryVM.WorkoutSaved += _weightWorkoutManagerVM.OnRefreshWorkouts;
                _weightHistoryVM.HistoryWorkoutItemSelected += OnHistoryWorkoutItemSelected;
                _weightHistoryVM.PopUpMessage += OnPopUpMessage;
                _weightHistoryVM.ClosePage += OnCloseNavigationPage;
                _weightHistoryVM.OpenMuscleSelector += OnOpenMuscleSelectPage;
            });
        }

        private Task CreateWeightWorkoutManagerVM()
        {
            return Task.Run(() =>
            {
                _weightWorkoutManagerVM = new WeightWorkoutManagerVM(_apiServices);

                _addNewWeightWorkoutPage.BindingContext = _weightWorkoutManagerVM;
                _addWeightDrillPage.BindingContext = _weightWorkoutManagerVM;
                _addSavedWeightExercises.BindingContext = _weightWorkoutManagerVM;
                _notePage.BindingContext = _weightWorkoutManagerVM;

                _weightWorkoutManagerVM.OpenAddWeightExercise += OnOpenAddWeightExercise;
                _weightWorkoutManagerVM.OpenNoteEditor += OnOpenNoteEditor;
                _weightWorkoutManagerVM.WeightExerciseMenuSelected += OnWeightExerciseMenuSelected;
                _weightWorkoutManagerVM.SavedWeightActivitySelected += OnSavedWeightActivitySelected;
                _weightWorkoutManagerVM.OpenEditWeightExercise += OnOpenEditWeightExercise;
                _weightWorkoutManagerVM.MessageApplication += OnMessageApplication;
                _weightWorkoutManagerVM.ExerciseRoundSelected += OnExerciseRoundSelected;
                _weightWorkoutManagerVM.ExceptionOccured += OnExceptionOccured;
                _weightWorkoutManagerVM.PopUpMessage += OnPopUpMessage;
                _weightWorkoutManagerVM.ClosePage += OnCloseNavigationPage;
                _weightWorkoutManagerVM.OpenMuscleSelector += OnOpenMuscleSelectPage;
            });
        }

        private Task CreateColorVM()
        {
            return Task.Run(() =>
            {
                _colorSelectPage = new ColorSelectPage();
                _muscleSelectPage = new MusclePage();
            });
        }

        private Task CreateOneRepMaximumVM()
        {
            return Task.Run(() =>
            {
                _oneRepetitionMaximumVM = new OneRepetitionMaximumVM(_apiServices, WeightRecordSelected);

                _oneRepetitionMaximumCalculatorPage.BindingContext = _oneRepetitionMaximumVM;
                _oneRepetitionMaximumCalculatedPage.BindingContext = _oneRepetitionMaximumVM;

                _oneRepetitionMaximumVM.CalculationStartEvent += OnCalculationStarted;
                _oneRepetitionMaximumVM.PopUpMessage += OnPopUpMessage;
            });
        }

        //EVENT HANDLERS
        private async void OnPopUpMessage(object send, Messages message) =>
            await _mainNavigationPage.DisplayAlert(MessageLibrary.Instance.GetMessageType(message), MessageLibrary.Instance.GetMessage(message), "Ok");

        private async void OnPopUpMessage(object sender, (Messages message, Action Callback) e)
        {
            await _mainNavigationPage.DisplayAlert(MessageLibrary.Instance.GetMessageType(e.message), MessageLibrary.Instance.GetMessage(e.message), "Ok");
            e.Callback?.Invoke();
        }

        private async void OnRecentWorkoutItemSelected(object sender, MessageEventArgs e)
        {
            try
            {
                string action = await _mainTabbedPage.DisplayActionSheet(e.Title, "Cancel", null, "Details");

                if (action == "Details")
                    await _mainNavigationPage.PushAsync(_recentWorkoutDetailsPage);
            }
            catch (Exception ex)
            {
                LogHandler.Instance.Nlog.Error(ex.Message);
                throw;
            }
        }

        private async void OnHistoryWorkoutItemSelected(object sender, MessageEventArgs e)
        {
            string action = await _mainTabbedPage.DisplayActionSheet(e.Title, "Cancel", null, "Delete", "Edit");

            if (action == "Delete")
                _weightHistoryVM.DeleteWeightWorkoutByStringGuid(e.Message);

            if (action == "Edit")
                await _mainNavigationPage.PushAsync(_addNewWeightWorkoutPageHistory);
        }

        private async void OnExerciseRoundSelected(object sender, string e)
        {
            string action = await _addWeightDrillPage.DisplayActionSheet("Round selected.", "Cancel", null, "Duplicate", "Color", "Delete");

            if (action == "Duplicate")
                _weightWorkoutManagerVM.DuplicateRoundByStringGuid(e);

            if (action == "Color")
            {
                EnumeratorVM<MaterialColors> viewModel = _weightWorkoutManagerVM.GetColorVMByRoundGuid(e);
                viewModel.ItemSelected += OnColorSelected;
                _colorSelectPage.BindingContext = viewModel;
                await _mainNavigationPage.PushAsync(_colorSelectPage);
            }

            if (action == "Delete")
                _weightWorkoutManagerVM.DeleteRoundByStringGuid(e);
        }

        private async void OnColorSelected(object sender, MaterialColors e)
        {
            ((EnumeratorVM<MaterialColors>)_colorSelectPage.BindingContext).ItemSelected -= OnColorSelected;
            _weightWorkoutManagerVM.CheckChangesAndSetResult();
            _weightHistoryVM.CheckChangesAndSetResult();
            await _mainNavigationPage.PopAsync();
        }

        private async void OnMuscleSelected(object sender, Muscle e)
        {
            ((MuscleVM)_muscleSelectPage.BindingContext).MuscleSelected -= OnMuscleSelected;
            _weightWorkoutManagerVM.CheckChangesAndSetResult();
            _weightHistoryVM.CheckChangesAndSetResult();
            await _mainNavigationPage.PopAsync();
        }

        private async void OnExerciseRoundSelectedHistory(object sender, string e)
        {
            string action = await _addWeightDrillPageHistory.DisplayActionSheet("Round selected.", "Cancel", null, "Duplicate", "Color", "Delete");

            if (action == "Duplicate")
                _weightHistoryVM.DuplicateRoundByStringGuid(e);

            if (action == "Color")
            {
                EnumeratorVM<MaterialColors> viewModel = _weightHistoryVM.GetColorVMByRoundGuid(e);
                viewModel.ItemSelected += OnColorSelected;
                _colorSelectPage.BindingContext = viewModel;
                await _mainNavigationPage.PushAsync(_colorSelectPage);
            }

            if (action == "Delete")
                _weightHistoryVM.DeleteRoundByStringGuid(e);
        }

        private async void OnWeightExerciseMenuSelected(object sender, MessageEventArgs e)
        {
            string action = await _mainTabbedPage.DisplayActionSheet(e.Title, "Cancel", null, "Edit", "Color", "Delete");

            if (action == "Delete")
                _weightWorkoutManagerVM.DeleteExercise(e.Message);

            if (action == "Color")
            {
                EnumeratorVM<MaterialColors> viewModel = _weightWorkoutManagerVM.GetColorVMByExerciseGuid(e.Message);
                viewModel.ItemSelected += OnColorSelected;
                _colorSelectPage.BindingContext = viewModel;
                await _mainNavigationPage.PushAsync(_colorSelectPage);
            }

            if (action == "Edit")
                _weightWorkoutManagerVM.EditExercise(e.Message);
        }

        private async void OnWeightExerciseMenuSelectedHistory(object sender, MessageEventArgs e)
        {
            string action = await _addNewWeightWorkoutPageHistory.DisplayActionSheet(e.Title, "Cancel", null, "Edit", "Color", "Delete");

            if (action == "Delete")
                _weightHistoryVM.DeleteExercise(e.Message);

            if (action == "Color")
            {
                EnumeratorVM<MaterialColors> viewModel = _weightHistoryVM.GetColorVMByExerciseGuid(e.Message);
                viewModel.ItemSelected += OnColorSelected;
                _colorSelectPage.BindingContext = viewModel;
                await _mainNavigationPage.PushAsync(_colorSelectPage);
            }

            if (action == "Edit")
                _weightHistoryVM.EditExercise(e.Message);
        }

        private async void OnMessageApplication(object sender, MessageEventArgs e) =>
            await _mainTabbedPage.DisplayAlert(e.Message, e.Message, "Ok");

        private void OnSavedWeightActivitySelectedHistory(object sender, EventArgs e) => _addNewDrillCaruselPageHistory.CurrentPage = _addNewDrillCaruselPageHistory.Children.First();
        private void OnSavedWeightActivitySelected(object sender, EventArgs e) => _addNewDrillCaruselPage.CurrentPage = _addNewDrillCaruselPage.Children.First();
        private async void OnExceptionOccured(object sender, ExceptionArgs e) => await _mainTabbedPage.DisplayAlert("Error", e.Message, "Ok");
        private async void OnCalculationStarted(object sender, EventArgs e) => await _mainNavigationPage.PushAsync(_oneRepetitionMaximumCalculatedPage);
        private async void OnCloseNavigationPage(object sender, EventArgs e) => await _mainNavigationPage.PopAsync();
        private async void OnCloseNavigationPage(object sender, Muscle e) => await _mainNavigationPage.PopAsync();
        private async void OnWeightWorkoutDateSelected(object sender, DateTime e) => await _mainNavigationPage.PushAsync(_addNewWeightWorkoutPageHistory);
        private void OnLogoutSuccess(object sender, EventArgs e) => Logout?.Invoke(this, EventArgs.Empty);
        private async void OnOpenAddWeightExercise(object sender, EventArgs e) => await _mainNavigationPage.PushAsync(_addNewDrillCaruselPage);
        private async void OnOpenAddWeightExerciseHistory(object sender, EventArgs e) => await _mainNavigationPage.PushAsync(_addNewDrillCaruselPageHistory);
        private async void OnOpenMuscleSelectPage(object sender, MessageEventArgs e)
        {
            MuscleVM viewModel = ((WorkoutManagerBaseVM)sender).GetMuscleVMByExerciseGuid(e.Message);
            viewModel.MuscleSelected += OnMuscleSelected;
            _muscleSelectPage.BindingContext = viewModel;
            await _mainNavigationPage.PushAsync(_muscleSelectPage);
        }
        private async void OnOpenEditWeightExercise(object sender, EventArgs e) => await _mainNavigationPage.PushAsync(_addWeightDrillPage);
        private async void OnOpenEditWeightExerciseHistory(object sender, EventArgs e) => await _mainNavigationPage.PushAsync(_addWeightDrillPageHistory);
        private async void OnOpenNoteEditor(object sender, EventArgs e) => await _mainNavigationPage.PushAsync(_notePage);
        private async void OnOpenNoteEditorHistory(object sender, EventArgs e) => await _mainNavigationPage.PushAsync(_notePageHistory);
        private async void OnProfileSelected(object sender, EventArgs e) => await _mainNavigationPage.PushAsync(_settingsPage);
        private async void OnWeightActivitySelected(object sender, EventArgs e) => await _mainNavigationPage.PushAsync(_activityDetailsPage);

        private async void OnMuscleSetup(object sender, EventArgs e)
        {
            _weightActivityManagerVM.MuscleVM.MuscleSelected += OnCloseNavigationPage;
            _muscleSelectPage.BindingContext = _weightActivityManagerVM.MuscleVM;
            await _mainNavigationPage.PushAsync(_muscleSelectPage);
        }

        private void WeightRecordSelected(PersonalRecordVM record) => _weightActivityManagerVM.SetupRecord(record);
    }
}
