using System.Linq;
using Windows.UI.Xaml;
using MyFavoriteWeb.Models;
using MyFavoriteWeb.Services;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.Models.Singletons;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyAppContext())
            {
                var usuario = context.Usuarios.Where(m => m.Email == txtEmail.Text && m.Senha == txtSenha.Password).FirstOrDefault();

                if (usuario == null)
                    NotificationService.ShowToastNotification("Erro", "Email e/ou Senha incorreto(s).");
                else
                {
                    new UsuarioLogado(usuario);
                    NavigationService.Navigate<MainPage>();
                }
            }
        }
    }
}
