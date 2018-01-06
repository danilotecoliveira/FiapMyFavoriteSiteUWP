using System;
using System.Linq;
using Windows.Storage;
using Windows.Foundation;
using MyFavoriteWeb.Models;
using Windows.UI.Xaml.Media;
using Windows.Media.Capture;
using MyFavoriteWeb.Services;
using Windows.UI.Xaml.Shapes;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using MyFavoriteWeb.Models.Singletons;
using System.Runtime.InteropServices.WindowsRuntime;

namespace MyFavoriteWeb.ViewModels
{
    public class CadastroViewModel : BaseModel
    {
        private StorageFile storageFile;
        private string email;
        private string senha;
        private string nome;
        private string avatar;

        public void Initialize(Ellipse imgAvatar)
        {
            if (!string.IsNullOrWhiteSpace(UsuarioLogado.Nome))
            {
                var diretorio = KnownFolders.PicturesLibrary;
                Nome = UsuarioLogado.Nome;
                Email = UsuarioLogado.Email;
                Senha = UsuarioLogado.Senha;
                Avatar = diretorio.DisplayName + "/" + UsuarioLogado.Avatar;
            }

            Ellipse = imgAvatar;
        }

        public Ellipse Ellipse { get; set; }

        public string Email
        {
            get { return email; }
            set { Set(ref email, value); }
        }

        public string Senha
        {
            get { return senha; }
            set { Set(ref senha, value); }
        }

        public string Nome
        {
            get { return nome; }
            set { Set(ref nome, value); }
        }

        public string Avatar
        {
            get { return (string.IsNullOrWhiteSpace(avatar)) ? "ms-appx:///Assets/profile.png" :  avatar; }
            set { Set(ref avatar, value); }
        }

        public void Cancelar()
        {
            NavigationService.GoBack();
        }

        public void Salvar()
        {
            try
            {
                VerificarUsuarioExistente(email);

                using (var context = new MyAppContext())
                {
                    var usuario = new Usuario
                    {
                        Nome = nome,
                        Email = email,
                        Senha = senha,
                        Avatar = avatar
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

        public async void AbrirCamera()
        {
            var captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.AllowCropping = true;
            captureUI.PhotoSettings.CroppedAspectRatio = new Size(300, 300);
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;

            storageFile = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

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

                    ImageBrush ib = new ImageBrush();
                    ib.ImageSource = bitmap;
                    Ellipse.Fill = ib;
                }
            }

            GuardarFoto();
        }

        private async void GuardarFoto()
        {
            try
            {
                var piclib = KnownFolders.PicturesLibrary;
                RenderTargetBitmap renderbmp = new RenderTargetBitmap();
                await renderbmp.RenderAsync(Ellipse);
                var pixels = await renderbmp.GetPixelsAsync();
                var nomeAvatar = $"{Guid.NewGuid()}.bmp";
                avatar = nomeAvatar;
                var file = await piclib.CreateFileAsync(nomeAvatar);
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream);
                    var bytes = pixels.ToArray();
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, 140, 140, 0, 0, bytes);
                    await encoder.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                //
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
