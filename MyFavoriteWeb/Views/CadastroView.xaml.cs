using System;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.Media.Capture;
using MyFavoriteWeb.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using MyFavoriteWeb.Models;
using Windows.UI.Notifications;

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
            CameraCaptureUI captureUI = new CameraCaptureUI();
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
            Image img = sender as Image;
            BitmapImage fallbackImage = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.png"));
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

                ShowToastNotification("Sucesso", "O cadastro foi realizado com sucesso.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ShowToastNotification(string title, string stringContent)
        {
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(stringContent));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
        }
    }
}
