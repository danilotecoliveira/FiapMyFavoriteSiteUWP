using System.Linq;
using MyFavoriteWeb.Models;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.Models.Singletons;
using MyFavoriteWeb.Services;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;

namespace MyFavoriteWeb.Views
{
    public sealed partial class ListaSitesView : Page
    {
        public ListaSitesView()
        {
            InitializeComponent();

            using (var context = new MyAppContext())
            {
                var lista = context.Sites.ToList();

                listaSites.ItemsSource = lista;
            }
        }

        private void listaSites_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var listView = (ListView)sender;
            var url = ((FrameworkElement)e.OriginalSource).DataContext as Site;

            WebViewUrl.Url = url.Url;
            NavigationService.Navigate<NavegadorWebView>();
        }
    }
}
