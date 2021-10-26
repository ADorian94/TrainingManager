using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls.Overlays
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainOverlay : BoxView
    {
        public MainOverlay()
        {
            InitializeComponent();
        }
    }
}