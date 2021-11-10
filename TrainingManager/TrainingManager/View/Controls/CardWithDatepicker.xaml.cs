using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardWithDatepicker : ContentView
    {
        public CardWithDatepicker()
        {
            InitializeComponent();
        }

        public string CardMainText
        {
            get { return (string)GetValue(CardMainTextProperty); }
            set { SetValue(CardMainTextProperty, value); }
        }

        public static readonly BindableProperty CardMainTextProperty =
            BindableProperty.Create("CardMainText", typeof(string), typeof(CardWithDatepicker), string.Empty);

        public DateTime CardDate
        {
            get { return (DateTime)GetValue(CardDateProperty); }
            set { SetValue(CardDateProperty, value); }
        }

        public static readonly BindableProperty CardDateProperty =
            BindableProperty.Create("CardDate", typeof(DateTime), typeof(CardWithDatepicker), DateTime.Now);
    }
}