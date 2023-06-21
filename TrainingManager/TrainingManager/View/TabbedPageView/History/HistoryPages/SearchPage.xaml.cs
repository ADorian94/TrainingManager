using System.Collections;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.TabbedPageView.History.HistoryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void OnScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (Device.RuntimePlatform != Device.UWP)
                return;

            if (sender is CollectionView cv)
            {
                var count = (cv.ItemsSource as ICollection).Count;

                if (e.LastVisibleItemIndex + 1 - count + cv.RemainingItemsThreshold >= 0)
                {
                    if (cv.RemainingItemsThresholdReachedCommand.CanExecute(null))
                        cv.RemainingItemsThresholdReachedCommand.Execute(null);
                }
            }

        }
    }
}