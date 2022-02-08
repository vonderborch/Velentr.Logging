using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using Velentr.Logging.Entry;
using Velentr.Logging.FileLogging;
using Velentr.Logging.Helpers;

namespace Velentr.Logging.Loggers
{

    /// <summary>
    /// A logger that logs messages to files
    /// </summary>
    /// <seealso cref="Velentr.Logging.Loggers.Logger" />
    /// <seealso cref="System.IDisposable" />
    public class FileLogger : Logger, IDisposable
    {

        /// <summary>
        /// The settings
        /// </summary>
        private FileLoggerSettings _settings;

        /// <summary>
        /// The stream
        /// </summary>
        private FileStream _stream;

        /// <summary>
        /// The log entries
        /// </summary>
        protected ConcurrentQueue<LogEntry> LogEntries;

        public FileLogger(string name, FileLoggerSettings? settings = null, LogLevel logLevel = LogLevel.Trace, LogMode logMode = LogMode.Instant, int batchedLoggingTimeBetweenBatchesMilliseconds = 5000, int batchedLoggingMaxNumberOfItemsPerBatch = int.MaxValue, TimeZoneInfo timeZone = null, int maxCharactersPerLine = 0) : base(name, logLevel, logMode, batchedLoggingTimeBetweenBatchesMilliseconds, batchedLoggingMaxNumberOfItemsPerBatch, timeZone, maxCharactersPerLine)
        {
            LogEntries = new ConcurrentQueue<LogEntry>();
            _settings = settings ?? new FileLoggerSettings("log.txt");
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "");
            }
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public FileLoggerSettings Settings => _settings;

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath
        {
            get => _settings.FilePath;
            set => _settings.FilePath = value;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Flush(true);
                _stream.Dispose();
                _stream = null;
            }
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

                // check if we meet the criteria to make a rolling backup of the current log file...
                var rollOver = false;
                switch (Settings.RollingType)
                {
                    case RollingType.FileSize:
                        var size = new FileInfo(FilePath).Length;
                        if (size >= Settings.MaxFileSizeBytes)
                        {
                            rollOver = true;
                        }

                        break;
                    case RollingType.PerTimespan:
                        if (TimeProvider.UtcNow() - File.GetCreationTimeUtc(FilePath) >= Settings.RollingTimeSpanInterval)
                        {
                            rollOver = true;
                        }

                        break;
                    default:
                        rollOver = false;
                        break;
                }

                // make a backup if we have to!
                if (rollOver)
                {
                    var newName = $"{Path.GetFileNameWithoutExtension(FilePath)}_{TimeProvider.GetCurrentTime(TimeZone).ToString(Settings.BackupFileTimestampFormat)}{Path.GetExtension(FilePath)}";
                    var newPath = Path.Combine(Path.GetDirectoryName(FilePath) ?? string.Empty, newName);
                    File.Copy(FilePath, newPath);

                    File.WriteAllText(FilePath, "", Settings.Encoding);

                    // find all files that match our naming scheme...
                    var baseName = Path.Combine(Path.GetDirectoryName(FilePath) ?? string.Empty, $"{Path.GetFileNameWithoutExtension(FilePath)}_");
                    var files = Directory.GetFiles(Path.GetDirectoryName(FilePath) ?? string.Empty).Where(x => x.StartsWith(baseName)).ToList();
                    if (files.Count > Settings.MaxBackups)
                    {
                        // find the oldest file and delete it...
                        var oldestFile = string.Empty;
                        var oldestFileTimestamp = DateTime.MaxValue;
                        for (var j = 0; j < files.Count; j++)
                        {
                            var time = File.GetCreationTimeUtc(files[j]);
                            if (time < oldestFileTimestamp)
                            {
                                oldestFileTimestamp = time;
                                oldestFile = files[j];
                            }

                        }

                        if (!string.IsNullOrEmpty(oldestFile))
                        {
                            File.Delete(oldestFile);
                        }
                    }
                }

                // make sure the file exists!
                if (!File.Exists(FilePath))
                {
                    File.WriteAllText(FilePath, "");
                }

                // log the entry!
                var bytes = Settings.Encoding.GetBytes($"{entry.Entry}{Environment.NewLine}");
                if (Settings.StreamBased)
                {
                    if (_stream == null)
                    {
                        _stream = File.Open(FilePath, FileMode.Append);
                    }

                    _stream.Write(bytes, 0, bytes.Length);
                }
                else
                {
                    using (var stream = File.Open(FilePath, FileMode.Append))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
        }

    }

}
