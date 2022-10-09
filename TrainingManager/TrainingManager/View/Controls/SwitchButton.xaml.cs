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
    public partial class SwitchButton : ContentView
    {
        public SwitchButton()
        {
            InitializeComponent();
            OffButton.AnchorX = 1;
            OnButton.AnchorX = 0;
           
            if (IsButtonSwitched)
            {
                OffButton.ScaleX = 0;
            }
            else
            {
                OnButton.ScaleX = 0;
            }
        }

        public bool IsButtonSwitched
        {
            get { return (bool)GetValue(IsButtonSwitchedProperty); }
            set { SetValue(IsButtonSwitchedProperty, value); }
        }

        public static readonly BindableProperty IsButtonSwitchedProperty =
            BindableProperty.Create("IsButtonSwitched", typeof(bool), typeof(SwitchButton), false);

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if(IsButtonSwitched)
            {
                AnimateOff();
            }
            else
            {
                AnimateOn();
            }

            IsButtonSwitched = !IsButtonSwitched;
        }

        private async Task AnimateOn()
        {
            OffButton.CornerRadius = 0;
            OnButton.CornerRadius = 0;
            var animation = new Task[]
{
                OffButton.ScaleXTo(0, 400),
                OnButton.ScaleXTo(1, 300),
            };

            await Task.WhenAll(animation);
            OffButton.CornerRadius = 10;
            OnButton.CornerRadius = 10;
        }

        private async Task AnimateOff()
        {
            OnButton.CornerRadius = 0;
            OffButton.CornerRadius = 0;
            var animation = new Task[]
            {
                OffButton.ScaleXTo(1, 300),
                OnButton.ScaleXTo(0, 400),
            };

            await Task.WhenAll(animation);
            OnButton.CornerRadius = 10;
            OffButton.CornerRadius = 10;
        }
    }
}