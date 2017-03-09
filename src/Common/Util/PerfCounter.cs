using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Shanism.Common.Util
{
    /// <summary>
    /// Used to get a breakdown of the performance of an application.
    /// </summary>
    public class PerfCounter
    {
        public const int DefaultBarLength = 20;

        static readonly Stopwatch timer = Stopwatch.StartNew();


        readonly Dictionary<string, double> stats = new Dictionary<string, double>();

        string category;
        TimeSpan? catStart;


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
        public void Log(string category, double timeTaken)
        {
            double t;
            if (!stats.TryGetValue(category, out t))
                t = 0;
            stats[category] = t + timeTaken;
        }


        public void Start(string catName)
        {
            if (catStart != null)
                Log(category, (timer.Elapsed - catStart.Value).TotalMilliseconds);

            catStart = timer.Elapsed;
            category = catName;
        }

        public void End()
        {
            Log(category, (timer.Elapsed - catStart.Value).TotalMilliseconds);

            catStart = null;
            category = null;
        }

        public string GetPerformanceData(int barLength = DefaultBarLength)
        {
            var totalTimeTaken = stats.Sum(s => s.Value);
            var lines = stats
                .OrderByDescending(kvp => kvp.Key)
                .Select(kvp => $"{kvp.Value:000000.00} [{writeBar(kvp.Value, totalTimeTaken, barLength)}] {kvp.Key}");

            var logData = string.Join("\n", lines);

            return $"Total: {totalTimeTaken:0.00}\n{logData}";
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
