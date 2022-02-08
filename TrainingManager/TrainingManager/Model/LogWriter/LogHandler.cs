using System;
using System.IO;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;
using TrainingManager.Model.Interfaces;
using Xamarin.Forms;

namespace TrainingManager.Model.LogWriter
{
    public sealed class LogHandler : ILogWriter
    {
        private static Lazy<ILogWriter> Lazy => new Lazy<ILogWriter>(() => new LogHandler(), true);
        public static ILogWriter Instance => Lazy.Value;
        public Logger Nlog { get; set; }

        public LogHandler()
        {
            //Platform specific 
            string pathOfLogDirectory = Path.Combine(DependencyService.Get<IDataAcess>().GetExternalStorage(), "logs");

            if (!Directory.Exists(pathOfLogDirectory))
                Directory.CreateDirectory(pathOfLogDirectory);

            string pathOfLogFile = Path.Combine(Path.Combine(pathOfLogDirectory, "LiftIt.log"));
            var config = new LoggingConfiguration();
            var target = new FileTarget
            {
                FileName = pathOfLogFile,
                Layout = "${longdate} ${uppercase:${level}} ${message}",
                ArchiveFileName = Path.Combine(pathOfLogDirectory, @"archives/liftit-{#}.log"),
                ArchiveEvery = FileArchivePeriod.Hour,
                ArchiveNumbering = ArchiveNumberingMode.Date,
                MaxArchiveFiles = 5,
                ArchiveDateFormat = "yyyy-MM-dd-HH-mm",
                Encoding = Encoding.UTF8,
            };
            config.AddTarget("logfile", target);
            var rule = new LoggingRule("*", LogLevel.Debug, target);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;
            Nlog = LogManager.GetCurrentClassLogger();
        }
    }
}
