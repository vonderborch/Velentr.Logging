namespace Velentr.Logging.Entry
{
    /// <summary>
    /// Available logging modes
    /// </summary>
    public enum LogMode
    {
        /// <summary>
        /// Each log entry will be outputted as it is added.
        /// </summary>
        Instant = 1,

        /// <summary>
        /// Log entries will be batched and outputted in set intervals.
        /// </summary>
        Batched = 2,
    }
}
