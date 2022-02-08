using System;
using System.Threading.Tasks;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.LogWriter;

namespace TrainingManager.ViewModel.WorkoutManager.Account
{
    public class LoginManagerVM : ViewModelBase
    {
        //FIELDS
        private readonly IApiServices _apiServices;
        private readonly IAuthService _authService;

        //PROPERTIES
        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged(); } }

        private string _password;
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged(); } }

        //EVENTS
        public event EventHandler LoginSuccess;
        public event EventHandler AuthenticationStarted;

        public LoginManagerVM(IApiServices apiServices, IAuthService authService)
        {
            _apiServices = apiServices;
            _authService = authService;
        }

        public async Task<bool> TryLoginWithSavedCredentialsAsync()
        {
            LogHandler.Instance.Nlog.Info("Auto login process started.");
            (string userName, string userPassword) = await _authService.GetUserCredentialsAsync();
            bool result = !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword);
            LogHandler.Instance.Nlog.Info($"Stored username is empty: {string.IsNullOrEmpty(userName)}, stored password is empty: {string.IsNullOrEmpty(userPassword)}");

            if (result)
            {
                bool loginResult = await _apiServices.LoginAsync(userName, userPassword);
                return loginResult;
            }

            return false;
        }

        //COMMANDS
        public DelegateCommand LoginCommand { get; private set; }

        protected override void InitializeCommands()
        {
            LoginCommand = new DelegateCommand(LoginFunction);
        }

        //COMMAND FUNCTIONS
        private async void LoginFunction(object obj)
        {
            LogHandler.Instance.Nlog.Info("Login process started.");

            if (IsReadyReadyToLogin())
            {
                AuthenticationStarted?.Invoke(this, EventArgs.Empty);
                bool loginResult = await _apiServices.LoginAsync(UserName, Password);

                if (loginResult)
                {
                    LogHandler.Instance.Nlog.Info("Login succeed.");
                    LoginSuccess?.Invoke(this, EventArgs.Empty);
                    _authService.SetUserCredentials(UserName, Password);
                    UserName = string.Empty;
                }
                else
                {
                    LogHandler.Instance.Nlog.Error("Login failed.");
                    SendPopUpMessage(Messages.LoginFailed);
                }

                Password = string.Empty;
            }
        }

        //PRIVATES
        private bool IsReadyReadyToLogin()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                SendPopUpMessage(Messages.EmptyUserName);
                LogHandler.Instance.Nlog.Warn("Can't login. Username is null or empty.");
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                SendPopUpMessage(Messages.EmptyPassword);
                LogHandler.Instance.Nlog.Warn("Can't login. Password is null or empty.");
                return false;
            }

            return true;
        }
    }
}
