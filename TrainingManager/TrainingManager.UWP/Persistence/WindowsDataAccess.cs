using System;
using TrainingManager.Model.Interfaces;
using TrainingManager.UWP.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(WindowsDataAccess))]
namespace TrainingManager.UWP.Persistence
{
    class WindowsDataAccess : IDataAcess
    {
        public string GetExternalStorage() => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }
}
