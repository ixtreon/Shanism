using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO.Performance
{
    /// <summary>
    /// Provides a breakdown of the performance of an application.. 
    /// </summary>
    public class PerfCounter
    {
        public static readonly PerfCounter Default = new PerfCounter();

        readonly ConcurrentDictionary<string, SectionCounter> stats = new ConcurrentDictionary<string, SectionCounter>();

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
            stats.AddOrUpdate(category,
                (_) => new SectionCounter(timeTaken),
                (_, pc) => pc.Add(timeTaken));
        }

        public void WrapLogger(string category, Action func)
        {
            var sw = Stopwatch.StartNew();
            func();
            sw.Stop();

            Log(category, sw.ElapsedTicks);
        }

        public IEnumerable<KeyValuePair<string, long>> Stats
        {
            get { return stats.Select(kvp => new KeyValuePair<string, long>(kvp.Key, kvp.Value.TotalElapsed)); }
        }

        struct SectionCounter
        {
            long _totalElapsed;

            public long TotalElapsed { get { return _totalElapsed; } }

            public SectionCounter(long msElapsed) { _totalElapsed = msElapsed; }

            public void Reset()
            {
                _totalElapsed = 0;
            }

            public SectionCounter Add(long msElapsed)
            {
                Interlocked.Add(ref _totalElapsed, msElapsed);
                return this;
            }
        }
    }
}
