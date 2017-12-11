using System;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using MyFavoriteWeb.Models;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.Models.Singletons;

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

                MessageDialog msg = new MessageDialog($"A página salva com sucesso!", "Página da web");
                await msg.ShowAsync();

                var brush = new WebViewBrush();
                brush.SetSource(webView);
                brush.Redraw();
                //myRect.Fill = brush;
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
            webView.Width = remainingWidth - 48;
            webView.Height = Window.Current.Bounds.Height;
        }
    }
}
