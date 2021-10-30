using System;
using System.Threading.Tasks;
using TrainingManager.Model.Interfaces;
using Xamarin.Essentials;

namespace TrainingManager.Model.Services
{
    public class AuthService : IAuthService
    {
        //FIELDS
        private const string USER_NAME = "UserName";
        private const string USER_PASSWORD = "UserPassword";

        //EVENT
        public event EventHandler FailedToStoreUserCredentials;

        public async Task<(string userName, string Password)> GetUserCredentialsAsync()
        {
            try
            {
                string userName = await SecureStorage.GetAsync(USER_NAME);
                string userPassword = await SecureStorage.GetAsync(USER_PASSWORD);
                return (userName, userPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async void SetUserCredentials(string userName, string password)
        {
            try
            {
                await SecureStorage.SetAsync(USER_NAME, userName);
                await SecureStorage.SetAsync(USER_PASSWORD, password);
            }
            catch (Exception)
            {
                FailedToStoreUserCredentials?.Invoke(this, EventArgs.Empty);
            }
        }

        public void RemoveUserCredentials()
        {
            SecureStorage.Remove(USER_NAME);
            SecureStorage.Remove(USER_PASSWORD);
        }
    }
}
