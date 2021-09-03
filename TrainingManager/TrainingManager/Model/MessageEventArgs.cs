using System;

namespace TrainingManager.Model
{
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Cím lekérdezése, vagy beállítása.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Üzenet lekérdezése, vagy beállítása.
        /// </summary>
        public string Message { get; private set; }

        public MessageEventArgs(string title) => Message = title;

        public MessageEventArgs(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
