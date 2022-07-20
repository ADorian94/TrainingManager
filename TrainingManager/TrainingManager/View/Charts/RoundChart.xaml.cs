using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoundChart : ContentView
	{
		public RoundChart ()
		{
			InitializeComponent ();
            RoundChartItem.HeightRequest = 300;
        }

        public string CardMainText
		{
			get { return (string)GetValue(CardMainTextProperty); }
			set { SetValue(CardMainTextProperty, value); }
		}

		public static readonly BindableProperty CardMainTextProperty =
			BindableProperty.Create("CardMainText", typeof(string), typeof(RoundChart), string.Empty);

		public ObservableCollection<(Muscle muscle, double weight)> ChartEntries
		{
			get { return (ObservableCollection<(Muscle muscle, double weight)>)GetValue(ChartEntriesProperty); }
			set { SetValue(ChartEntriesProperty, value); }
		}

		public static readonly BindableProperty ChartEntriesProperty =
			BindableProperty.Create("ChartEntries", typeof(ObservableCollection<(Muscle muscle, double weight)>), typeof(RoundChart), null, propertyChanged: OnChartItemsChanged);

        private static void OnChartItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RoundChart headerTemplate && newValue is ObservableCollection<(Muscle muscle, double weight)> newEntries)
            {
                headerTemplate.RoundChartItem.Chart = new DonutChart()
                {
                    Entries = newEntries.Select(x => new ChartEntry((float)x.weight)
                    {
                        Label = x.muscle.ToString(),
                        ValueLabel = x.weight.ToString(),
                        Color = GetColorByMuscle(x.muscle),
                        ValueLabelColor = GetColorByMuscle(x.muscle),
                        TextColor = SKColor.Parse("FFF"),
                    }),
                    LabelTextSize = GetSizeByDevice(),
                    IsAnimated = true,
					BackgroundColor = SKColor.Empty,
				};
                
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

        private static SKColor GetColorByMuscle(Muscle m)
        {
            string colorHex;

            switch (m)
            {
                case Muscle.Unknown:
                    colorHex = "#f05545";
                    break;
                case Muscle.Chest:
                    colorHex = "#d05ce3";
                    break;
                case Muscle.Biceps:
                    colorHex = "#8559da";
                    break;
                case Muscle.Triceps:
                    colorHex = "#005CB2";
                    break;
                case Muscle.Forearms:
                    colorHex = "#8559da";
                    break;
                case Muscle.Deltoid:
                    colorHex = "#007C91";
                    break;
                case Muscle.Lats:
                    colorHex = "#4B830D";
                    break;
                case Muscle.Trapeus:
                    colorHex = "#8C9900";
                    break;
                case Muscle.Glute:
                    colorHex = "#c56000";
                    break;
                case Muscle.Hamstrings:
                    colorHex = "#c63f17";
                    break;
                case Muscle.Quadriceps:
                    colorHex = "#5d4037";
                    break;
                case Muscle.Abdominals:
                    colorHex = "#03A9F9";
                    break;
                default:
                    colorHex = "#ff6961";
                    break;
            }

            SKColor.TryParse(colorHex, out SKColor parsedColor);
            return parsedColor;
        }

    }
}