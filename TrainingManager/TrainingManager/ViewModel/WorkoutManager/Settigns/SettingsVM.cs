using System;
using TrainingManager.Model;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.Services;
using System.IO;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace TrainingManager.ViewModel.WorkoutManager.Settigns
{
    public class SettingsVM : ViewModelBase
    {
        //FILEDS
        private readonly IApiServices _apiServices;
        private readonly IAuthService _authServices;
        private readonly IMediaService _mediaService;

        //EVENTS
        public event EventHandler LogoutSuccess;
        public event EventHandler<MessageEventArgs> LogoutFailed;
        public event EventHandler ProfileChanged;

        //COMMANDS
        public DelegateCommand SignOutCommand { get; private set; }
        public DelegateCommand UploadImageCommand { get; private set; }

        public SettingsVM(IApiServices apiServices, IAuthService authServices)
        {
            _apiServices = apiServices;
            _authServices = authServices;
            _mediaService = new MediaService();
        }

        protected override void InitializeCommands()
        {
            SignOutCommand = new DelegateCommand(SingOutFunction);
            UploadImageCommand = new DelegateCommand(UploadImageFunction);
        }

        //COMMAND FUNCTION
        private async void UploadImageFunction(object obj)
        {
            var statusCamera = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (statusCamera != PermissionStatus.Granted)
                statusCamera = await Permissions.RequestAsync<Permissions.Camera>();

            var statusStorageRead = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (statusStorageRead != PermissionStatus.Granted)
                await Permissions.RequestAsync<Permissions.StorageRead>();

            if (statusCamera == PermissionStatus.Granted && statusStorageRead == PermissionStatus.Granted)
                SendPopUpMessage(Messages.UploadPictureMessage, SelectAndUploadFunction);
            else
                SendPopUpMessage(Messages.AssesDeniedProfilePictureMessage);
        }

        private async void SelectAndUploadFunction()
        {
            byte[] image = await _mediaService.SelectPhotoAsync();
            await _apiServices.UploadProfilePicture(image);
            byte[] originalImage = await _apiServices.DownloadProfilePicture();

            using (MemoryStream memoryStream = new MemoryStream(originalImage))
            {
                ImageSource imageSource = ImageSource.FromStream(() => memoryStream);
            }

            ProfileChanged?.Invoke(this, EventArgs.Empty);
        }

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
