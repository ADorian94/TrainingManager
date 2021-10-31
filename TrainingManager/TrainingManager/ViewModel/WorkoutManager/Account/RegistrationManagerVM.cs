using System;
using TrainingManager.Model;

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
        public event EventHandler<MessageEventArgs> RegistrationFailed;

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
            if (ArePreconditionsFine())
            {
                AuthenticationStarted?.Invoke(this, EventArgs.Empty);
                bool registrationResult = await _apiServices.RegisterAync(Name, UserName, UserEmail, Password, ConfirmPassword);

                if (registrationResult)
                {
                    bool loginResult = await _apiServices.LoginAsync(UserName, Password);

                    if (loginResult)
                        RegistrationSuccess?.Invoke(this, EventArgs.Empty);
                    else
                        OnMessageApplication("Can't login after registration. Try from the login page.");

                    UserName = string.Empty;
                    UserEmail = string.Empty;
                    Name = string.Empty;

                }
                else
                    RegistrationFailed?.Invoke(this, new MessageEventArgs("Registration failed."));

                Password = string.Empty;
                ConfirmPassword = string.Empty;
            }
        }

        //PRIVATES
        private bool ArePreconditionsFine()
        {
            if (string.IsNullOrEmpty(Name))
            {
                OnMessageApplication("A teljes név megadása kötelező!");
                return false;
            }

            if (string.IsNullOrEmpty(UserName))
            {
                OnMessageApplication("A felhasznélónév megadása kötelező!");
                return false;
            }

            if (string.IsNullOrEmpty(UserEmail))
            {
                OnMessageApplication("Az email cím megadása kötelető!");
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                OnMessageApplication("A jelszó megadása kötelező!");
                return false;
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                OnMessageApplication("A jelszó megerősítése kötelező!");
                return false;
            }

            if (Password != ConfirmPassword)
            {
                OnMessageApplication("A jelszavak nem egyeznek!");
                return false;
            }

            return true;
        }
    }
}
