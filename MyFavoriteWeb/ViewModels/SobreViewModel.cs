using MyFavoriteWeb.Services;

namespace MyFavoriteWeb.ViewModels
{
    public class SobreViewModel
    {
        public void Initialize()
        { }

        public void Voltar()
        {
            NavigationService.GoBack();
        }
    }
}
