using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.MasterDetailNavigationPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailNavigationPage : MasterDetailPage
    {
        public EventHandler DetailPageSelected;

        public MasterDetailNavigationPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DetailPageSelected?.Invoke(this, e);
            MasterPage.ListView.SelectedItem = null;
        }
    }
}