using System;
using System.IO;
using System.Threading;

namespace BackendProjects.LoggerLib
{
    /// <summary>
    /// Thread-safe logger that supports logging messages with different levels and outputs (console, file).
    /// </summary>
    public sealed partial class Logger
    {
        private static readonly Logger _instance = new();
        private readonly string _logFilePath;
        //private readonly object _logLock = new();
        private readonly Lock _logLock = new();

        /// <summary>
        /// Indicates whether to write logs to console.
        /// Default is true.
        /// </summary>
        public bool EnableConsoleLogging { get; set; } = true;
        /// <summary>
        /// Indicates whether to write logs to file.
        /// Default is true.
        /// </summary>
        public bool EnableFileLogging { get; set; } = true;

        private Logger()
        {
            Directory.CreateDirectory("logs");
            this._logFilePath = Path.Combine("logs", "logfile.log");
        }

        public static Logger Instance => _instance;

        /// <summary>
        /// Logs a message with timestamp and specified log level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logLevel">The severity level of the log message.</param>
        public void Log(string message, LogLevel logLevel = LogLevel.Debug)
        {
            string formattedMessage = $"{DateTime.Now}: [{logLevel}] {message}";

            lock (_logLock)
            {
                if (EnableConsoleLogging)
                    WriteToConsole(formattedMessage, logLevel);
                if (EnableFileLogging)
                    WriteToFile(formattedMessage, logLevel);
            }
        }
    }
}
