using System;
using TrainingManager.Model;
using TrainingManager.Model.LogWriter;

namespace TrainingManager.ViewModel.WorkoutManager.Account
{
    public class RegistrationManagerVM : ViewModelBase
    {
        //FIELDS
        private readonly IApiServices _apiServices;

        //PROPERTIES
        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }

        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged(); } }

        private string _userEmail;
        public string UserEmail { get { return _userEmail; } set { _userEmail = value; OnPropertyChanged(); } }

        private string _password;
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged(); } }

        private string _confirmPassword;
        public string ConfirmPassword { get { return _confirmPassword; } set { _confirmPassword = value; OnPropertyChanged(); } }

        //EVENTS
        public event EventHandler AuthenticationStarted;
        public event EventHandler RegistrationSuccess;

        //COMMANDS
        public DelegateCommand RegisterCommand { get; private set; }

        public RegistrationManagerVM(IApiServices apiServices)
        {
            _apiServices = apiServices ?? throw new Exception();
        }

        protected override void InitializeCommands()
        {
            RegisterCommand = new DelegateCommand(RegisterFunction);
        }

        //COMMAND FUNCTIONS
        private async void RegisterFunction(object obj)
        {
            LogHandler.Instance.Nlog.Info("Registration process started.");

            if (ArePreconditionsFine())
            {
                AuthenticationStarted?.Invoke(this, EventArgs.Empty);
                bool registrationResult = await _apiServices.RegisterAync(Name, UserName, UserEmail, Password, ConfirmPassword);

                if (registrationResult)
                {
                    LogHandler.Instance.Nlog.Info("Registration succeed.");
                    bool loginResult = await _apiServices.LoginAsync(UserName, Password);

                    if (loginResult)
                    {
                        LogHandler.Instance.Nlog.Error("Login after registration suceed.");
                        RegistrationSuccess?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        LogHandler.Instance.Nlog.Error("Can't login after registration.");
                        SendPopUpMessage(Messages.LoginFailedAfterRegistration);
                    }

                    UserName = string.Empty;
                    UserEmail = string.Empty;
                    Name = string.Empty;
                }
                else
                {
                    LogHandler.Instance.Nlog.Error("Registration failed.");
                    SendPopUpMessage(Messages.RegistrationFailed);
                }

                Password = string.Empty;
                ConfirmPassword = string.Empty;
            }
        }

        //PRIVATES
        private bool ArePreconditionsFine()
        {
            if (string.IsNullOrEmpty(Name))
            {
                LogHandler.Instance.Nlog.Warn("Can't sign up. Name is null or empty.");
                SendPopUpMessage(Messages.RequiredFirstName);
                return false;
            }

            if (string.IsNullOrEmpty(UserName))
            {
                LogHandler.Instance.Nlog.Warn("Can't sign up. Username is null or empty.");
                SendPopUpMessage(Messages.RequiredUserName);
                return false;
            }

            if (string.IsNullOrEmpty(UserEmail))
            {
                LogHandler.Instance.Nlog.Warn("Can't sign up. UserEmail is null or empty.");
                SendPopUpMessage(Messages.RequiredEmail);
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                LogHandler.Instance.Nlog.Warn("Can't sign up. Password is null or empty.");
                SendPopUpMessage(Messages.RequiredPassword);
                return false;
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                LogHandler.Instance.Nlog.Warn("Can't sign up. ConfirmPassword is null or empty.");
                SendPopUpMessage(Messages.RequiredConfirmPassword);
                return false;
            }

            if (Password != ConfirmPassword)
            {
                LogHandler.Instance.Nlog.Warn("Can't sign up. Password and ConfirmPassword is not equals.");
                SendPopUpMessage(Messages.PasswordsAreNotEquals);
                return false;
            }

            return true;
        }
    }
}
