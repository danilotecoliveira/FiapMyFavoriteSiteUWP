using Windows.UI.Xaml;
using MyFavoriteWeb.Services;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.Models;
using System.Linq;
using MyFavoriteWeb.Models.Singletons;

namespace MyFavoriteWeb
{
    public sealed partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
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

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<Views.CadastroView>();
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<Views.SobreView>();
        }
    }
}
