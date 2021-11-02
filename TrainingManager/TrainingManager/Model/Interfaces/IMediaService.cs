using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace TrainingManager.Model.Interfaces
{
    public interface IMediaService
    {
        Task<byte[]> SelectPhotoAsync();
    }
}
