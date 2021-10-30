using System;
using System.Collections.Generic;
using System.Text;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
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
        private readonly LoadingView _loadingView;
        private readonly RegistrationPage _registrationPage;

        //VIEWMODELLS
        private readonly RegistrationManagerVM _registrationManagerVM;
        private readonly LoginManagerVM _loginManagerVM;

        public AuthenticationNavigationManager(IApiServices apiServices, IAuthService authService)
        {
            _apiServices = apiServices;

            //VIEWMODELS
            _registrationManagerVM = new RegistrationManagerVM(_apiServices);
            _loginManagerVM = new LoginManagerVM(_apiServices, authService);

            //INITIALIZE PAGES
            _loadingView = new LoadingView();
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
            _registrationManagerVM.RegistrationFailed += OnAuthenticationMessage;
            _registrationManagerVM.MessageApplication += OnAuthenticationMessage;
            _registrationManagerVM.AuthenticationStarted += OnAuthenticationStarted;
            _loginManagerVM.AuthenticationStarted += OnAuthenticationStarted;
            _loginManagerVM.LoginSuccess += OnAuthenticationSucceed;
            _loginManagerVM.LoginFailed += OnAuthenticationMessage;
            _loginManagerVM.MessageApplication += OnAuthenticationMessage;

            MainPage = new NavigationPage(_loadingView);
            TryLoginAndSetMainPage();
        }

        private async void TryLoginAndSetMainPage()
        {
            bool loginResult = await _loginManagerVM.TryLoginWithSavedCredentialsAsync();

            if (loginResult)
                AuthenticationSuceed?.Invoke(this, EventArgs.Empty);
            else
            {
                MainPage = new NavigationPage(_loginAndRegisterCaruselPage);
                MainPageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        internal void Logout()
        {
            MainPage = _loginAndRegisterCaruselPage;
            MainPageChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnAuthenticationStarted(object sender, EventArgs e)
        {
            MainPage = _loadingView;
            MainPageChanged?.Invoke(this, EventArgs.Empty);
        }

        private async void OnAuthenticationMessage(object sender, MessageEventArgs e)
        {
            MainPage = _loginAndRegisterCaruselPage;
            MainPageChanged?.Invoke(this, EventArgs.Empty);
            await _loginAndRegisterCaruselPage.DisplayAlert(e.Message, e.Message, "Ok");
        }

        private void OnAuthenticationSucceed(object sender, EventArgs e) => AuthenticationSuceed?.Invoke(this, EventArgs.Empty);
    }
}
