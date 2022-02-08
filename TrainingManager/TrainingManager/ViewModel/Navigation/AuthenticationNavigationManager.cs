using System;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.LogWriter;
using TrainingManager.View.LoginAndRegistration;
using TrainingManager.ViewModel.WorkoutManager.Account;
using Xamarin.Forms;

namespace TrainingManager.ViewModel.Navigation
{
    public class AuthenticationNavigationManager
    {
        //FIELDS
        private IApiServices _apiServices;

        //PROPERTIES
        public Page MainPage { get; private set; }

        //EVENT
        public event EventHandler MainPageChanged;
        public event EventHandler AuthenticationSuceed;

        //PAGES
        private readonly LoginAndRegistrationCaruselPage _loginAndRegisterCaruselPage;
        private readonly LoginPage _loginPage;
        private readonly RegistrationPage _registrationPage;

        //VIEWMODELLS
        private readonly RegistrationManagerVM _registrationManagerVM;
        private readonly LoginManagerVM _loginManagerVM;

        public AuthenticationNavigationManager(IApiServices apiServices, IAuthService authService)
        {
            LogHandler.Instance.Nlog.Info("Authentication Manager Initialization started.");
            _apiServices = apiServices;

            //VIEWMODELS
            _registrationManagerVM = new RegistrationManagerVM(_apiServices);
            _loginManagerVM = new LoginManagerVM(_apiServices, authService);

            //INITIALIZE PAGES
            _loginAndRegisterCaruselPage = new LoginAndRegistrationCaruselPage();
            _loginPage = new LoginPage();
            _registrationPage = new RegistrationPage();
            _loginAndRegisterCaruselPage.Children.Add(_loginPage);
            _loginAndRegisterCaruselPage.Children.Add(_registrationPage);

            //BINDINGCONTEXT
            _registrationPage.BindingContext = _registrationManagerVM;
            _loginPage.BindingContext = _loginManagerVM;

            //EVENT SUBSCRIBE
            _registrationManagerVM.RegistrationSuccess += OnAuthenticationSucceed;
            _registrationManagerVM.PopUpMessage += OnPopUpMessageMessage;
            _registrationManagerVM.AuthenticationStarted += OnAuthenticationStarted;
            _loginManagerVM.AuthenticationStarted += OnAuthenticationStarted;
            _loginManagerVM.LoginSuccess += OnAuthenticationSucceed;
            _loginManagerVM.PopUpMessage += OnPopUpMessageMessage;

            MainPage = new NavigationPage(new LoadingView());
            TryLoginAndSetMainPage();
            LogHandler.Instance.Nlog.Info("Authentication Manager Initialization finished.");
        }

        private async void TryLoginAndSetMainPage()
        {
            LogHandler.Instance.Nlog.Info("Authentication attempt.");
            bool loginResult = await _loginManagerVM.TryLoginWithSavedCredentialsAsync();

            if (loginResult)
            {
                LogHandler.Instance.Nlog.Info("Authentication succeed.");
                AuthenticationSuceed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                LogHandler.Instance.Nlog.Info("Authentication failed.");
                MainPage = new NavigationPage(_loginAndRegisterCaruselPage);
                MainPageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        internal void Logout()
        {
            MainPage = _loginAndRegisterCaruselPage;
            MainPageChanged?.Invoke(this, EventArgs.Empty);
        }

        //EVENT HANDLERS
        private void OnAuthenticationStarted(object sender, EventArgs e)
        {
            MainPage = new LoadingView();
            MainPageChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnAuthenticationSucceed(object sender, EventArgs e) => AuthenticationSuceed?.Invoke(this, EventArgs.Empty);

        private async void OnPopUpMessageMessage(object sender, Messages e)
        {
            MainPage = _loginAndRegisterCaruselPage;
            MainPageChanged?.Invoke(this, EventArgs.Empty);
            await _loginAndRegisterCaruselPage.DisplayAlert(MessageLibrary.Instance.GetMessageType(e), MessageLibrary.Instance.GetMessage(e), "Ok");
        }
    }
}
