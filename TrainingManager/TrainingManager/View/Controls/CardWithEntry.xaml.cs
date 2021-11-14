using System.Text;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Globalization;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardWithEntry : ContentView
    {
        public CardWithEntry()
        {
            InitializeComponent();
        }

        public string CardMainText
        {
            get { return (string)GetValue(CardMainTextProperty); }
            set { SetValue(CardMainTextProperty, value); }
        }

        public static readonly BindableProperty CardMainTextProperty =
            BindableProperty.Create("CardMainText", typeof(string), typeof(CardWithEntry), string.Empty);

        public string CardEntryString
        {
            get { return (string)GetValue(CardEntryStringProperty); }
            set { SetValue(CardEntryStringProperty, value); }
        }

        public static readonly BindableProperty CardEntryStringProperty =
            BindableProperty.Create("CardEntryString", typeof(string), typeof(CardWithEntry), "0");

        public double CardEntry
        {
            get { return (double)GetValue(CardEntryProperty); }
            set { SetValue(CardEntryProperty, value); }
        }

        public static readonly BindableProperty CardEntryProperty =
            BindableProperty.Create("CardEntry", typeof(double), typeof(CardWithEntry));

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(CardEntryString))
            {
                CardEntry = 0;
                return;
            }

            var replacedString = CardEntryString.Replace(',', '.');
            var valueStringBuidler = new StringBuilder(replacedString);

            for (int i = valueStringBuidler.Length - 1; i >= 0; --i)
            {
                if (!char.IsDigit(valueStringBuidler[i]) && (valueStringBuidler.ToString().Count(x => x == '.') > 1 || valueStringBuidler[i] != '.'))
                    valueStringBuidler.Remove(i, 1);
            }

            CardEntryString = valueStringBuidler.ToString();
            CardEntry = double.Parse(CardEntryString, CultureInfo.InvariantCulture);
        }
    }
}