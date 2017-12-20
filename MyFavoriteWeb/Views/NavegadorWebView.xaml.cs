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

                Savetoimage();

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
            // resize width to content
            double originalWidth = webView.Width;
            var widthString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollWidth.toString()" });
            int contentWidth;

            if (!int.TryParse(widthString, out contentWidth))
            {
                throw new Exception(string.Format("failure/width:{0}", widthString));
            }

            webView.Width = contentWidth;

            // resize height to content
            double originalHeight = webView.Height;
            var heightString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollHeight.toString()" });
            int contentHeight;

            if (!int.TryParse(heightString, out contentHeight))
            {
                throw new Exception(string.Format("failure/height:{0}", heightString));
            }

            webView.Height = contentHeight;

            // create brush
            var originalVisibilty = webView.Visibility;
            webView.Visibility = Visibility.Visible;

            WebViewBrush brush = new WebViewBrush
            {
                SourceName = webView.Name,
                Stretch = Stretch.Uniform
            };

            brush.Redraw();

            // reset, return
            webView.Width = originalWidth;
            webView.Height = originalHeight;
            webView.Visibility = originalVisibilty;

            return brush;
        }

        private async void Savetoimage()
        {
            var piclib = KnownFolders.PicturesLibrary;
            var rect = rectangle as Rectangle;
            var renderbmp = new RenderTargetBitmap();
            await renderbmp.RenderAsync(rect);
            var pixels = await renderbmp.GetPixelsAsync();
            var file = await piclib.CreateFileAsync("webview.bmp", CreationCollisionOption.GenerateUniqueName);
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream);
                var bytes = pixels.ToArray();
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)rect.Width, (uint)rect.Height, 0, 0, bytes);
                await encoder.FlushAsync();
            }
        }
    }
}
