using System;
using System.Collections.Generic;
using Velentr.Logging.Entry;
using Velentr.Logging.Loggers;

namespace Velentr.Logging
{
    /// <summary>
    /// A manager for storing multiple loggers
    /// </summary>
    public class LogManager : IDisposable
    {

        /// <summary>
        /// The loggers
        /// </summary>
        private Dictionary<string, Logger> loggers;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager"/> class.
        /// </summary>
        public LogManager()
        {
            loggers = new Dictionary<string, Logger>();
        }

        /// <summary>
        /// Adds the logger.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="logger">The logger.</param>
        public void AddLogger(string name, Logger logger)
        {
            loggers.Add(name, logger);
        }

        /// <summary>
        /// Removes the logger.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool RemoveLogger(string name)
        {
            return loggers.Remove(name);
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">No logger with the name [{name}] exists!</exception>
        public Logger GetLogger(string name)
        {
            if (loggers.TryGetValue(name, out var logger))
            {
                return logger;
            }

            throw new Exception($"No logger with the name [{name}] exists!");
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        public void Setup()
        {
            foreach (var log in loggers)
            {
                log.Value.Setup();
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            foreach (var log in loggers)
            {
                log.Value.Update();
            }
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="entry">The entry.</param>
        public void AddEntry(string log, string entry)
        {
            loggers[log].AddEntry(entry);
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="level">The level.</param>
        public void AddEntry(string log, string entry, LogLevel level)
        {
            loggers[log].AddEntry(entry, level);
        }

        /// <summary>
        /// Adds the dividers.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="count">The count.</param>
        public void AddDividers(string log, int count)
        {
            loggers[log].AddDividers(count);
        }

        /// <summary>
        /// Adds the dividers.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="level">The level.</param>
        /// <param name="count">The count.</param>
        public void AddDividers(string log, LogLevel level, int count)
        {
            loggers[log].AddDividers(level, count);
        }

        /// <summary>
        /// Adds the new lines.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="count">The count.</param>
        public void AddNewLines(string log, int count)
        {
            loggers[log].AddNewLines(count);
        }

        /// <summary>
        /// Adds the new lines.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="level">The level.</param>
        /// <param name="count">The count.</param>
        public void AddNewLines(string log, LogLevel level, int count)
        {
            loggers[log].AddNewLines(level, count);
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="parameters">The parameters.</param>
        public void AddEntry(string log, string entry, params object[] parameters)
        {
            loggers[log].AddEntry(entry, parameters);
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="level">The level.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="parameters">The parameters.</param>
        public void AddEntry(string log, LogLevel level, string entry, params object[] parameters)
        {
            loggers[log].AddEntry(level, entry, parameters);
        }

        /// <summary>
        /// Adds a message at Fatal log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        public void Fatal(string log, string message)
        {
            loggers[log].Fatal(message);
        }

        /// <summary>
        /// Adds a message at Fatal log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Fatal(string log, string message, params object[] parameters)
        {
            loggers[log].Fatal(message, parameters);
        }

        /// <summary>
        /// Adds a message at Error log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        public void Error(string log, string message)
        {
            loggers[log].Error(message);
        }

        /// <summary>
        /// Adds a message at Error log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Error(string log, string message, params object[] parameters)
        {
            loggers[log].Error(message, parameters);
        }

        /// <summary>
        /// Adds a message at Warning log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        public void Warning(string log, string message)
        {
            loggers[log].Warning(message);
        }

        /// <summary>
        /// Adds a message at Warning log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Warning(string log, string message, params object[] parameters)
        {
            loggers[log].Warning(message, parameters);
        }

        /// <summary>
        /// Adds a message at Info log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        public void Info(string log, string message)
        {
            loggers[log].Info(message);
        }

        /// <summary>
        /// Adds a message at Info log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Info(string log, string message, params object[] parameters)
        {
            loggers[log].Info(message, parameters);
        }

        /// <summary>
        /// Adds a message at Debug log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        public void Debug(string log, string message)
        {
            loggers[log].Debug(message);
        }

        /// <summary>
        /// Adds a message at Debug log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Debug(string log, string message, params object[] parameters)
        {
            loggers[log].Debug(message, parameters);
        }

        /// <summary>
        /// Adds a message at Trace log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        public void Trace(string log, string message)
        {
            loggers[log].Trace(message);
        }

        /// <summary>
        /// Adds a message at Trace log level.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        public void Trace(string log, string message, params object[] parameters)
        {
            loggers[log].Trace(message, parameters);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var log in loggers)
            {
                if (log.Value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            loggers.Clear();
        }

    }
}
