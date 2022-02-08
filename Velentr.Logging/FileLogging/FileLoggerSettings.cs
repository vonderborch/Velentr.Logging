using System;
using System.IO;
using System.Text;

namespace Velentr.Logging.FileLogging
{
    public struct FileLoggerSettings
    {

        private string _filePath;

        public FileLoggerSettings(string filePath, RollingType rollingType = RollingType.FileSize, TimeSpan? rollingTimeSpanInterval = null, long maxFileSizeBytes = long.MaxValue, bool appendIfFileExists = false, bool streamBased = false, Encoding encoding = null, string backupFileTimestampFormat = "dd-mm-yy_hh-mm-ss-fff", int maxBackups = Int32.MaxValue)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("FilePath must contain a valid path!");
            }

            AppendIfFileExists = appendIfFileExists;
            _filePath = filePath;
            RollingType = rollingType;
            MaxFileSizeBytes = maxFileSizeBytes;
            StreamBased = streamBased;
            RollingTimeSpanInterval = rollingTimeSpanInterval ?? TimeSpan.Zero;
            Encoding = encoding ?? Encoding.Unicode;
            BackupFileTimestampFormat = backupFileTimestampFormat;
            MaxBackups = maxBackups;

            UpdateFilePath();
        }

        public bool AppendIfFileExists { get; }

        public bool StreamBased { get; }

        public long MaxFileSizeBytes { get; set; }

        public string BackupFileTimestampFormat { get; set; }

        public int MaxBackups { get; set; }

        public Encoding Encoding { get; set; }

        public string FilePath
        {
            get => _filePath;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("FilePath must contain a valid path!");
                }
                _filePath = value;
                UpdateFilePath();
            }
        }

        public RollingType RollingType { get; }

        public TimeSpan RollingTimeSpanInterval { get; }

        private void UpdateFilePath()
        {
            var fileExists = File.Exists(FilePath);

            if (Directory.Exists(FilePath) && !fileExists)
            {
                throw new Exception("FilePath is a directory!");
            }

            if (!fileExists)
            {
                var path = Path.GetDirectoryName(FilePath);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.WriteAllText(FilePath, "", Encoding);
            }

            if (fileExists && !AppendIfFileExists)
            {
                File.WriteAllText(FilePath, "", Encoding);
            }
        }
    }
}
