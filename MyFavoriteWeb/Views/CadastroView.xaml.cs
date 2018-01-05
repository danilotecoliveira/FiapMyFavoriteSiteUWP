using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.ViewModels;
using Windows.UI.Xaml.Media.Imaging;

namespace MyFavoriteWeb.Views
{
    public sealed partial class CadastroView : Page
    {
        public CadastroViewModel ViewModel { get; } = new CadastroViewModel();

        public CadastroView()
        {
            InitializeComponent();
            Loaded += Login_Loaded;
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }

        private void imgFoto_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var img = sender as Image;
            var fallbackImage = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.png"));
            img.Width = 100;
            img.Source = fallbackImage;
        }
    }
}
