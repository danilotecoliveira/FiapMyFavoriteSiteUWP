using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyFavoriteWeb.ViewModels
{
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Set<T>(ref T data, T value, [CallerMemberName]string propertyName = null)
        {
            if (!object.Equals(data, value))
            {
                data = value;
                this.OnPropertyChanged(propertyName);
            }
        }
    }
}
