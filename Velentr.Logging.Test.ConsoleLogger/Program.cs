using System.Threading;
using Velentr.Logging.ConsoleLogging;

namespace Velentr.Logging.Test.ConsoleLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new Loggers.ConsoleLogger("ConsoleLogger")
            {
                ApplyColorsToEntries = true
            };

            while (true)
            {
                logger.Trace("Hello World!");
                logger.Trace("Hello [f: blue]World!");
                logger.Trace("Hello [f: blue][b: red]World! [/]Resets!");
                Thread.Sleep(1000);
            }
        }
    }
}
