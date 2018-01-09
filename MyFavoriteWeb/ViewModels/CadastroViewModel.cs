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
using Windows.Graphics.Display;
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
                Avatar = PegarAvatar(UsuarioLogado.Avatar);
            }

            Ellipse = imgAvatar;
        }

        private string PegarAvatar(string nomeImagem)
        {
            var myfolder = ApplicationData.Current.LocalFolder;
            return $"{myfolder.Path}/{nomeImagem}";
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

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                NotificationService.ShowToastNotification("Erro", ex.Message);
            }
        }

        public async void AbrirCamera()
        {
            var camera = new CameraCaptureUI();
            camera.PhotoSettings.AllowCropping = true;
            camera.PhotoSettings.CroppedAspectRatio = new Size(300, 300);
            camera.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;

            storageFile = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (storageFile != null)
            {
                using (var stream = await storageFile.OpenReadAsync())
                {
                    var bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(stream);

                    var imageBrush = new ImageBrush { ImageSource = bitmap };
                    Ellipse.Fill = imageBrush;
                }
            }

            GuardarFoto();
        }

        private async void GuardarFoto()
        {
            var biblioteca = ApplicationData.Current.LocalFolder;
            var render = new RenderTargetBitmap();
            await render.RenderAsync(Ellipse);

            var info = DisplayInformation.GetForCurrentView();
            var pixels = await render.GetPixelsAsync();
            var nomeImagem = $"{Guid.NewGuid()}.bmp";
            avatar = nomeImagem;
            var arquivo = await biblioteca.CreateFileAsync(nomeImagem, CreationCollisionOption.GenerateUniqueName);

            using (var stream = await arquivo.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream);
                var bytes = pixels.ToArray();
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)render.PixelWidth, (uint)render.PixelHeight,
                    info.LogicalDpi,
                    info.LogicalDpi, bytes);
                await encoder.FlushAsync();
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
