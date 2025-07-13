using System;

namespace BackendProjects.LoggerLib
{
    public sealed partial class Logger
    {
        /// <summary>
        /// Writes log message to console with different color depending on log level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logLevel">The severity level of the log message.</param>
        private void WriteToConsole(string message, LogLevel logLevel)
        {
            ConsoleColor originalConsoleColor = Console.ForegroundColor;
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
            }
            Console.WriteLine(message);
            Console.ForegroundColor = originalConsoleColor;
        }
    }
}
