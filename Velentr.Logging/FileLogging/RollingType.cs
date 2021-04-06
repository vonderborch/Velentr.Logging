namespace Velentr.Logging.FileLogging
{
    /// <summary>
    /// Types of file rollovers/backup schemes that are available
    /// </summary>
    public enum RollingType
    {
        /// <summary>
        /// No file rolling/backups
        /// </summary>
        None = 1,

        /// <summary>
        /// Files are rolled over based on a time interval
        /// </summary>
        PerTimespan = 2,

        /// <summary>
        /// Files are rolled over based on their size
        /// </summary>
        FileSize = 4,
    }
}
