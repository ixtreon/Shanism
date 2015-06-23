using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;

namespace Network
{
    enum LogLevel
    {
        Info, Debug, Warning, Error
    }

    static class Log
    {

        public static void Info(string msg, params object[] args)
        {
            writeLog(LogLevel.Info, msg.Format(args));
        }

        public static void Debug(string msg, params object[] args)
        {
            writeLog(LogLevel.Debug, msg.Format(args));
        }

        public static void Warning(string msg, params object[] args)
        {
            writeLog(LogLevel.Warning, msg.Format(args));
        }

        public static void Error(string msg, params object[] args)
        {
            writeLog(LogLevel.Error, msg.Format(args));
        }

        private static void writeLog(LogLevel type, string msg)
        {
            var typeText = type.ToString().ToUpper();
            var text = "[{0}][{1}] {2}".Format((object)DateTime.Now.ToString("hh:mm:ss"), typeText, msg);

            Console.WriteLine(text);
        }
    }
}
