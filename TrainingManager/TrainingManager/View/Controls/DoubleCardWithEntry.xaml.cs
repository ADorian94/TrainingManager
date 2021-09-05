using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DoubleCardWithEntry : ContentView
    {
        public DoubleCardWithEntry()
        {
            InitializeComponent();
        }

        //FIRST CARD
        public string FirstCardMainText
        {
            get { return (string)GetValue(FirstCardMainTextProperty); }
            set { SetValue(FirstCardMainTextProperty, value); }
        }

        public static readonly BindableProperty FirstCardMainTextProperty =
            BindableProperty.Create("FirstCardMainText", typeof(string), typeof(DoubleCardWithEntry), string.Empty);

        public double FirstCardEntry
        {
            get { return (double)GetValue(FirstCardEntryProperty); }
            set { SetValue(FirstCardEntryProperty, value); }
        }

        public static readonly BindableProperty FirstCardEntryProperty =
            BindableProperty.Create("FirstCardEntry", typeof(double), typeof(DoubleCardWithEntry), 0.0);

        //SECOND CARD
        public string SecondCardMainText
        {
            get { return (string)GetValue(SecondCardMainTextProperty); }
            set { SetValue(SecondCardMainTextProperty, value); }
        }

        public static readonly BindableProperty SecondCardMainTextProperty =
            BindableProperty.Create("SecondCardMainText", typeof(string), typeof(DoubleCardWithEntry), string.Empty);

        public double SecondCardEntry
        {
            get { return (double)GetValue(SecondCardEntryProperty); }
            set { SetValue(SecondCardEntryProperty, value); }
        }

        public static readonly BindableProperty SecondCardEntryProperty =
            BindableProperty.Create("SecondCardEntry", typeof(double), typeof(DoubleCardWithEntry), 0.0);

        //COMMNAD
        public DelegateCommand CardFrameTapped
        {
            get { return (DelegateCommand)GetValue(CardFrameTappedProperty); }
            set { SetValue(CardFrameTappedProperty, value); }
        }

        public static readonly BindableProperty CardFrameTappedProperty =
            BindableProperty.Create("CardFrameTapped", typeof(DelegateCommand), typeof(DoubleCardWithEntry), defaultValue: default(DelegateCommand), propertyChanged: OnFrameTapCommandPropertyChanged);

        private static void OnFrameTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SimpleCard headerTemplate && newValue is DelegateCommand command)
                headerTemplate.CardFrameTapped = command;
        }

        public string TapCommandParameter
        {
            get { return (string)GetValue(TapCommandParameterProperty); }
            set { SetValue(TapCommandParameterProperty, value); }
        }

        public static readonly BindableProperty TapCommandParameterProperty =
            BindableProperty.Create("TapCommandParameter", typeof(string), typeof(DoubleCardWithEntry), string.Empty);
    }
}