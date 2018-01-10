using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MyFavoriteWeb.ViewModels;
using Windows.UI.Core;
using System;

namespace MyFavoriteWeb.Views
{
    public sealed partial class SobreView : Page
    {
        public SobreViewModel ViewModel { get; } = new SobreViewModel();

        public SobreView()
        {
            InitializeComponent();
            Loaded += Login_Loaded;

            //SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }

        //private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        //{
        //    if (Frame.CanGoBack)
        //    {
        //        Frame.GoBack();
        //        e.Handled = true;
        //    }
        //}

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
