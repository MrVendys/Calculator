using System.ComponentModel;

namespace Calculator.ViewModels
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string nazevPromenne)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nazevPromenne));
        }
    }
}
