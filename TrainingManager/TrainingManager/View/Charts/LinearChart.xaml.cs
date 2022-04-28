using Microcharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinearChart : ContentView
    {
        public LinearChart()
        {
            InitializeComponent();
        }

        public ObservableCollection<(DateTime date, double weight)> ChartEntries
        {
            get { return (ObservableCollection<(DateTime date, double weight)>)GetValue(ChartEntriesProperty); }
            set { SetValue(ChartEntriesProperty, value); }
        }

        public static readonly BindableProperty ChartEntriesProperty =
            BindableProperty.Create("ChartEntries", typeof(ObservableCollection<(DateTime date, double weight)>), typeof(LinearChart), null, propertyChanged: OnChartItemsChanged);

        private static void OnChartItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LinearChart headerTemplate && newValue is ObservableCollection<(DateTime date, double weight)> newEntries)
            {
                SkiaSharp.SKColor.TryParse("03A9F9", out SkiaSharp.SKColor blueColor);
                SkiaSharp.SKColor.TryParse("ff6961", out SkiaSharp.SKColor redColor);

                headerTemplate.LinearChartItem.Chart = new LineChart()
                {
                    Entries = newEntries.Select(x => new ChartEntry((float)x.weight)
                    {
                        Label = x.date.ToString("dd"),
                        Color = x.date < DateTime.Now.ToUniversalTime() ? blueColor : redColor,
                        ValueLabel = x.weight.ToString()
                    }),
                    ValueLabelOrientation = Orientation.Horizontal,
                    LabelOrientation = Orientation.Horizontal,
                    LabelColor = redColor,
                    IsAnimated = true,
                    BackgroundColor = SkiaSharp.SKColor.Empty,
                };
            }
        }
    }
}