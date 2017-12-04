using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyFavoriteWeb.Views
{
    public sealed partial class ConfiguracaoView : Page
    {
        public ConfiguracaoView()
        {
            this.InitializeComponent();
        }

        private void Light_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["appTema"] = "Light";
            Alerta("Light");
        }

        private void Dark_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["appTema"] = "Dark";
            Alerta("Dark");
        }

        private static void Alerta(string tema)
        {
            MessageDialog showDialog = new MessageDialog($"O tema {tema} foi escolhido. Deseja reiniciar o App?");
            showDialog.Commands.Add(new UICommand("Sim")
            {
                Id = 1
            });
            showDialog.Commands.Add(new UICommand("Não")
            {
                Id = 0
            });

            showDialog.DefaultCommandIndex = 0;
            var result = showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                //do your task  
            }
            else
            {
                //
            }
        }
    }
}
