using PropertyChanged;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TreeVisualization
{
    [AddINotifyPropertyChangedInterface]
    public abstract class BaseNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string str = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }
    }
}