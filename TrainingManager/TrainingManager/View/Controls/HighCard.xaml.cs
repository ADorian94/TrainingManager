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
	public partial class HighCard : ContentView
	{
		public HighCard ()
		{
			InitializeComponent ();
		}

        public string MainLabel
        {
            get { return (string)GetValue(MainLabelProperty); }
            set { SetValue(MainLabelProperty, value); }
        }

        public static readonly BindableProperty MainLabelProperty =
            BindableProperty.Create("MainLabel", typeof(string), typeof(HighCard), string.Empty);

        public string FirstLabel
        {
            get { return (string)GetValue(FirstLabelProperty); }
            set { SetValue(FirstLabelProperty, value); }
        }

        public static readonly BindableProperty FirstLabelProperty =
            BindableProperty.Create("FirstLabel", typeof(string), typeof(HighCard), string.Empty);

        public string FirstValue
        {
            get { return (string)GetValue(FirstValueProperty); }
            set { SetValue(FirstValueProperty, value); }
        }

        public static readonly BindableProperty FirstValueProperty =
            BindableProperty.Create("FirstValue", typeof(string), typeof(HighCard), string.Empty);

        public string SecondLabel
        {
            get { return (string)GetValue(SecondLabelProperty); }
            set { SetValue(SecondLabelProperty, value); }
        }

        public static readonly BindableProperty SecondLabelProperty =
            BindableProperty.Create("SecondLabel", typeof(string), typeof(HighCard), string.Empty);

        public string SecondValue
        {
            get { return (string)GetValue(SecondValueProperty); }
            set { SetValue(SecondValueProperty, value); }
        }

        public static readonly BindableProperty SecondValueProperty =
            BindableProperty.Create("SecondValue", typeof(string), typeof(HighCard), string.Empty);

        public DelegateCommand ButtonCommand
        {
            get { return (DelegateCommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public static readonly BindableProperty ButtonCommandProperty =
            BindableProperty.Create("ButtonCommand", typeof(DelegateCommand), typeof(HighCard), defaultValue: default(DelegateCommand), propertyChanged: OnTapCommandPropertyChanged);

        private static void OnTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is OpenableCard headerTemplate && newValue is DelegateCommand command)
            {
                headerTemplate.ButtonCommand = command;
            }
        }

        public string ButtonCommandParameter
        {
            get { return (string)GetValue(ButtonCommandParameterProperty); }
            set { SetValue(ButtonCommandParameterProperty, value); }
        }

        public static readonly BindableProperty ButtonCommandParameterProperty =
            BindableProperty.Create("ButtonCommandParameter", typeof(string), typeof(HighCard), string.Empty);

        public bool ButtonVisibility
        {
            get { return (bool)GetValue(ButtonVisibilityProperty); }
            set { SetValue(ButtonVisibilityProperty, value); }
        }

        public static readonly BindableProperty ButtonVisibilityProperty =
            BindableProperty.Create("ButtonVisibility", typeof(bool), typeof(HighCard), defaultValue: false);

    }
}