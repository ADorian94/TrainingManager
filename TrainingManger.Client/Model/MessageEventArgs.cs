namespace TrainingManager.Model
{
    public class MessageEventArgs : EventArgs
    {
        public string Title { get; private set; }
        public string Message { get; private set; }

        public MessageEventArgs(string title) => Message = title;

        public MessageEventArgs(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
