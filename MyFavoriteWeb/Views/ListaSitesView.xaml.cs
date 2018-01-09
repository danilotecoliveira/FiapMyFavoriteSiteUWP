using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using MyFavoriteWeb.Models;
using Windows.UI.Xaml.Input;
using MyFavoriteWeb.Services;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MyFavoriteWeb.Models.Singletons;

namespace MyFavoriteWeb.Views
{
    public sealed partial class ListaSitesView : Page
    {
        private ObservableCollection<Site> lista = new ObservableCollection<Site>();
        private string itemClicado;

        public ListaSitesView()
        {
            InitializeComponent();

            using (var context = new MyAppContext())
            {
                var sites = context.Sites.ToList();
                sites.ForEach(l => l.Imagem = PegarPrint(l.Imagem));

                listaSites.ItemsSource = ConverterLista(sites);
            }
        }

        private ObservableCollection<Site> ConverterLista(List<Site> lista)
        {

            foreach (var item in lista)
            {
                this.lista.Add(item);
            }

            return this.lista;
        }

        private string PegarPrint(string nomeImagem)
        {
            var myfolder = ApplicationData.Current.LocalFolder;
            return $"{myfolder.Path}/{nomeImagem}";
        }

        private void listaSites_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var listView = (ListView)sender;
            sitesCadastrados.ShowAt(listView, e.GetPosition(listView));
            var a = ((FrameworkElement)e.OriginalSource).DataContext as Site;
            itemClicado = a.Id.ToString();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in lista.ToList())
            {
                if (item.Id.ToString() == itemClicado)
                {
                    using (var context = new MyAppContext())
                    {
                        lista.Remove(item);
                        context.Sites.Remove(item);
                        context.SaveChanges();
                    }
                }
            }

            itemClicado = string.Empty;
        }

        private void Visitar_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in lista.ToList())
            {
                if (item.Id.ToString() == itemClicado)
                {
                    WebViewUrl.Url = item.Url;
                    NavigationService.Navigate<NavegadorWebView>();
                }
            }

            itemClicado = string.Empty;
        }
    }
}
