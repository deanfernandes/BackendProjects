using System;
using System.IO;

namespace BackendProjects.LoggerLib
{
    public sealed partial class Logger
    {
        /// <summary>
        /// Writes log message to logs folder and log file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logLevel">The severity level of the log message.</param>
        private void WriteToFile(string message, LogLevel logLevel)
        {
            try
            {
                File.AppendAllText(this._logFilePath, message + Environment.NewLine);
            }
            catch
            {
                Console.Error.WriteLine("Logger failed to write to log file.");
            }
        }
    }
}
