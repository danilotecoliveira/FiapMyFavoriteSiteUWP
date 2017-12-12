namespace MyFavoriteWeb.Models.Singletons
{
    public sealed class WebViewUrl
    {
        private static string _url;

        public static string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public WebViewUrl(string url)
        {
            _url = url;
        }
    }
}
