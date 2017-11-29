using MyFavoriteWeb.Abstracts;

namespace MyFavoriteWeb.ViewModels
{
    public class MainPageViewModel : NotifyableClass
    {
        public void Initialize()
        {
        
        }

        private bool _isSplitViewOpen;

        public bool IsSplitViewOpen
        {
            get { return _isSplitViewOpen; }
            set { Set(ref _isSplitViewOpen, value); }
        }

        public void HamburguerButton_Click()
        {
            IsSplitViewOpen = !IsSplitViewOpen;
        }

        public void WebView_Click()
        {
            
        }
    }
}
