using System;
using System.Collections.Generic;
using System.Text;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;

namespace TrainingManager.ViewModel.WorkoutManager.Settigns
{
    public class SettingsVM : ViewModelBase
    {
        //FILEDS
        private readonly IApiServices _apiServices;
        private readonly IAuthService _authServices;

        //EVENTS
        public event EventHandler LogoutSuccess;
        public event EventHandler<MessageEventArgs> LogoutFailed;

        //COMMANDS
        public DelegateCommand SignOutCommand { get; private set; }

        public SettingsVM(IApiServices apiServices, IAuthService authServices)
        {
            _apiServices = apiServices;
            _authServices = authServices;
        }

        protected override void InitializeCommands()
        {
            SignOutCommand = new DelegateCommand(SingOutFunction);
        }

        //COMMAND FUNCTION
        private async void SingOutFunction(object obj)
        {
            bool result = await _apiServices.LogoutAsync();

            if (result)
            {
                LogoutSuccess?.Invoke(this, EventArgs.Empty);
                _authServices.RemoveUserCredentials();
            }
            else
                LogoutFailed?.Invoke(this, new MessageEventArgs("Error during the log out process."));
        }
    }
}
