using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.Data;
using TrainingManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingManager.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LowCardWithLabels : ContentView
    {
        public LowCardWithLabels()
        {
            InitializeComponent();
        }

        //BINDABEL PROPERTIES
        public string CardIndex
        {
            get { return (string)GetValue(CardIndexProperty); }
            set { SetValue(CardIndexProperty, value); }
        }

        public static readonly BindableProperty CardIndexProperty =
            BindableProperty.Create("CardIndex", typeof(string), typeof(LowCardWithLabels), string.Empty);

        public bool IsIndexVisible
        {
            get { return (bool)GetValue(IsIndexVisibleProperty); }
            set { SetValue(IsIndexVisibleProperty, value); }
        }

        public static readonly BindableProperty IsIndexVisibleProperty =
            BindableProperty.Create("IsIndexVisible", typeof(bool), typeof(LowCardWithEntries), false);

        public string CardMainText
        {
            get { return (string)GetValue(CardMainTextProperty); }
            set { SetValue(CardMainTextProperty, value); }
        }

        public static readonly BindableProperty CardMainTextProperty =
            BindableProperty.Create("CardMainText", typeof(string), typeof(LowCardWithLabels), string.Empty);

        public double CardMainEntry
        {
            get { return (double)GetValue(CardMainEntryProperty); }
            set { SetValue(CardMainEntryProperty, value); }
        }

        public static readonly BindableProperty CardMainEntryProperty =
            BindableProperty.Create("CardMainEntry", typeof(double), typeof(LowCardWithLabels), 0.0);

        public string CardSecondaryText
        {
            get { return (string)GetValue(CardSecondaryTextProperty); }
            set { SetValue(CardSecondaryTextProperty, value); }
        }

        public static readonly BindableProperty CardSecondaryTextProperty =
            BindableProperty.Create("CardSecondaryText", typeof(string), typeof(LowCardWithLabels), string.Empty);

        public double CardSecondaryEntry
        {
            get { return (double)GetValue(CardSecondaryEntryProperty); }
            set { SetValue(CardSecondaryEntryProperty, value); }
        }

        public static readonly BindableProperty CardSecondaryEntryProperty =
            BindableProperty.Create("CardSecondaryEntry", typeof(double), typeof(LowCardWithLabels), 0.0);

        public MaterialColors CardColor
        {
            get { return (MaterialColors)GetValue(CardColorProperty); }
            set { SetValue(CardColorProperty, value); }
        }

        public static readonly BindableProperty CardColorProperty =
            BindableProperty.Create("CardColor", typeof(MaterialColors), typeof(LowCardWithLabels), MaterialColors.Default);

        public DelegateCommand ButtonCommand
        {
            get { return (DelegateCommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public static readonly BindableProperty ButtonCommandProperty =
            BindableProperty.Create("ButtonCommand", typeof(DelegateCommand), typeof(LowCardWithLabels), defaultValue: default(DelegateCommand), propertyChanged: OnTapCommandPropertyChanged);

        private static void OnTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LowCardWithLabels headerTemplate && newValue is DelegateCommand command)
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
            BindableProperty.Create("ButtonCommandParameter", typeof(string), typeof(LowCardWithEntries), string.Empty);
        
        public bool IsButtonVisible
        {
            get { return (bool)GetValue(IsButtonVisibleProperty); }
            set { SetValue(IsButtonVisibleProperty, value); }
        }

        public static readonly BindableProperty IsButtonVisibleProperty =
            BindableProperty.Create("IsButtonVisibles", typeof(bool), typeof(LowCardWithEntries), false);
    }
}
