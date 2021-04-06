using System;
using Velentr.Logging.Entry;

namespace Velentr.Logging.Loggers
{

    /// <summary>
    /// A logger that outputs messages to sub-loggers
    /// </summary>
    /// <seealso cref="Velentr.Logging.Loggers.Logger" />
    public class AllLogger : Logger
    {

        /// <summary>
        /// The loggers
        /// </summary>
        public Logger[] Loggers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllLogger"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="batchedLoggingTimeBetweenBatchesMilliseconds">The batched logging time between batches milliseconds.</param>
        /// <param name="batchedLoggingMaxNumberOfItemsPerBatch">The batched logging maximum number of items per batch.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="maxCharactersPerLine">The maximum characters per line.</param>
        /// <param name="loggers">The loggers.</param>
        public AllLogger(string name, LogLevel logLevel = LogLevel.Trace, int batchedLoggingTimeBetweenBatchesMilliseconds = 5000, int batchedLoggingMaxNumberOfItemsPerBatch = int.MaxValue, TimeZoneInfo timeZone = null, int maxCharactersPerLine = 0, params Logger[] loggers) : base(name, logLevel, LogMode.Instant, batchedLoggingTimeBetweenBatchesMilliseconds, batchedLoggingMaxNumberOfItemsPerBatch, timeZone, maxCharactersPerLine)
        {
            Loggers = loggers;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllLogger"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="loggers">The loggers.</param>
        public AllLogger(string name, LogLevel logLevel = LogLevel.Trace, params Logger[] loggers) : this(name, logLevel, 5000, int.MaxValue, null, 0, loggers) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllLogger"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="loggers">The loggers.</param>
        public AllLogger(string name, params Logger[] loggers) : this(name, LogLevel.Trace, 5000, int.MaxValue, null, 0, loggers) { }

        /// <summary>
        /// Logs the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public override void Log(LogEntry entry)
        {
            for (var i = 0; i < Loggers.Length; i++)
            {
                Loggers[i].Log(entry);
            }
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        public override void Setup()
        {
            for (var i = 0; i < Loggers.Length; i++)
            {
                Loggers[i].Setup();
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            for (var i = 0; i < Loggers.Length; i++)
            {
                Loggers[i].Update();
            }
        }

    }

}
