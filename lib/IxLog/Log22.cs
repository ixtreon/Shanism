using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IxLog
{
    /// <summary>
    /// A simple simple log that can an output to the console, a file, or both. 
    /// Uses a shared lock to manage multiple writers to the same file. 
    /// </summary>
    public class ShanoLog
    {
        const string DateTimeFormat = "yy-MM-dd";

        readonly TraceSource tracer;

        public string Name { get; }

        public string FilePath { get; }

        [Switch("SourceSwitch", typeof(SourceSwitch))]
        public ShanoLog(string prefix = "log")
        {
            var postfix = DateTime.Now.ToString(DateTimeFormat);
            Name = $"{prefix}_{postfix}.txt";
            FilePath = Path.GetFullPath(Name);

            tracer = new TraceSource(Name);
            tracer.Switch = new SourceSwitch("SourceSwitch")
            {
                Level = SourceLevels.Verbose
            };
            tracer.Listeners.Add(new ConsoleTraceListener
            {
                Name = "Console",
                TraceOutputOptions = TraceOptions.Timestamp,
                
            });

            tracer.Listeners.Add(new TextWriterTraceListener(FilePath)
            {
                Name = "File",
                TraceOutputOptions = TraceOptions.DateTime,
            });
        }

        public void Close()
        {
            tracer.Flush();
            tracer.Close();
        }


        public void Debug(string msg, params object[] args)
        {
            trace(TraceEventType.Verbose, string.Format(msg, args));
        }

        public void Info(string msg, params object[] args)
        {
            trace(TraceEventType.Information, string.Format(msg, args));
        }

        public void Warning(string msg, params object[] args)
        {
            trace(TraceEventType.Warning, string.Format(msg, args));
        }

        public void Error(string msg, params object[] args)
        {
            trace(TraceEventType.Error, string.Format(msg, args));
        }

        void trace(TraceEventType error, string msg)
        {
            var timestamp = DateTime.Now.ToString("hh:mm:ss");
            var line = $"[{timestamp}][{error.ToString()}] {msg}";

            foreach (TraceListener list in tracer.Listeners)
                list.WriteLine(line);
        }
    }
}
