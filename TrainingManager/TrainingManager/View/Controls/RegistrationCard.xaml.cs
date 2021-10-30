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
    public partial class RegistrationCard : ContentView
    {
        public RegistrationCard()
        {
            InitializeComponent();
        }

        //BINDABEL PROPERTIES
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly BindableProperty NameProperty =
            BindableProperty.Create("Name", typeof(string), typeof(RegistrationCard), string.Empty);

        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        public static readonly BindableProperty UserNameProperty =
            BindableProperty.Create("UserName", typeof(string), typeof(RegistrationCard), string.Empty);

        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        public static readonly BindableProperty EmailProperty =
            BindableProperty.Create("Email", typeof(string), typeof(RegistrationCard), string.Empty);

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly BindableProperty PasswordProperty =
            BindableProperty.Create("Password", typeof(string), typeof(RegistrationCard), string.Empty);

        public string ConfirmPassword
        {
            get { return (string)GetValue(ConfirmPasswordProperty); }
            set { SetValue(ConfirmPasswordProperty, value); }
        }

        public static readonly BindableProperty ConfirmPasswordProperty =
            BindableProperty.Create("ConfirmPassword", typeof(string), typeof(RegistrationCard), string.Empty);
    }
}