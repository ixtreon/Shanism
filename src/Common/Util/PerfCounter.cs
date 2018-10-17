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
        public const int BarLength = 20;


        readonly Dictionary<string, double> stats = new Dictionary<string, double>();

        PerfNode? curItem;
        double totalMsTaken;

        struct PerfNode
        {
            public long Start { get; }
            public string Name { get; }

            public PerfNode(long start, string name)
            {
                Start = start;
                Name = name;
            }

            public long CalcDeltaT(long now)
                => 1000 * (now - Start) / Stopwatch.Frequency;
        }


        public double this[string category]
        {
            get => stats[category];
        }

        public string Name { get; }

        public PerfCounter(string name)
        {
            Name = name;
        }


        /// <summary>
        /// Resets the timings for all performance categories in the application. 
        /// </summary>
        public void Reset()
        {
            stats.Clear();
            totalMsTaken = 0;
        }


        public void Start(string catName)
        {
            var t = Stopwatch.GetTimestamp();
            
            if(curItem != null)
                end(t);

            curItem = new PerfNode(t, catName);
        }

        /// <summary>
        /// Logs the time taken to run the given benchmarked category. 
        /// </summary>
        void end(long now)
        {
            if(curItem == null)
                throw new InvalidOperationException($"Start was not called!");

            var n = curItem.Value;
            if(!stats.TryGetValue(n.Name, out var oldT))
                oldT = 0;
            var newT = n.CalcDeltaT(now);

            stats[n.Name] = oldT + newT;
            totalMsTaken += newT;

            curItem = null;
        }

        public void End() => end(Stopwatch.GetTimestamp());

        public string GetPerformanceData(float timePeriod)
        {
            var sb = new StringBuilder();

            sb.Append($"# {Name}: {(totalMsTaken / timePeriod * 100):00.00}%");

            sb.AppendLine();

            foreach(var kvp in stats.OrderBy(kvp => kvp.Key))
            {
                var name = kvp.Key;
                var percTotalPeriod = kvp.Value / timePeriod * 100;
                var percThisInstance = kvp.Value / totalMsTaken * 100;

                sb.Append($"{percTotalPeriod:00.000}%");
                sb.Append(' ');
                writeBar(sb, percThisInstance);
                sb.Append(' ');
                sb.Append(name);

                sb.AppendLine();
            }

            return sb.ToString();
        }

        void writeBar(StringBuilder sb, double t)
        {
            if(totalMsTaken > 0)
            {
                var nPluses = (int)(t * BarLength / 100);

                sb.Append(" [");
                sb.Append('!', nPluses);
                sb.Append('.', BarLength - nPluses);
                sb.Append("] ");
            }
        }
    }
}
