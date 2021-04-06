using System;

namespace Velentr.Logging
{
    /// <summary>
    /// A time provider object
    /// </summary>
    public static class TimeProvider
    {
        /// <summary>
        /// The now
        /// </summary>
        public static Func<DateTime> Now = () => DateTime.Now;

        /// <summary>
        /// The UTC now
        /// </summary>
        public static Func<DateTime> UtcNow = () => Now().ToUniversalTime();

        /// <summary>
        /// Sets the date time.
        /// </summary>
        /// <param name="newDateTime">The new date time.</param>
        public static void SetDateTime(DateTime newDateTime)
        {
            Now = () => newDateTime;
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        /// <returns></returns>
        public static DateTime GetCurrentTime(TimeZoneInfo timeZone)
        {
            switch (timeZone.HasSameRules(TimeZoneInfo.Utc))
            {
                case true:
                    return UtcNow();
                default:
                    return timeZone.HasSameRules(TimeZoneInfo.Local)
                        ? Now()
                        : TimeZoneInfo.ConvertTimeFromUtc(UtcNow(), timeZone);
            }
        }

        /// <summary>
        /// Resets the date time.
        /// </summary>
        public static void ResetDateTime()
        {
            Now = () => DateTime.Now;
        }
    }
}
