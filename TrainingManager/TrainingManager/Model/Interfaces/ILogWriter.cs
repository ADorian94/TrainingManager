using NLog;

namespace TrainingManager.Model.Interfaces
{
    public interface ILogWriter
    {
        Logger Nlog { get; set; }
    }
}
