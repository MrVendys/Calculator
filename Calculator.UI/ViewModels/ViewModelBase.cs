﻿using System.ComponentModel;

namespace Calculator.UI.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string nazevPromenne)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nazevPromenne));
        }
    }
}
