using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamForms.Controls;

namespace TrainingManager.View.Controls.Calendar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeightCalendar : ContentView
    {
        public WeightCalendar()
        {
            InitializeComponent();
        }

        public ObservableCollection<SpecialDate> Dates
        {
            get { return (ObservableCollection<SpecialDate>)GetValue(DatesProperty); }
            set { SetValue(DatesProperty, value); }
        }

        public static readonly BindableProperty DatesProperty =
            BindableProperty.Create("Dates", typeof(ObservableCollection<SpecialDate>), typeof(WeightCalendar), null);

        public DelegateCommand DateSelectedCommand
        {
            get { return (DelegateCommand)GetValue(DateSelectedCommandProperty); }
            set { SetValue(DateSelectedCommandProperty, value); }
        }

        public static readonly BindableProperty DateSelectedCommandProperty =
            BindableProperty.Create("DateSelectedCommand", typeof(DelegateCommand), typeof(WeightCalendar), defaultValue: default(DelegateCommand), propertyChanged: OnFrameTapCommandPropertyChanged);

        private static void OnFrameTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WeightCalendar headerTemplate && newValue is DelegateCommand command)
                headerTemplate.DateSelectedCommand = command;
        }

        //public string DateCommandParameter
        //{
        //    get { return (string)GetValue(DateCommandParameterProperty); }
        //    set { SetValue(DateCommandParameterProperty, value); }
        //}

        //public static readonly BindableProperty DateCommandParameterProperty =
        //    BindableProperty.Create("DateCommandParameter", typeof(string), typeof(WeightCalendar), string.Empty);

        private void Calendar_DateClicked(object sender, DateTimeEventArgs e)
        {
            DateSelectedCommand?.Execute(e);
        }
    }
}