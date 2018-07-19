using System.ComponentModel;
using System.Runtime.CompilerServices;
using SvgViewer.Properties;

namespace SvgViewer
{
    public class NotifyPropertyChanger : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(property, value) is false)
            {
                property = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
