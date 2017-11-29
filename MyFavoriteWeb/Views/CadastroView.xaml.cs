using System;
using Windows.UI.Xaml;
using Windows.Foundation;
using MyFavoriteWeb.Models;
using Windows.Media.Capture;
using MyFavoriteWeb.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MyFavoriteWeb.Views
{
    public sealed partial class CadastroView : Page
    {
        public CadastroView()
        {
            InitializeComponent();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void OpenCamera(object sender, RoutedEventArgs e)
        {
            var captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.AllowCropping = true;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(300, 300);
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;

            var storageFile = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (storageFile == null)
            {
                //operação cancelada
            }
            else
            {
                using (var stream = await storageFile.OpenReadAsync())
                {
                    var bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(stream);
                    imgAvatar.Source = bitmap;
                }
            }
        }

        private void imgFoto_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var img = sender as Image;
            var fallbackImage = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.png"));
            img.Width = 100;
            img.Source = fallbackImage;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new MyAppContext())
                {
                    var usuario = new Usuario
                    {
                        Nome = Nome.Text,
                        Email = Email.Text,
                        Senha = Senha.Password
                    };

                    context.Usuarios.Add(usuario);
                    context.SaveChanges();
                }

                NotificationService.ShowToastNotification("Sucesso", "O cadastro foi realizado com sucesso.");
            }
            catch (Exception ex)
            {
                NotificationService.ShowToastNotification("Erro", ex.Message);
            }
        }

        
    }
}
