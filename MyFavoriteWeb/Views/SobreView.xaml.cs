using Windows.UI.Xaml;
using MyFavoriteWeb.Services;
using Windows.UI.Xaml.Controls;

namespace MyFavoriteWeb.Views
{
    public sealed partial class SobreView : Page
    {
        public SobreView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<MainPage>();
        }
    }
}
