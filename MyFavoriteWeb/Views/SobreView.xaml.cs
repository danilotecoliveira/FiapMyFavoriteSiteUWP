using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.ViewModels;

namespace MyFavoriteWeb.Views
{
    public sealed partial class SobreView : Page
    {
        public SobreViewModel ViewModel { get; } = new SobreViewModel();

        public SobreView()
        {
            InitializeComponent();
            Loaded += Login_Loaded;
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
