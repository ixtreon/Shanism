using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ix.Logging
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
        static string fileName(string name)
            => $"{name}_{DateTime.Now:yy-MM-dd}.txt";

        static string formatLine(string level, string message)
            => $"[{DateTime.Now:hh:mm:ss}][{level.ToUpper()}] {message}";


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
        /// Gets the friendly name of the log. 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the file name of the log. 
        /// </summary>
        public string FileName { get; }


        public string FilePath => Path.GetFullPath(FileName);

        /// <summary>
        /// The object used to lock the file we write to. 
        /// </summary>
        readonly object _writerLock;


        static readonly Dictionary<string, object> _writerLocks = new Dictionary<string, object>();


        public Log(string name = "log", 
            LogLevel consoleLogLevel = LogLevel.Info, 
            LogLevel fileLogLevel = LogLevel.Debug)
        {
            Name = name;
            FileName = fileName(name);

            ConsoleLogLevel = consoleLogLevel;
            FileLogLevel = fileLogLevel;

            //get the shared lock object for this file, or make a new one
            lock (_writerLocks)
                if (!_writerLocks.TryGetValue(FilePath, out _writerLock))
                    _writerLocks[FilePath] = (_writerLock = new object());
        }


        public void Info(string msg)
            => Write(LogLevel.Info, msg);

        public void Debug(string msg)
            => Write(LogLevel.Debug, msg);

        public void Warning(string msg)
            => Write(LogLevel.Warning, msg);

        public void Error(string msg)
            => Write(LogLevel.Error, msg);

        public void Write(LogLevel msgLogLevel, string msg)
        {
            var entry = formatLine(msgLogLevel.ToString(), msg);

            if (ConsoleLogLevel >= msgLogLevel)
                Console.WriteLine(entry);

            if (FileLogLevel >= msgLogLevel)
            {
                lock (_writerLock)
                    File.AppendAllText(FileName, entry + Environment.NewLine);
            }
        }


        void writeToFile(string fn, string text)
        {
            lock (_writerLock)
                File.AppendAllText(fn, text);
        }
    }
}
