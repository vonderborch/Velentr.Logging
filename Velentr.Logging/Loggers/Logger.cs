using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Velentr.Logging.Entry;
using Velentr.Logging.Helpers;

namespace Velentr.Logging.Loggers
{

    /// <summary>
    /// Represents a logger object
    /// </summary>
    public abstract class Logger
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="logMode">The log mode.</param>
        /// <param name="batchedLoggingTimeBetweenBatchesMilliseconds">The batched logging time between batches milliseconds.</param>
        /// <param name="batchedLoggingMaxNumberOfItemsPerBatch">The batched logging maximum number of items per batch.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="maxCharactersPerLine">The maximum characters per line.</param>
        /// <param name="allowNewLines">Whether to allow the AddNewLines methods to output entries.</param>
        /// <param name="allowDividers">Whether to allow the AddDividers methods to output entries.</param>
        protected Logger(string name, LogLevel logLevel = LogLevel.Trace, LogMode logMode = LogMode.Instant, int batchedLoggingTimeBetweenBatchesMilliseconds = 5000, int batchedLoggingMaxNumberOfItemsPerBatch = int.MaxValue, TimeZoneInfo timeZone = null, int maxCharactersPerLine = 0, bool allowNewLines = true, bool allowDividers = true)
        {
            Name = name;
            MinimumLogLevel = logLevel;
            LogMode = logMode;
            BatchedLoggingTimeBetweenBatchesMilliseconds = batchedLoggingTimeBetweenBatchesMilliseconds;
            BatchedLoggingMaxNumberOfItemsPerBatch = batchedLoggingMaxNumberOfItemsPerBatch;
            TimeZone = timeZone ?? TimeZoneInfo.Local;
            MaxCharactersPerLine = maxCharactersPerLine;
            LastBatchUpdateTime = TimeProvider.GetCurrentTime(TimeZone);
            AllowNewLines = allowNewLines;
            AllowDividers = allowDividers;
        }

        /// <summary>
        /// Gets or sets the log name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the last batch update time.
        /// </summary>
        /// <value>
        /// The last batch update time.
        /// </value>
        public DateTime LastBatchUpdateTime { get; protected set; }

        /// <summary>
        /// Gets or sets the minimum log level.
        /// </summary>
        /// <value>
        /// The minimum log level.
        /// </value>
        public LogLevel MinimumLogLevel { get; protected set; }

        /// <summary>
        /// Gets or sets the log mode.
        /// </summary>
        /// <value>
        /// The log mode.
        /// </value>
        public LogMode LogMode { get; protected set; }

        /// <summary>
        /// Gets or sets the batched logging time between batches milliseconds.
        /// </summary>
        /// <value>
        /// The batched logging time between batches milliseconds.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int BatchedLoggingTimeBetweenBatchesMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the batched logging maximum number of items per batch.
        /// </summary>
        /// <value>
        /// The batched logging maximum number of items per batch.
        /// </value>
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int BatchedLoggingMaxNumberOfItemsPerBatch { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>
        /// The time zone.
        /// </value>
        public TimeZoneInfo TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the maximum characters per line.
        /// </summary>
        /// <value>
        /// The maximum characters per line.
        /// </value>
        [Range(0, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}. To prevent word wrap, set this to 0.")]
        public int MaxCharactersPerLine { get; set; }

        /// <summary>
        /// Gets or sets the entry divider.
        /// </summary>
        /// <value>
        /// The entry divider.
        /// </value>
        public string EntryDivider { get; set; } = "----------------------------------------------------";

        /// <summary>
        /// Creates new line.
        /// </summary>
        /// <value>
        /// The new line.
        /// </value>
        public string NewLine { get; set; } = Environment.NewLine;

        /// <summary>
        /// Gets the prepended parameter method mapping.
        /// </summary>
        /// <value>
        /// The prepended parameter method mapping.
        /// </value>
        public Func<string, LogLevel, DateTime, string, string>[] PrependedParameterMethodMapping => new Func<string, LogLevel, DateTime, string, string>[2] {GetTimeFormatted, GetLogLevel};

        /// <summary>
        /// Gets or sets the prepended entry format.
        /// </summary>
        /// <value>
        /// The prepended entry format.
        /// </value>
        public string PrependedEntryFormat { get; set; } = "{TimeStamp} [{Level}] ";

        /// <summary>
        /// Gets or sets the time format.
        /// </summary>
        /// <value>
        /// The time format.
        /// </value>
        public string TimeFormat { get; set; } = "dd/MM/yy - hh:mm:ss:fff";

        /// <summary>
        /// Gets or sets a value indicating whether [word wrap prepend each line].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [word wrap prepend each line]; otherwise, <c>false</c>.
        /// </value>
        public bool WordWrapPrependEachLine { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether [allow dividers].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow dividers]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowDividers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow new lines].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow new lines]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowNewLines { get; set; }

        /// <summary>
        /// Gets the formatted time.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="level">The level.</param>
        /// <param name="timeStamp">The time stamp.</param>
        /// <param name="timeFormat">The time format.</param>
        /// <returns>The formatted time</returns>
        public static string GetTimeFormatted(string entry, LogLevel level, DateTime timeStamp, string timeFormat)
        {
            return timeStamp.ToString(timeFormat);
        }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="level">The level.</param>
        /// <param name="timeStamp">The time stamp.</param>
        /// <param name="timeFormat">The time format.</param>
        /// <returns>The log level</returns>
        public static string GetLogLevel(string entry, LogLevel level, DateTime timeStamp, string timeFormat)
        {
            return Constants.Settings.LogLevelNameMapping[level];
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public void AddEntry(string entry)
        {
            AddEntryInternal(entry, MinimumLogLevel, TimeProvider.GetCurrentTime(TimeZone));
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="entry">The entry.</param>
        public void AddEntry(LogLevel level, string entry)
        {
            if (level > MinimumLogLevel || level == LogLevel.Off)
            {
                return;
            }

            AddEntryInternal(entry, level, TimeProvider.GetCurrentTime(TimeZone));
        }

        /// <summary>
        /// Adds dividers to the log.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="count">The count.</param>
        public void AddDividers(LogLevel level, int count)
        {
            if (level > MinimumLogLevel || level == LogLevel.Off)
            {
                return;
            }

            var entry = new StringBuilder();

            for (var i = 0; i < count; i++)
            {
                entry.AppendLine(EntryDivider);
            }

            AddEntryInternal(entry.ToString(), level, TimeProvider.GetCurrentTime(TimeZone));
        }

        /// <summary>
        /// Adds dividers to the log.
        /// </summary>
        /// <param name="count">The count.</param>
        public void AddDividers(int count)
        {
            AddDividers(MinimumLogLevel, count);
        }

        /// <summary>
        /// Adds new lines to the log.
        /// </summary>
        /// <param name="count">The count.</param>
        public void AddNewLines(int count)
        {
            AddNewLines(MinimumLogLevel, count);
        }

        /// <summary>
        /// Adds new lines to the log.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="count">The count.</param>
        public void AddNewLines(LogLevel level, int count)
        {
            if (level > MinimumLogLevel || level == LogLevel.Off)
            {
                return;
            }

            var entry = new StringBuilder();

            for (var i = 0; i < count; i++)
            {
                entry.AppendLine(NewLine);
            }

            AddEntryInternal(entry.ToString(), level, TimeProvider.GetCurrentTime(TimeZone));
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="parameters">The parameters.</param>
        public void AddEntry(string entry, params object[] parameters)
        {
            AddEntry(MinimumLogLevel, entry, parameters);
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="parameters">The parameters.</param>
        public void AddEntry(LogLevel level, string entry, params object[] parameters)
        {
            if (level > MinimumLogLevel || level == LogLevel.Off)
            {
                return;
            }

            AddEntryInternal(StringHelpers.FormatString(entry, parameters), level, TimeProvider.GetCurrentTime(TimeZone));
        }

        /// <summary>
        /// Internal method for adding the entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="level">The level.</param>
        /// <param name="timeStamp">The time stamp.</param>
        protected void AddEntryInternal(string entry, LogLevel level, DateTime timeStamp)
        {
            if (!string.IsNullOrWhiteSpace(PrependedEntryFormat))
            {
                var parameters = new object[PrependedParameterMethodMapping.Length];
                for (var i = 0; i < PrependedParameterMethodMapping.Length; i++)
                {
                    parameters[i] = PrependedParameterMethodMapping[i](entry, level, timeStamp, TimeFormat);
                }

                var prepend = StringHelpers.FormatString(PrependedEntryFormat, parameters);
                if (MaxCharactersPerLine > 0)
                {
                    var splitEntry = StringHelpers.GetStringChunks(entry, MaxCharactersPerLine).ToList();
                    var wordWrapPrepend = WordWrapPrependEachLine ? prepend : StringHelpers.GetStringOfSpaces(prepend.Length);

                    var entryString = new StringBuilder();
                    for (var i = 0; i < splitEntry.Count; i++)
                    {
                        entryString.AppendLine(string.Format($"{wordWrapPrepend}{splitEntry[i]}"));
                    }

                    entry = entryString.ToString();
                }
                else
                {
                    entry = $"{prepend}{entry}";
                }
            }

            Log(new LogEntry(this, entry, level, timeStamp));
        }

        /// <summary>
        /// Adds a message at Fatal log level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal(string message)
        {
            AddEntry(LogLevel.Fatal, message);
        }

        /// <summary>
        /// Adds a message at Fatal log level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Fatal(string message, params object[] parameters)
        {
            AddEntry(LogLevel.Fatal, message, parameters);
        }

        /// <summary>
        /// Logs the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public abstract void Log(LogEntry entry);

        /// <summary>
        /// Setups this instance.
        /// </summary>
        public abstract void Setup();

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Adds a message at Error log level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
            AddEntry(LogLevel.Error, message);
        }

        /// <summary>
        /// Adds a message at Error log level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Error(string message, params object[] parameters)
        {
            AddEntry(LogLevel.Error, message, parameters);
        }

        /// <summary>
        /// Adds a message at Warning log level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warning(string message)
        {
            AddEntry(LogLevel.Warning, message);
        }

        /// <summary>
        /// Adds a message at Warning log level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Warning(string message, params object[] parameters)
        {
            AddEntry(LogLevel.Warning, message, parameters);
        }

        /// <summary>
        /// Adds a message at Info log level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            AddEntry(LogLevel.Info, message);
        }

        /// <summary>
        /// Adds a message at Info log level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Info(string message, params object[] parameters)
        {
            AddEntry(LogLevel.Info, message, parameters);
        }

        /// <summary>
        /// Adds a message at Debug log level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
            AddEntry(LogLevel.Debug, message);
        }

        /// <summary>
        /// Adds a message at Debug log level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Debug(string message, params object[] parameters)
        {
            AddEntry(LogLevel.Debug, message, parameters);
        }

        /// <summary>
        /// Adds a message at Trace log level.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Trace(string message)
        {
            AddEntry(LogLevel.Trace, message);
        }

        /// <summary>
        /// Adds a message at Trace log level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Trace(string message, params object[] parameters)
        {
            AddEntry(LogLevel.Trace, message, parameters);
        }

    }

}
