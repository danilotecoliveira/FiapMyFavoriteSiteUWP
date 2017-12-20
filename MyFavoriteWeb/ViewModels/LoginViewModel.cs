using MyFavoriteWeb.Services;

namespace MyFavoriteWeb.ViewModels
{
    public class LoginViewModel
    {
        public void Initialize()
        {

        }

        public void NovoCadastro()
        {
            NavigationService.Navigate<Views.CadastroView>();
        }

        public void Sobre()
        {
            NavigationService.Navigate<Views.SobreView>();
        }
    }
}