using System.Linq;
using MyFavoriteWeb.Models;
using Windows.UI.Xaml.Controls;

namespace MyFavoriteWeb.Views
{
    public sealed partial class ListaSitesView : Page
    {
        public ListaSitesView()
        {
            InitializeComponent();

            using (var context = new MyAppContext())
            {
                var lista = context.Sites.ToList();

                listaSites.ItemsSource = lista;
            }
        }
    }
}
