using System;
using System.Security;
using TrainingManager.Model;

namespace TrainingManager.ViewModel.WorkoutManager.Account
{
    public class LoginManagerVM : ViewModelBase
    {
        //FIELDS
        private readonly IApiServices _apiServices;

        //PROPERTIES
        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged(); } }

        private string _password;
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged(); } }

        //EVENTS
        public event EventHandler LoginSuccess;
        public event EventHandler AuthenticationStarted;
        public event EventHandler<MessageEventArgs> LoginFailed;

        public LoginManagerVM(IApiServices apiServices)
        {
            _apiServices = apiServices;
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

                bool registrationResult = await _apiServices.LoginAsync(UserName, Password);

                if (registrationResult)
                {
                    LoginSuccess?.Invoke(this, EventArgs.Empty);
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
