using System;
using System.Linq;
using Windows.Foundation;
using MyFavoriteWeb.Models;
using Windows.Media.Capture;
using MyFavoriteWeb.Services;
using Windows.UI.Xaml.Media.Imaging;
using MyFavoriteWeb.Models.Singletons;

namespace MyFavoriteWeb.ViewModels
{
    public class CadastroViewModel : BaseModel
    {
        private string email;
        private string senha;
        private string nome;

        public void Initialize()
        {
            if (!string.IsNullOrWhiteSpace(UsuarioLogado.Nome))
            {
                Nome = UsuarioLogado.Nome;
                Email = UsuarioLogado.Email;
                Senha = UsuarioLogado.Senha;
            }
        }

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
                        Senha = senha
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

        public void AbrirCamera()
        {
            var captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.AllowCropping = true;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(300, 300);
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;

            var storageFile = captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo).GetResults();

            if (storageFile == null)
            {
                //operação cancelada
            }
            else
            {
                using (var stream = storageFile.OpenReadAsync().GetResults())
                {
                    var bitmap = new BitmapImage();
                    bitmap.SetSourceAsync(stream).GetResults();
                    //imgAvatar.Source = bitmap;
                }
            }
        }
    }
}
