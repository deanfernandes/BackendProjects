using System;

namespace BackendProjects.LoggerLib
{
    public sealed partial class Logger
    {
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
