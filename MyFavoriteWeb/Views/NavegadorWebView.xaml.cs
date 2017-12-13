using System;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using MyFavoriteWeb.Models;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.Models.Singletons;
using MyFavoriteWeb.Services;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Shapes;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;

namespace MyFavoriteWeb.Views
{
    public sealed partial class NavegadorWebView : Page
    {
        public NavegadorWebView()
        {
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(WebViewUrl.Url))
            {
                webView.Source = new Uri(WebViewUrl.Url);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                webView.Source = new Uri($"http://{site.Text}");
            }
            catch (Exception)
            {
                NotificationService.ShowToastNotification("Erro", "Url inválida");
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new MyAppContext())
                {
                    var site = new Site
                    {
                        Titulo = webView.Source.ToString(),
                        Url = webView.Source.ToString(),
                        UsuarioId = UsuarioLogado.Id
                    };

                    context.Sites.Add(site);
                    context.SaveChanges();
                }

                await SaveImage();

                MessageDialog msg = new MessageDialog($"A página salva com sucesso!", "Página da web");
                await msg.ShowAsync();
            }
            catch (Exception ex)
            {
                MessageDialog msg = new MessageDialog($"Erro: {ex.Message}", "Erro");
                await msg.ShowAsync();
            }
        }

        private async Task SaveImage()
        {
            var brush = new WebViewBrush();
            brush.SetSource(webView);
            brush.Redraw();

            var myRect = new Rectangle();
            myRect.Fill = brush;

            //FileSavePicker fileSavePicker = new FileSavePicker();
            //fileSavePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //fileSavePicker.FileTypeChoices.Add("JPEG files", new List<string>() { ".jpg" });
            //fileSavePicker.SuggestedFileName = "image";

            //var outputFile = await fileSavePicker.PickSaveFileAsync();

            //if (outputFile == null)
            //{
            //    // The user cancelled the picking operation
            //    return;
            //}

            var piclib = KnownFolders.PicturesLibrary;
            var rect = myRect as Rectangle;
            RenderTargetBitmap renderbmp = new RenderTargetBitmap();
            await renderbmp.RenderAsync(rect);
            var pixels = await renderbmp.GetPixelsAsync();
            var file = await piclib.CreateFileAsync("webview.png", CreationCollisionOption.GenerateUniqueName);
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                byte[] bytes = pixels.ToArray();
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                    BitmapAlphaMode.Ignore,
                                    (uint)rect.Width, (uint)rect.Height,
                                    0, 0, bytes);
                await encoder.FlushAsync();
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            double remainingWidth = Window.Current.Bounds.Width;
            webView.Width = remainingWidth;
            webView.Height = Window.Current.Bounds.Height;
        }
    }
}
