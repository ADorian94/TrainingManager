namespace TrainingManager.Model.Interfaces
{
    public interface IProfileService
    {
        Task<byte[]> LoadProfilePictureAsync();
        Task StoreProfilePictureAsync(byte[] image);
        bool IsProfilePictureStored();
    }
}
