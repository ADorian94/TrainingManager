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
        public event EventHandler<(Messages, Action)> PopUpMessageWithCallBack;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnMessageApplication(string message) => MessageApplication?.Invoke(this, new MessageEventArgs(message));
        protected void SendPopUpMessage(Messages message) => PopUpMessage?.Invoke(this, message);
        protected void SendPopUpMessage(Messages message, Action callBack) => PopUpMessageWithCallBack?.Invoke(this, (message, callBack));
        protected void OnExeptionOccured(ExceptionArgs exception) => ExceptionOccured?.Invoke(this, exception);
        protected abstract void InitializeCommands();
    }
}
