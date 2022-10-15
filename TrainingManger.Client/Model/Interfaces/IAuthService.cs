namespace TrainingManager.Model.Interfaces
{
    public interface IAuthService
    {
        Task<(string userName, string Password)> GetUserCredentialsAsync();
        void SetUserCredentials(string userName, string Password);
        void RemoveUserCredentials();
    }
}
