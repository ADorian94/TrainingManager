using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.MasterDetailNavigationPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailNavigationPageMaster : ContentPage
    {
        public ListView ListView;

        public MasterDetailNavigationPageMaster()
        {
            InitializeComponent();
            ListView = MenuItemsListView;
        }
    }
}