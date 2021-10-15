using TrainingManager.Model;
using TrainingManager.Model.Services;
using TrainingManager.ViewModel.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TrainingManager
{
    public partial class App : Application
    {
        //FIELDS
        private PageNavigationManager _pageNavigationManager;
        private IApiServices _apiService;

        public App()
        {
            InitializeComponent();
            _apiService = new ApiServices("http://localhost:51426");
            //_apiService = new ApiServices("http://192.168.56.1:51426"); //phone debug
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
