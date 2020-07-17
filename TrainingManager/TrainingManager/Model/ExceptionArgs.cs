using System;

namespace TrainingManager.Model
{

    public class ExceptionArgs : Exception
    {
        public ExceptionArgs(Exception exception) : base(exception.Message) { }

        public ExceptionArgs(string message) : base(message) { }

        public string CallStack { get; set; }
    }
}
