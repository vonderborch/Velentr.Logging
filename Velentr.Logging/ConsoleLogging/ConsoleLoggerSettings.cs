using System;

namespace Velentr.Logging.ConsoleLogging
{
    /// <summary>
    /// Settings for the Console Logger
    /// </summary>
    public struct ConsoleLoggerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLoggerSettings"/> struct.
        /// </summary>
        /// <param name="applyColorsToEntries">if set to <c>true</c> [color markdown will be applied to entries]. Defaults to false.</param>
        /// <param name="backgroundColor">Color of the background. Defaults to the Console's current Background Color.</param>
        /// <param name="foregroundColor">Color of the foreground. Defaults to the Console's current Foreground Color.</param>
        public ConsoleLoggerSettings(bool applyColorsToEntries = false, ConsoleColor? backgroundColor = null, ConsoleColor? foregroundColor = null)
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
