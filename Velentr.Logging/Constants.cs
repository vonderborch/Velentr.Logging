using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Velentr.Logging.Entry;

namespace Velentr.Logging
{
    public sealed class Constants
    {
        /// <summary>
        /// Initializes the <see cref="Constants"/> class.
        /// </summary>
        static Constants() { }

        /// <summary>
        /// Prevents a default instance of the <see cref="Constants"/> class from being created.
        /// </summary>
        private Constants() { }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public static Constants Settings { get; } = new Constants();

        /// <summary>
        /// The log level name mapping
        /// </summary>
        public Dictionary<LogLevel, string> LogLevelNameMapping = new Dictionary<LogLevel, string>(Enum.GetValues(typeof(LogLevel)).Length)
        {
            {LogLevel.Fatal, "FATAL"},
            {LogLevel.Error, "ERROR"},
            {LogLevel.Warning, "WARNING"},
            {LogLevel.Info, "INFO"},
            {LogLevel.Debug, "DEBUG"},
            {LogLevel.Trace, "TRACE"},
            {LogLevel.Off, "OFF"},
        };

        /// <summary>
        /// The color mapping
        /// </summary>
        private Dictionary<string, ConsoleColor> _colorMapping = null;

        /// <summary>
        /// Gets the color mapping for console colors.
        /// </summary>
        /// <value>
        /// The color mapping.
        /// </value>
        public Dictionary<string, ConsoleColor> ColorMapping
        {
            get
            {
                if (_colorMapping == null)
                {
                    _colorMapping = new Dictionary<string, ConsoleColor>();
                    var colors = Enum.GetValues(typeof(ConsoleColor));

                    foreach (var color in colors)
                    {
                        _colorMapping.Add(color.ToString().ToUpperInvariant(), (ConsoleColor)color);
                    }
                }

                return _colorMapping;
            }
        }
    }
}
