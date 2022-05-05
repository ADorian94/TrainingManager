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
            //ChartCollection.ChildAdded += ChartCollection_ChildAdded;
        }

        //private void ChartCollection_ChildAdded(object sender, ElementEventArgs e)
        //{
        //    OneRepetitionMaximumVM viewModel = BindingContext as OneRepetitionMaximumVM;
        //    var chart = viewModel.GroupedWorkouts.Last();
        //    ChartCollection.ScrollTo(chart);
        //}
    }
}