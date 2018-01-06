using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.ViewModels;

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
            ViewModel.Initialize(imgAvatar);
        }
    }
}
