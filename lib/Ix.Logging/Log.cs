using System;
using System.Collections.Concurrent;
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
        static string GetFileName(string name)
            => $"{name}_{DateTime.Now:yy-MM-dd}.txt";

        static string GetLine(LogLevel level, string message)
            => $"[{DateTime.Now:HH:mm:ss}][{level.ToString().ToUpper()}] {message}";


        static ConcurrentDictionary<string, object> WriterLocks { get; } = new ConcurrentDictionary<string, object>();


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
        /// The object used to lock the file we use. 
        /// </summary>
        readonly object _writerLock;



        public Log(string name = "log")
        {
            Name = name;
            FileName = GetFileName(name);

            _writerLock = WriterLocks.GetOrAdd(name, _ => new object());
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
            var entry = GetLine(msgLogLevel, msg);

            if (ConsoleLogLevel >= msgLogLevel)
                Console.WriteLine(entry);

            if (FileLogLevel >= msgLogLevel)
            {
                lock (_writerLock)
                    File.AppendAllText(FileName, entry + Environment.NewLine);
            }
        }

    }
}
