using MyFavoriteWeb.Services;
using MyFavoriteWeb.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyFavoriteWeb
{
    public sealed partial class MainPageFirstVersion : Page
    {
        public MainPageViewModel ViewModel { get; } = new MainPageViewModel();

        public MainPageFirstVersion()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            NavigationService.Frame = frameNavigation;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }

        private void WebView_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<Views.NavegadorWebView>();
        }

        private void ListaSites_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<Views.ListaSitesView>();
        }

        private void Cadastro_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<Views.CadastroView>();
        }

        private void Sobre_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<Views.SobreView>();
        }

        private void Configuracao_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<Views.SobreView>();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Frame = Window.Current.Content as Frame;
            NavigationService.Navigate(typeof(Login));
        }
    }
}
