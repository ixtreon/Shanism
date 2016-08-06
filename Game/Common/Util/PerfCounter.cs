using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Common.Util
{
    /// <summary>
    /// Used to get a breakdown of the performance of an application.
    /// </summary>
    public class PerfCounter
    {
        public const int DefaultBarLength = 20;

        static readonly Stopwatch Stopwatch = Stopwatch.StartNew();


        readonly Dictionary<string, long> stats = new Dictionary<string, long>();

        public IReadOnlyDictionary<string, long> Stats => stats;



        /// <summary>
        /// Resets the timings for all performance categories in the application. 
        /// </summary>
        public void Reset()
        {
            stats.Clear();
        }

        /// <summary>
        /// Logs the time taken to run the given benchmarked category. 
        /// </summary>
        public void Log(string category, long timeTaken)
        {
            long t;
            if (!stats.TryGetValue(category, out t))
                t = 0;
            stats[category] = t + timeTaken;
        }

        public void RunAndLog(string categoryName, Action act)
        {
            var st = Stopwatch.ElapsedTicks;
            act();
            var end = Stopwatch.ElapsedTicks;

            Log(categoryName, end - st);
        }

        public void RunAndLog<T>(string categoryName, Action<T> act, T arg0)
        {
            var st = Stopwatch.ElapsedTicks;
            act(arg0);
            var end = Stopwatch.ElapsedTicks;

            Log(categoryName, end - st);
        }

        public string GetPerformanceData(int barLength = DefaultBarLength)
        {
            return GetPerformanceData(stats.Sum(s => s.Value), barLength);
        }

        public string GetPerformanceData(long totalTimeTaken, int barLength = DefaultBarLength)
        {
            var lines = Stats
                .OrderByDescending(kvp => kvp.Key)
                .Select(kvp => $"{kvp.Key}  {kvp.Value:0000000}    {writeBar(kvp.Value, totalTimeTaken, barLength)}");

            var logData = string.Join("\n", lines);

            return $"Total: {totalTimeTaken}\n{logData}";
        }

        static string writeBar(double curT, double totalT, int totalLength)
        {
            if (totalT <= 0)
                return new string('-', totalLength);
            var sb = new StringBuilder();
            var pluses = (int)(curT / totalT * totalLength);

            sb.Append('+', pluses);
            sb.Append('-', totalLength - pluses);

            return sb.ToString();
        }
    }
}
