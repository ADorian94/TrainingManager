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
        public event EventHandler<MessageContent> PopUpMessage;
        public event EventHandler<(MessageContent, Action)> PopUpMessageWithCallBack;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnMessageApplication(string message) => MessageApplication?.Invoke(this, new MessageEventArgs(message));
        protected void SendPopUpMessage(MessageContent message) => PopUpMessage?.Invoke(this, message);
        protected void SendPopUpMessage(MessageContent message, Action callBack) => PopUpMessageWithCallBack?.Invoke(this, (message, callBack));
        protected void OnExeptionOccured(ExceptionArgs exception) => ExceptionOccured?.Invoke(this, exception);
        protected abstract void InitializeCommands();
    }
}
