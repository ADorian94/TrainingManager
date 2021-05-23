using System;
using System.Threading.Tasks;
using TrainingManager.Model.Services;
using TrainingManager.ViewModel.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TrainingManager
{
    public partial class App : Application
    {
        private PageNavigationManager _pageNavigationManager;
        private ApiServices _apiService;

        public App()
        {
            InitializeComponent();
            _apiService = new ApiServices("http://localhost:51426");
            _pageNavigationManager = new PageNavigationManager(_apiService);
            MainPage = _pageNavigationManager.GetMainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        //this is a test change
        //second test change
    }
}
