using System.Linq;
using System.Threading;
using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OneRepetitionMaximumCalculatorPage : ContentPage
    {
        public OneRepetitionMaximumCalculatorPage()
        {
            InitializeComponent();
            ChartCollection.ChildAdded += ChartCollection_ChildAdded;
            Appearing += OneRepetitionMaximumCalculatorPage_Appearing;
        }

        private void OneRepetitionMaximumCalculatorPage_Appearing(object sender, System.EventArgs e)
        {
            OneRepetitionMaximumVM viewModel = BindingContext as OneRepetitionMaximumVM;
            ChartCollection.ItemsSource = viewModel.GroupedWorkouts;
            //ChartCollection.ScrollTo(viewModel.GroupedWorkouts.Last());
        }

        private void ChartCollection_ChildAdded(object sender, ElementEventArgs e)
        {
            OneRepetitionMaximumVM viewModel = BindingContext as OneRepetitionMaximumVM;
            var chart = viewModel.GroupedWorkouts.Last();
            ChartCollection.ScrollTo(chart);
        }
    }
}