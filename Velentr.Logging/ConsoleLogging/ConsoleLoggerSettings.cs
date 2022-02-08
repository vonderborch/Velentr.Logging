using System;

namespace Velentr.Logging.ConsoleLogging
{
    /// <summary>
    /// Settings for the Console Logger
    /// </summary>
    public struct ConsoleLoggerSettings
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="applyColorsToEntries"> <c>true</c> if [color markdown will be applied to
        ///                                     entries]; otherwise, <c>false</c>. </param>
        /// <param name="backgroundColor">      (Optional) The background color. </param>
        /// <param name="foregroundColor">      (Optional) The foreground color. </param>
        public ConsoleLoggerSettings(bool applyColorsToEntries, ConsoleColor? backgroundColor = null, ConsoleColor? foregroundColor = null)
        {
            BackColor = backgroundColor ?? Console.BackgroundColor;
            ForeColor = foregroundColor ?? Console.ForegroundColor;
            ApplyColorsToEntries = applyColorsToEntries;
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>
        /// The color of the background.
        /// </value>
        public ConsoleColor BackColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the foreground.
        /// </summary>
        /// <value>
        /// The color of the foreground.
        /// </value>
        public ConsoleColor ForeColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [color markdown will be applied to entries].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [color markdown will be applied to entries]; otherwise, <c>false</c>.
        /// </value>
        public bool ApplyColorsToEntries { get; set; }
    }
}
