using Windows.UI.Xaml;
using MyFavoriteWeb.Models;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.ViewModels;

namespace MyFavoriteWeb.Views
{
    public sealed partial class ListaSitesView : Page
    {
        public ListaSitesViewModel ViewModel { get; } = new ListaSitesViewModel();

        public ListaSitesView()
        {
            InitializeComponent();
            Loaded += ListaSitesView_Loaded;
            DataContext = ViewModel;
        }

        private void ListaSitesView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }

        private void listaSites_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var listView = (ListView)sender;
            sitesCadastrados.ShowAt(listView, e.GetPosition(listView));
            var a = ((FrameworkElement)e.OriginalSource).DataContext as Site;
            ViewModel.ItemClicado = a.Id.ToString();
        }
    }
}
