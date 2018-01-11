using System.Linq;
using Xamarin.Forms;
using Windows.Storage;
using MyFavoriteWeb.Views;
using System.Windows.Input;
using MyFavoriteWeb.Models;
using MyFavoriteWeb.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MyFavoriteWeb.Models.Singletons;

namespace MyFavoriteWeb.ViewModels
{
    public class ListaSitesViewModel
    {
        public string ItemClicado;
        public ObservableCollection<Site> ListaSites { get; set; } = new ObservableCollection<Site>();
        public ICommand Visitar { get; set; }
        public ICommand Excluir { get; set; }
        public ICommand ExcluirTodos { get; set; }

        internal void Initialize()
        {
            CarregaListaSites();

            Visitar = new Command(VisitarSite);
            Excluir = new Command(ExcluirSite);
            ExcluirTodos = new Command(ExcluirTodosSites);
        }

        private void ExcluirTodosSites()
        {
            foreach (var item in ListaSites.ToList())
            {
                using (var context = new MyAppContext())
                {
                    ListaSites.Remove(item);
                    context.Sites.Remove(item);
                    context.SaveChanges();
                }
            }

            ItemClicado = string.Empty;
        }

        private void ExcluirSite()
        {
            foreach (var item in ListaSites.ToList())
            {
                if (item.Id.ToString() == ItemClicado)
                {
                    using (var context = new MyAppContext())
                    {
                        ListaSites.Remove(item);
                        context.Sites.Remove(item);
                        context.SaveChanges();
                    }
                }
            }

            ItemClicado = string.Empty;
        }

        private void VisitarSite()
        {
            foreach (var item in ListaSites.ToList())
            {
                if (item.Id.ToString() == ItemClicado)
                {
                    WebViewUrl.Url = item.Url;
                    NavigationService.Navigate<NavegadorWebView>();
                }
            }

            ItemClicado = string.Empty;
        }

        private void CarregaListaSites()
        {
            using (var context = new MyAppContext())
            {
                var sites = context.Sites.ToList();
                sites.ForEach(l => l.Imagem = PegarPrint(l.Imagem));

                ListaSites = ConverterLista(sites);
            }
        }

        private ObservableCollection<Site> ConverterLista(List<Site> sites)
        {
            foreach (var item in sites)
            {
                ListaSites.Add(item);
            }

            return ListaSites;
        }

        private string PegarPrint(string nomeImagem)
        {
            var myfolder = ApplicationData.Current.LocalFolder;
            return $"{myfolder.Path}/{nomeImagem}";
        }
    }
}
