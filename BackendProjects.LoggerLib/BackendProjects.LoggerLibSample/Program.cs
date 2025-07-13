using BackendProjects.LoggerLib;
using System.Threading;

namespace BackendProjects.LoggerLibSample
{
    internal class Program
    {
        static void Main()
        {
            Logger logger = Logger.Instance;

            logger.EnableFileLogging = true;
            logger.EnableConsoleLogging = true;

            logger.Log("Starting logging threads...", LogLevel.Info);

            int threadCount = 3;
            int messagesPerThread = 10;
            Thread[] threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                int threadNum = i + 1;
                threads[i] = new Thread(() =>
                {
                    for (int j = 1; j <= messagesPerThread; j++)
                    {
                        if (j % 10 == 0)
                            logger.Log($"Message {j} from thread {threadNum}", LogLevel.Error);
                        else if (j % 3 == 0)
                            logger.Log($"Message {j} from thread {threadNum}", LogLevel.Warning);
                        else
                            logger.Log($"Message {j} from thread {threadNum}");

                        Thread.Sleep(50); // simulate work
                    }
                });
                threads[i].Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }

            logger.Log("Finished running all logging threads!", LogLevel.Info);
        }
    }
}
