namespace TrainingManager.Model.Interfaces
{
    public interface IMediaService
    {
        Task<byte[]> SelectPhotoAsync();
    }
}
