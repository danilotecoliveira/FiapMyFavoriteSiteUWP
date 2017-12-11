using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.Foundation;
using MyFavoriteWeb.Models;
using Windows.Media.Capture;
using MyFavoriteWeb.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using MyFavoriteWeb.Models.Singletons;

namespace MyFavoriteWeb.Views
{
    public sealed partial class CadastroView : Page
    {
        public CadastroView()
        {
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(UsuarioLogado.Nome))
            {
                Nome.Text = UsuarioLogado.Nome;
                Email.Text = UsuarioLogado.Email;
                Senha.Password = UsuarioLogado.Senha;
            }
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

        protected void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                VerificarUsuarioExistente(Email.Text);

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

                NavigationService.Navigate<Login>();
            }
            catch (Exception ex)
            {
                NotificationService.ShowToastNotification("Erro", ex.Message);
            }
        }

        private void VerificarUsuarioExistente(string email)
        {
            using (var context = new MyAppContext())
            {
                var usuario = context.Usuarios.Where(m => m.Email == email).FirstOrDefault();

                if (usuario != null)
                {
                    context.Usuarios.Remove(usuario);
                    context.SaveChanges();
                }
            }
        }
    }
}
