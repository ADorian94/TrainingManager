using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrainingManager.Model;

namespace TrainingManager.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            InitializeCommands();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<MessageEventArgs> MessageApplication;
        public event EventHandler<ExceptionArgs> ExceptionOccured;
        public event EventHandler<Messages> PopUpMessage;

        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnMessageApplication(string message) => MessageApplication?.Invoke(this, new MessageEventArgs(message));
        protected void SendPopUpMessage(Messages message) => PopUpMessage?.Invoke(this, message);
        protected void OnExeptionOccured(ExceptionArgs exception) => ExceptionOccured?.Invoke(this, exception);
        protected abstract void InitializeCommands();
    }
}
