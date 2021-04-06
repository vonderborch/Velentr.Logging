namespace Velentr.Logging.Entry
{
    /// <summary>
    /// Log levels
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Fatal logs
        /// </summary>
        Fatal = 1,

        /// <summary>
        /// Error logs
        /// </summary>
        Error = 2,

        /// <summary>
        /// Warning logs
        /// </summary>
        Warning = 4,

        /// <summary>
        /// Info logs
        /// </summary>
        Info = 8,

        /// <summary>
        /// Debug logs
        /// </summary>
        Debug = 16,

        /// <summary>
        /// Trace logs
        /// </summary>
        Trace = 32,

        /// <summary>
        /// No logging
        /// </summary>
        Off = 64,
    }
}
