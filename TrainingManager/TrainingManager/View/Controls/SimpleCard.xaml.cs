using System;
using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleCard : ContentView
    {
        public event EventHandler ButtonClickHandler;

        public SimpleCard()
        {
            InitializeComponent();
        }

        public string CardMainText
        {
            get { return (string)GetValue(CardMainTextProperty); }
            set { SetValue(CardMainTextProperty, value); }
        }

        public static readonly BindableProperty CardMainTextProperty =
            BindableProperty.Create("CardMainText", typeof(string), typeof(SimpleCard), string.Empty);

        public string CardSecondaryText
        {
            get { return (string)GetValue(CardSecondaryTextProperty); }
            set { SetValue(CardSecondaryTextProperty, value); }
        }

        public static readonly BindableProperty CardSecondaryTextProperty =
            BindableProperty.Create("CardSecondaryText", typeof(string), typeof(SimpleCard), string.Empty);

        public DelegateCommand CardTappedCommand
        {
            get { return (DelegateCommand)GetValue(CardTappedCommandProperty); }
            set { SetValue(CardTappedCommandProperty, value); }
        }

        public static readonly BindableProperty CardTappedCommandProperty =
            BindableProperty.Create("CardTappedCommand", typeof(DelegateCommand), typeof(SimpleCard), defaultValue: default(DelegateCommand), propertyChanged: OnTapCommandPropertyChanged);

        private static void OnTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SimpleCard headerTemplate && newValue is DelegateCommand command)
            {
                headerTemplate.CardTappedCommand = command;
            }
        }

        public string CardTappedCommandParameter
        {
            get { return (string)GetValue(CardTappedCommandParameterProperty); }
            set { SetValue(CardTappedCommandParameterProperty, value); }
        }

        public static readonly BindableProperty CardTappedCommandParameterProperty =
            BindableProperty.Create("CardTappedCommandParameter", typeof(string), typeof(SimpleCard), string.Empty);

    }
}