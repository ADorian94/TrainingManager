using System.IO;
using System.Threading.Tasks;
using TrainingManager.Model.Interfaces;
using Xamarin.Essentials;

namespace TrainingManager.Model.Services
{
    public class MediaService : IMediaService
    {
        public async Task<byte[]> SelectPhotoAsync()
        {
            var photo = await MediaPicker.PickPhotoAsync();
            var photoArray = await LoadPhotoAsync(photo);
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
