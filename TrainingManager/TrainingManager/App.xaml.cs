using System;
using System.Globalization;
using System.Threading;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.Services;
using TrainingManager.ViewModel.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
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
            InitializeComponent();
            _apiService = new ApiServices("http://trainingmanagerwebapi.azurewebsites.net");
            _authService = new AuthService();
            _authenticationNavigationManager = new AuthenticationNavigationManager(_apiService, _authService);
            _pageNavigationManager = new PageNavigationManager(_apiService, _authService);
            _pageNavigationManager.MainPageChanged += OnMainPageChanged;
            _pageNavigationManager.Logout += OnLogout;
            _authenticationNavigationManager.MainPageChanged += OnAuthenticationMainPageChanged;
            _authenticationNavigationManager.AuthenticationSuceed += OnAuthenticationSuceed;
            MainPage = _authenticationNavigationManager.MainPage;
        }

        private void OnLogout(object sender, EventArgs e)
        {
            _authenticationNavigationManager.Logout();
        }

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
