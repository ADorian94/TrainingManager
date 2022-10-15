using TrainingManager.Model.Interfaces;

namespace TrainingManager.Model.Services
{
    public class MediaService : IMediaService
    {
        public async Task<byte[]> SelectPhotoAsync()
        {
            FileResult photo = await MediaPicker.PickPhotoAsync();
            byte[] photoArray = await LoadPhotoAsync(photo);
            return photoArray;
        }

        private async Task<byte[]> LoadPhotoAsync(FileResult photo)
        {
            if (photo != null)
            {
                using (Stream stream = await photo.OpenReadAsync())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            else
            {
                return null;
            }
        }
    }
}
