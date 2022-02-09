using System;
using System.IO;
using System.Threading.Tasks;
using TrainingManager.Model.Interfaces;
using TrainingManager.Model.LogWriter;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TrainingManager.Model.Services
{
    public class ProfileService : IProfileService
    {
        //FIELDS
        private const string PROFILE_PICTURE = "ProfilePicture";

        public async Task<byte[]> LoadProfilePictureAsync()
        {
            try
            {
                string pathOfProfilePicture = Path.Combine(DependencyService.Get<IDataAcess>().GetExternalStorage(), "media", PROFILE_PICTURE);

                using (FileStream photo = new FileStream(pathOfProfilePicture, FileMode.Open))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                LogHandler.Instance.Nlog.Info($"Exception while reading user credentials: {ex.Message}");
                throw ex;
            }
        }

        public async Task StoreProfilePictureAsync(byte[] image)
        {
            try
            {
                string pathOfMediaDirectory = Path.Combine(DependencyService.Get<IDataAcess>().GetExternalStorage(), "media");

                if (!Directory.Exists(pathOfMediaDirectory))
                    Directory.CreateDirectory(pathOfMediaDirectory);

                if (File.Exists(Path.Combine(pathOfMediaDirectory, PROFILE_PICTURE)))
                    File.Delete(Path.Combine(pathOfMediaDirectory, PROFILE_PICTURE));

                using (Stream stream = new MemoryStream(image))
                using (var newStream = File.OpenWrite(Path.Combine(pathOfMediaDirectory, PROFILE_PICTURE)))
                    await stream.CopyToAsync(newStream);

                LogHandler.Instance.Nlog.Info("Profile picture stored.");
            }
            catch (Exception ex)
            {
                LogHandler.Instance.Nlog.Error($"Exception while writeing user profile pocture: {ex.Message}");
                throw ex;
            }
        }

        public bool IsProfilePictureStored() => File.Exists(Path.Combine(DependencyService.Get<IDataAcess>().GetExternalStorage(), "media", PROFILE_PICTURE));
    }
}
