using System;
using Velentr.Logging.Loggers;

namespace Velentr.Logging.Entry
{
    /// <summary>
    /// Represents a single log entry, of any type.
    /// </summary>
    public struct LogEntry
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> struct.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="timeStamp">The time stamp.</param>
        public LogEntry(Logger logger, string entry, LogLevel logLevel, DateTime timeStamp)
        {
            Logger = logger;
            Entry = entry;
            LogLevel = logLevel;
            TimeStamp = timeStamp;
        }

        /// <summary>
        /// Gets the logger associated with this log entry.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public Logger Logger { get; private set; }

        /// <summary>
        /// Gets the entry.
        /// </summary>
        /// <value>
        /// The entry.
        /// </value>
        public string Entry { get; private set; }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        public LogLevel LogLevel { get; private set; }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public DateTime TimeStamp { get; private set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Entry;
        }
    }
}
