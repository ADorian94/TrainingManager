using System;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.LogWriter;
using TrainingManager.Model.Services;
using TrainingManager.ViewModel.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TrainingManager
{
    public partial class App : Application
    {
        //FIELDS
        private AuthenticationNavigationManager _authenticationNavigationManager;
        private PageNavigationManager _pageNavigationManager;
        private IApiServices _apiService;
        private IAuthService _authService;

        public App()
        {
            LogWriter.Instance.Nlog.Info("**********NEW RUN**********");
            CheckPermissions();
            LogWriter.Instance.Nlog.Info("Permissions checked");
            _apiService = new ApiServices("http://trainingmanagerwebapi.azurewebsites.net");
            LogWriter.Instance.Nlog.Info("Api service initialized");
            _authService = new AuthService();
            _authenticationNavigationManager = new AuthenticationNavigationManager(_apiService, _authService);
            _pageNavigationManager = new PageNavigationManager(_apiService, _authService);
            _pageNavigationManager.MainPageChanged += OnMainPageChanged;
            _pageNavigationManager.Logout += OnLogout;
            _authenticationNavigationManager.MainPageChanged += OnAuthenticationMainPageChanged;
            _authenticationNavigationManager.AuthenticationSuceed += OnAuthenticationSuceed;
            InitializeComponent();
            LogWriter.Instance.Nlog.Info("Component initialized");
            MainPage = _authenticationNavigationManager.MainPage;
        }

        private async void CheckPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
                await Permissions.RequestAsync<Permissions.Camera>();

            status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (status != PermissionStatus.Granted)
                await Permissions.RequestAsync<Permissions.StorageWrite>();

            status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status != PermissionStatus.Granted)
                await Permissions.RequestAsync<Permissions.StorageRead>();
        }

        private void OnLogout(object sender, EventArgs e) => _authenticationNavigationManager.Logout();
        private void OnAuthenticationSuceed(object sender, EventArgs e) =>
            Dispatcher.BeginInvokeOnMainThread(async () => await _pageNavigationManager.InitializeAfterAuthenticationAsync());
        private void OnMainPageChanged(object sender, EventArgs e) => Dispatcher.BeginInvokeOnMainThread(() => MainPage = _pageNavigationManager.MainPage);
        private void OnAuthenticationMainPageChanged(object sender, EventArgs e) => MainPage = _authenticationNavigationManager.MainPage;

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
