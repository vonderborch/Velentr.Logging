using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Velentr.Logging.ConsoleLogging;
using Velentr.Logging.Entry;
using Velentr.Logging.Helpers;

namespace Velentr.Logging.Loggers
{

    /// <summary>
    /// A logger that outputs messages to the console
    /// </summary>
    /// <seealso cref="Velentr.Logging.Loggers.Logger" />
    public class ConsoleLogger : Logger
    {

        /// <summary>
        /// The settings
        /// </summary>
        private ConsoleLoggerSettings _settings;

        /// <summary>
        /// The log entries
        /// </summary>
        protected ConcurrentQueue<LogEntry> LogEntries;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="logMode">The log mode.</param>
        /// <param name="batchedLoggingTimeBetweenBatchesMilliseconds">The batched logging time between batches milliseconds.</param>
        /// <param name="batchedLoggingMaxNumberOfItemsPerBatch">The batched logging maximum number of items per batch.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="maxCharactersPerLine">The maximum characters per line.</param>
        public ConsoleLogger(string name, ConsoleLoggerSettings settings = new ConsoleLoggerSettings(), LogLevel logLevel = LogLevel.Trace, LogMode logMode = LogMode.Instant, int batchedLoggingTimeBetweenBatchesMilliseconds = 5000, int batchedLoggingMaxNumberOfItemsPerBatch = int.MaxValue, TimeZoneInfo timeZone = null, int maxCharactersPerLine = 0) : base(name, logLevel, logMode, batchedLoggingTimeBetweenBatchesMilliseconds, batchedLoggingMaxNumberOfItemsPerBatch, timeZone, maxCharactersPerLine)
        {
            LogEntries = new ConcurrentQueue<LogEntry>();
            _settings = settings;
        }

        /// <summary>
        /// Gets or sets the color of the back.
        /// </summary>
        /// <value>
        /// The color of the back.
        /// </value>
        public ConsoleColor BackColor
        {
            get => _settings.BackColor;
            set => _settings.BackColor = value;
        }

        /// <summary>
        /// Gets or sets the color of the fore.
        /// </summary>
        /// <value>
        /// The color of the fore.
        /// </value>
        public ConsoleColor ForeColor
        {
            get => _settings.ForeColor;
            set => _settings.ForeColor = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [apply colors to entries].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [apply colors to entries]; otherwise, <c>false</c>.
        /// </value>
        public bool ApplyColorsToEntries
        {
            get => _settings.ApplyColorsToEntries;
            set => _settings.ApplyColorsToEntries = value;
        }

        /// <summary>
        /// Logs the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public override void Log(LogEntry entry)
        {
            LogEntries.Enqueue(entry);
            if (LogMode == LogMode.Instant)
            {
                UpdateBatch();
            }
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        public override void Setup() { }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update()
        {
            switch (LogMode)
            {
                case LogMode.Instant:
                    UpdateBatch();
                    break;
                case LogMode.Batched:
                    if (TimeHelpers.ElapsedMilliSeconds(LastBatchUpdateTime, TimeProvider.GetCurrentTime(TimeZone)) > BatchedLoggingTimeBetweenBatchesMilliseconds)
                    {
                        UpdateBatch();
                        LastBatchUpdateTime = TimeProvider.GetCurrentTime(TimeZone);
                    }

                    break;
            }
        }

        /// <summary>
        /// Updates the batch.
        /// </summary>
        private void UpdateBatch()
        {
            for (var i = 0; i < BatchedLoggingMaxNumberOfItemsPerBatch && !LogEntries.IsEmpty; i++)
            {
                if (!LogEntries.TryDequeue(out var entry))
                {
                    break;
                }

                if (ApplyColorsToEntries)
                {
                    // Item1 = string, Item2 = BackgroundColor, Item3 = ForegroundColor
                    var writes = new List<(string, ConsoleColor, ConsoleColor)>();
                    var entryString = entry.Entry;
                    var currentBackColor = BackColor;
                    var currentForeColor = ForeColor;
                    var currentWriteEntry = new StringBuilder();
                    for (var j = 0; j < entryString.Length; j++)
                    {
                        if (entryString[j] == '[' && j > 0 && entryString[j - 1] != '\\')
                        {
                            var results = ApplyMarkdownCommands(entryString, BackColor, ForeColor, j);

                            if (j != results.Item1)
                            {
                                writes.Add((currentWriteEntry.ToString(), currentBackColor, currentForeColor));
                                currentWriteEntry = new StringBuilder();
                                j = results.Item1;
                                currentBackColor = results.Item2;
                                currentForeColor = results.Item3;
                                continue;
                            }
                        }

                        currentWriteEntry.Append(entryString[j]);
                    }

                    if (currentWriteEntry.Length > 0)
                    {
                        writes.Add((currentWriteEntry.ToString(), currentBackColor, currentForeColor));
                    }

                    for (var j = 0; j < writes.Count; j++)
                    {
                        Console.BackgroundColor = writes[j].Item2;
                        Console.ForegroundColor = writes[j].Item3;
                        Console.Write(writes[j].Item1);
                    }

                    Console.WriteLine(' ');
                    Console.BackgroundColor = BackColor;
                    Console.ForegroundColor = ForeColor;
                }
                else
                {
                    Console.BackgroundColor = BackColor;
                    Console.ForegroundColor = ForeColor;
                    Console.WriteLine(entry.Entry);
                }
            }
        }

        /// <summary>
        /// Applies the markdown commands.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        /// <param name="currentIndex">Index of the current.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Invalid markdown exception!
        /// or
        /// Invalid markdown exception!
        /// </exception>
        private (int, ConsoleColor, ConsoleColor) ApplyMarkdownCommands(string text, ConsoleColor backgroundColor, ConsoleColor foregroundColor, int currentIndex)
        {
            int finalIIndex;
            int endIndex;
            var finalBackgroundColor = backgroundColor;
            var finalForegroundColor = foregroundColor;

            // exit early if we're at the end of the string
            if (text.Length <= currentIndex + 1)
            {
                return (currentIndex, finalBackgroundColor, finalForegroundColor);
            }

            switch (text[currentIndex + 1])
            {
                // invalid markdown, we'll skip this markdown...
                case ']':
                case '/':
                    endIndex = text.Substring(currentIndex).IndexOf(']');
                    finalIIndex = endIndex == -1 ? text.Length : endIndex + currentIndex;

                    if (text[currentIndex + 1] == '/')
                    {
                        finalBackgroundColor = backgroundColor;
                        finalForegroundColor = foregroundColor;
                    }

                    break;
                default:
                    endIndex = text.Substring(currentIndex).IndexOf(']');
                    finalIIndex = endIndex == -1 ? text.Length : endIndex + currentIndex;
                    var length = finalIIndex - (currentIndex + 1);
                    var rawMarkdown = text.Substring(currentIndex + 1, length);
                    var cmd = rawMarkdown.Split(':');

                    if (cmd.Length == 2)
                    {
                        cmd[0] = cmd[0].Trim().ToUpperInvariant();
                        cmd[1] = cmd[1].Trim().ToUpperInvariant();

                        // Foreground command
                        if (cmd[0] == "F" || cmd[0] == "FORE" || cmd[0] == "FOREGROUND")
                        {
                            if (Constants.Settings.ColorMapping.TryGetValue(cmd[1], out var newColor))
                            {
                                finalForegroundColor = newColor;
                            }
                            else
                            {
                                throw new Exception("Invalid markdown exception!");
                            }
                        }
                        // Background command
                        else if (cmd[0] == "B" || cmd[0] == "BACK" || cmd[0] == "BACKGROUND")
                        {
                            if (Constants.Settings.ColorMapping.TryGetValue(cmd[1], out var newColor))
                            {
                                finalBackgroundColor = newColor;
                            }
                            else
                            {
                                throw new Exception("Invalid markdown exception!");
                            }
                        }
                    }
                    else
                    {
                        finalIIndex = currentIndex;
                    }

                    break;
            }

            return (finalIIndex, finalBackgroundColor, finalForegroundColor);
        }

    }

}
