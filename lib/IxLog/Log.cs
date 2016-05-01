using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IxLog
{
    public enum LogLevel
    {
        None,
        Error,
        Warning,
        Info,
        Debug,
    }

    /// <summary>
    /// A simple simple log that can an output to the console, a file, or both. 
    /// Uses a shared lock to manage multiple writers to the same file. 
    /// </summary>
    public class Log
    {

        const string DateTimeFormat = "yy-MM-dd";
        //0 is name, 1 is formatted year
        const string FileNameFormat = "{0}_{1}.txt";
        //0 is timestamp, 1 is log level, 2 is message. 
        const string LogMessageFormat = "[{0}][{1}] {2}";

        /// <summary>
        /// Gets or sets the messages that will be output to the console. 
        /// Has a value of <see cref="LogLevel.Info"/> by default. 
        /// Set to <see cref="LogLevel.None"/> to disable console logging. 
        /// </summary>
        public LogLevel ConsoleLogLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// Gets or sets the messages that will be output to the file. 
        /// Has a value of <see cref="LogLevel.Debug"/> (the highest) by default. 
        /// Set to <see cref="LogLevel.None"/> to disable file logging. 
        /// </summary>
        public LogLevel FileLogLevel { get; set; } = LogLevel.Debug;

        /// <summary>
        /// Gets or sets the name of the log. 
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the file name of the log which also contains the log <see cref="Name"/> as well as the date stamp. 
        /// </summary>
        public string LogFileName { get; private set; }

        /// <summary>
        /// The object used to lock the file we write to. 
        /// </summary>
        readonly object _writerLock;


        static readonly Dictionary<string, object> _writerLocks = new Dictionary<string, object>();

        public Log(string prefix = "log", 
            LogLevel consoleLogLevel = LogLevel.Info, 
            LogLevel fileLogLevel = LogLevel.Debug)
        {
            var postfix = DateTime.Now.ToString(DateTimeFormat);
            LogFileName = $"{prefix}_{postfix}.txt";
            Name = prefix;

            ConsoleLogLevel = consoleLogLevel;
            FileLogLevel = fileLogLevel;

            //get the shared lock object for this file, or make a new one
            var fullPath = Path.GetFullPath(LogFileName);
            lock(_writerLocks)
                if (!_writerLocks.TryGetValue(fullPath, out _writerLock))
                    _writerLocks[fullPath] = (_writerLock = new object());
        }


        public void Info(string msg, params object[] args)
        {
            writeToLog(LogLevel.Info, msg, args);
        }

        public void Debug(string msg, params object[] args)
        {
            writeToLog(LogLevel.Debug, msg, args);
        }

        public void Warning(string msg, params object[] args)
        {
            writeToLog(LogLevel.Warning, msg, args);
        }

        public void Error(string msg, params object[] args)
        {
            writeToLog(LogLevel.Error, msg, args);
        }

        void writeToLog(LogLevel msgLogLevel, string msg, params object[] args)
        {
            var logLevelText = msgLogLevel.ToString().ToUpper();
            var formattedMessage = string.Format(msg, args);

            var text = string.Format(LogMessageFormat, 
                DateTime.Now.ToString("hh:mm:ss"), 
                logLevelText, 
                formattedMessage);

            if(ConsoleLogLevel >= msgLogLevel)
                Console.WriteLine(text);

            if (FileLogLevel >= msgLogLevel)
                writeToFile(LogFileName, text + Environment.NewLine);
        }

        void writeToFile(string fn, string text)
        {
            lock (_writerLock)
                File.AppendAllText(fn, text);
        }
    }
}
