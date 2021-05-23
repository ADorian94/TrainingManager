using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationTabbedPage : TabbedPage
    {
        public NavigationTabbedPage()
        {
            InitializeComponent();
        }
    }
}