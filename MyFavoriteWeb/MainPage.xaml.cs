using MyFavoriteWeb.Services;
using MyFavoriteWeb.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyFavoriteWeb
{
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel { get; } = new MainPageViewModel();

        public MainPage()
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
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void ListaSites_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<Views.ListaSitesView>();
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void Cadastro_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<Views.CadastroView>();
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void Sobre_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<Views.SobreView>();
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void Configuracao_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<Views.ConfiguracaoView>();
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Frame = Window.Current.Content as Frame;
            NavigationService.Navigate(typeof(Login));
        }

        private void btnHamburger_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }
    }
}
