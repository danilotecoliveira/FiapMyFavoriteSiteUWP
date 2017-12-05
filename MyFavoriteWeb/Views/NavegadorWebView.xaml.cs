using System;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace MyFavoriteWeb.Views
{
    public sealed partial class NavegadorWebView : Page
    {
        public NavegadorWebView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            webView.Source = new Uri(site.Text);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog($"A página {webView.Source} foi salva com sucesso!", "Página da web");
            await msg.ShowAsync();

            var brush = new WebViewBrush();
            brush.SetSource(webView);
            brush.Redraw();
            //myRect.Fill = brush;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            double remainingWidth = Window.Current.Bounds.Width;
            webView.Width = remainingWidth - 48;
            webView.Height = Window.Current.Bounds.Height;
        }
    }
}
