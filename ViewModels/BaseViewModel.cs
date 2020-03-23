using System;
using System.ComponentModel;

namespace EarTrainer
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected virtual void RaisePropertyChanged(string propName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName)); // некоторые из нас здесь используют Dispatcher, для безопасного взаимодействия с UI thread
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    class MagicAttribute : Attribute { }
    class NoMagicAttribute : Attribute { }
}

