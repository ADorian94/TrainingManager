using Microcharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinearChart : ContentView
    {
        public LinearChart()
        {
            InitializeComponent();
            LinearChartItem.WidthRequest = Application.Current.MainPage.Width * 0.90;
        }

        public string CardMainText
        {
            get { return (string)GetValue(CardMainTextProperty); }
            set { SetValue(CardMainTextProperty, value); }
        }

        public static readonly BindableProperty CardMainTextProperty =
            BindableProperty.Create("CardMainText", typeof(string), typeof(CardWithEntry), string.Empty);

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

                if (newEntries != null && newEntries.Count > 0)
                    headerTemplate.CardMainText = newEntries.First().date.ToString("MMMM");

                headerTemplate.LinearChartItem.Chart = new LineChart()
                {
                    Entries = newEntries.Select(x => new ChartEntry((float)x.weight)
                    {
                        Label = x.date.ToString("dd"),
                        Color = x.weight < 15000 ? blueColor : redColor,
                        ValueLabel = (x.weight / 1000).ToString("0.##"),
                        ValueLabelColor = x.weight < 15000 ? blueColor : redColor,
                    }),
                    ValueLabelOrientation = Orientation.Vertical,
                    LabelTextSize = GetSizeByDevice(),
                    LabelOrientation = GetLabelOrientation(),
                    LineMode = LineMode.Spline,
                    LabelColor = redColor,
                    IsAnimated = true,
                    BackgroundColor = SkiaSharp.SKColor.Empty,
                };
            }
        }

        private static Orientation GetLabelOrientation()
        {
            switch (Device.Idiom)
            {
                case TargetIdiom.Phone:
                    return Orientation.Vertical;
                case TargetIdiom.Desktop:
                default:
                    return Orientation.Horizontal;
            }
        }

        private static float GetSizeByDevice()
        {
            switch (Device.Idiom)
            {
                case TargetIdiom.Phone:
                    return 40;
                case TargetIdiom.Desktop:
                default:
                    return 20;
            }
        }
    }
}