using System;
using System.Security;
using System.Threading.Tasks;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;

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
        public event EventHandler<MessageEventArgs> LoginFailed;

        public LoginManagerVM(IApiServices apiServices, IAuthService authService)
        {
            _apiServices = apiServices;
            _authService = authService;
        }

        public async Task<bool> TryLoginWithSavedCredentialsAsync()
        {
            (string userName, string userPassword) = await _authService.GetUserCredentialsAsync();
            bool result = !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword);

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
            if (ArePreconditionsFine())
            {
                AuthenticationStarted?.Invoke(this, EventArgs.Empty);

                bool loginResult = await _apiServices.LoginAsync(UserName, Password);

                if (loginResult)
                {
                    LoginSuccess?.Invoke(this, EventArgs.Empty);
                    _authService.SetUserCredentials(UserName, Password);
                    UserName = string.Empty;
                }
                else
                    LoginFailed?.Invoke(this, new MessageEventArgs("Loogn failed."));

                Password = string.Empty;
            }
        }

        //PRIVATES
        private bool ArePreconditionsFine()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                OnMessageApplication("A felhasznélónév megadása kötelező!");
                return false;
            }

            if (string.IsNullOrEmpty(Password.ToString()))
            {
                OnMessageApplication("A jelszó megadása kötelező!");
                return false;
            }

            return true;
        }
    }
}
