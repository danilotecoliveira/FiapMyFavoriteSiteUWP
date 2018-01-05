using System.Linq;
using MyFavoriteWeb.Models;
using MyFavoriteWeb.Services;
using MyFavoriteWeb.Models.Singletons;

namespace MyFavoriteWeb.ViewModels
{
    public class LoginViewModel : BaseModel
    {
        private string email;
        private string senha;

        public void Initialize()
        { }

        public string Email
        {
            get { return email; }
            set { Set(ref email, value); }
        }
        
        public string Senha
        {
            get { return senha; }
            set { Set(ref senha, value); }
        }

        public void NovoCadastro()
        {
            NavigationService.Navigate<Views.CadastroView>();
        }

        public void Sobre()
        {
            NavigationService.Navigate<Views.SobreView>();
        }

        public void Logar()
        {
            using (var context = new MyAppContext())
            {
                var usuario = context.Usuarios.Where(m => m.Email == email && m.Senha == Senha).FirstOrDefault();

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