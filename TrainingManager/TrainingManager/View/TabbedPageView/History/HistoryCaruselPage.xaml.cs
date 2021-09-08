using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.TabbedPageView.History
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryCaruselPage : CarouselPage
    {
        public HistoryCaruselPage()
        {
            InitializeComponent();
        }
    }
}