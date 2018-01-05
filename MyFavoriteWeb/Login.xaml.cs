using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.ViewModels;

namespace MyFavoriteWeb
{
    public sealed partial class Login : Page
    {
        public LoginViewModel ViewModel { get; } = new LoginViewModel();

        public Login()
        {
            InitializeComponent();
            Loaded += Login_Loaded;
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
