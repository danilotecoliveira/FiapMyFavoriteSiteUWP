using System;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using MyFavoriteWeb.Models;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.Models.Singletons;
using MyFavoriteWeb.Services;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Display;

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
                var printTela = await Savetoimage();

                using (var context = new MyAppContext())
                {
                    var site = new Site
                    {
                        Imagem = printTela,
                        Url = webView.Source.ToString(),
                        UsuarioId = UsuarioLogado.Id
                    };

                    context.Sites.Add(site);
                    context.SaveChanges();
                }

                MessageDialog msg = new MessageDialog($"A página salva com sucesso!", "Página da web");
                await msg.ShowAsync();
            }
            catch (Exception ex)
            {
                MessageDialog msg = new MessageDialog($"Erro: {ex.Message}", "Erro");
                await msg.ShowAsync();
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            double remainingWidth = Window.Current.Bounds.Width;
            webView.Width = remainingWidth;
            webView.Height = Window.Current.Bounds.Height;
        }

        private async void webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            rectangle.Fill = await GetWebViewBrush(webView);
        }

        private async Task<WebViewBrush> GetWebViewBrush(WebView webView)
        {
            double larguraOriginal = webView.Width;
            var largura = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollWidth.toString()" });
            int conteudoLargura;

            if (!int.TryParse(largura, out conteudoLargura))
            {
                throw new Exception(string.Format("failure/width:{0}", largura));
            }

            webView.Width = conteudoLargura;

            double alturaOriginal = webView.Height;
            var altura = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollHeight.toString()" });
            int conteudoAltura;

            if (!int.TryParse(altura, out conteudoAltura))
            {
                throw new Exception(string.Format("failure/height:{0}", altura));
            }

            webView.Height = conteudoAltura;

            var original = webView.Visibility;
            webView.Visibility = Visibility.Visible;

            var webPrint = new WebViewBrush
            {
                SourceName = webView.Name,
                Stretch = Stretch.Uniform
            };

            webPrint.Redraw();

            webView.Width = larguraOriginal;
            webView.Height = alturaOriginal;
            webView.Visibility = original;

            return webPrint;
        }

        private async Task<string> Savetoimage()
        {
            var piclib = ApplicationData.Current.LocalFolder;
            var rect = rectangle as Rectangle;
            var renderbmp = new RenderTargetBitmap();
            await renderbmp.RenderAsync(rectangle);
            var nomeImagem = $"{Guid.NewGuid()}.bmp";
            var displayInformation = DisplayInformation.GetForCurrentView();
            var pixels = await renderbmp.GetPixelsAsync();

            var file = await piclib.CreateFileAsync(nomeImagem, CreationCollisionOption.GenerateUniqueName);
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream);
                var bytes = pixels.ToArray();
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)renderbmp.PixelWidth, (uint)renderbmp.PixelHeight, 
                    displayInformation.LogicalDpi,
                    displayInformation.LogicalDpi, bytes);
                await encoder.FlushAsync();
            }

            return nomeImagem;
        }
    }
}
