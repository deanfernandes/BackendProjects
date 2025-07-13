using System;
using System.IO;

namespace BackendProjects.LoggerLib
{
    public sealed partial class Logger
    {
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
