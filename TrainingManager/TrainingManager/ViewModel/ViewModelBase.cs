using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrainingManager.Model;

namespace TrainingManager.ViewModel
{
    /// <summary>
    /// Nézetmodell ősosztály típusa.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Tulajdonság változásának eseménye.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Üzenet küldésének eseménye.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageApplication;

        public event EventHandler<ExceptionArgs> ExceptionOccured;

        /// <summary>
        /// Tulajdonság változása ellenőrzéssel.
        /// </summary>
        /// <param name="propertyName">Tulajdonság neve.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Üzenet küldésének eseménykiváltása.
        /// </summary>
        /// <param name="message">Az üzenet.</param>
        protected void OnMessageApplication(string message) => MessageApplication?.Invoke(this, new MessageEventArgs(message));

        protected void OnExeptionoccured(ExceptionArgs exception) => ExceptionOccured?.Invoke(this, exception);

        protected abstract void InitializeCommands();
    }
}
