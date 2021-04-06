using System;

namespace Velentr.Logging.Helpers
{
    /// <summary>
    /// Various Time helper methods.
    /// </summary>
    public static class TimeHelpers
    {

        /// <summary>
        /// Gets the milliseconds of difference between two DateTime objects
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>The milliseconds of difference</returns>
        public static int ElapsedMilliSeconds(DateTime startTime, DateTime endTime)
        {
            switch (startTime > endTime)
            {
                case true:
                    return Convert.ToInt32((startTime - endTime).TotalMilliseconds);
                case false:
                    return Convert.ToInt32((endTime - startTime).TotalMilliseconds);
            }

            return 0;
        }
    }
}
