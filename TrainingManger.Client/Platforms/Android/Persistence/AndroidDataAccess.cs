using Android.Content;
using TrainingManager.Droid.Persistence;
using TrainingManager.Model.Interfaces;

[assembly: Dependency(typeof(AndroidDataAccess))]
namespace TrainingManager.Droid.Persistence
{
    public class AndroidDataAccess : IDataAcess
    {
        public string GetExternalStorage()
        {
            Context context = Android.App.Application.Context;
            var filePath = context.GetExternalFilesDir("");
            return filePath.Path;
        }
    }
}
