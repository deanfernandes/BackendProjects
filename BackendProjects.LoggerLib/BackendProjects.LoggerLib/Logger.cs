using System.IO;

namespace BackendProjects.LoggerLib
{
    public sealed partial class Logger
    {
        private static readonly Logger _instance = new Logger();
        private readonly string _logFilePath;
        public bool EnableConsoleLogging { get; set; } = true;
        public bool EnableFileLogging { get; set; } = true;

        private Logger()
        {
            Directory.CreateDirectory("logs");
            this._logFilePath = Path.Combine("logs", "_.log");
        }

        public static Logger Instance => _instance;

        public void Log(string message, LogLevel logLevel = LogLevel.Debug)
        {
            string formattedMessage = $"{DateTime.Now.ToString()}: [{logLevel}] {message}";
            if(EnableConsoleLogging) 
                WriteToConsole(formattedMessage, logLevel);
            if (EnableFileLogging)
                WriteToFile(formattedMessage, logLevel);
        }
    }
}
