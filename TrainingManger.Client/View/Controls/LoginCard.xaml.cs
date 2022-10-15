namespace TrainingManager.View.Controls
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginCard : ContentView
    {
        public LoginCard()
        {
            InitializeComponent();
        }

        //BINDABEL PROPERTIES
        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        public static readonly BindableProperty UserNameProperty =
            BindableProperty.Create("UserName", typeof(string), typeof(LoginCard), string.Empty);

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly BindableProperty PasswordProperty =
            BindableProperty.Create("Password", typeof(string), typeof(LoginCard), string.Empty);
    }
}