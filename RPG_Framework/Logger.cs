using RPG_Framework.Lib;
using System;

namespace RPG_Framework
{
    /// <summary>
    /// This class sends messages to the user
    /// </summary>
    internal class Logger
    {
        #region Properties

        private static Logger instance;
        public static Logger Instance
        {
            get
            {
                if (instance == null)
                    instance = new Logger();

                return instance;
            }
        }
        #endregion

        #region Events
        public static event EventHandler<LogEvents> MessageLogged;

        public class LogEvents : EventArgs
        {
            public string Message { get; set; }
            public int MessageDisplayTime { get; set; }
            public LogType LogType { get; set; }
        }

        /// <summary>
        /// When a message has been sent to the Output() function
        /// </summary>
        /// <param name="e">LogEvent args containing the output message</param>
        public void OnMessageLogged(LogEvents e)
        {
            EventHandler<LogEvents> handler = MessageLogged;
            if (handler != null)
                handler(this, e);
        }

        #endregion


        /// <summary>
        /// Passes message to OnMessageLogged for Event Handling.
        /// </summary>
        /// <param name="text">Message to output to user</param>
        public static void Log(string text) => Log(text, LogType.InGame);

        /// <summary>
        /// Passes message to OnMessageLogged for Event Handling.
        /// </summary>
        /// <param name="text">Message to output to user</param>
        /// <param name="logType">How the message should output to the user</param>
        public static void Log(string text, LogType logType)
        {
            LogEvents args = new LogEvents();
            args.Message = text;
            args.LogType = logType;
            Instance.OnMessageLogged(args);
        }
    }
}
