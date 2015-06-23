using Engine.Maps;
using IO.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class HashMapPerfTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestListPerformance()
        {
            const int ptsCount = 50000;
            const int rectSz = 100;
            const int spreadSz = 1000;
            var rect = new Rectangle(10, 10, rectSz, rectSz);


            var list = new System.Collections.Concurrent.ConcurrentDictionary<Vector, bool>();
            var map = new HashMap<Vector>(new Vector(5));
            var map2 = new HashMap2<Vector>(new Vector(5));

            var sw = new Stopwatch();
            sw.Reset();

            var rnd = new Random();
            Func<int, IEnumerable<Vector>> mkPts = (v) => Enumerable.Range(0, ptsCount).Select(i => new Vector(rnd.NextDouble() * spreadSz, rnd.NextDouble() * spreadSz));

            Func<Action, long> timeIt = (act) =>
            {
                sw.Start();
                act();
                sw.Stop();
                var elapsed = sw.ElapsedTicks;
                sw.Reset();
                return elapsed;
            };

            for (int i = 0; i < 100; i++)
            {
                var pts = mkPts(i);
                var listAdd = timeIt(() =>
                {
                    foreach (var p in pts)
                        list.TryAdd(p, true);
                });

                var mapAdd = timeIt(() =>
                {
                    foreach (var p in pts)
                        map.Add(p, (Vector)p);
                });

                var map2Add = timeIt(() =>
                {
                    foreach (var p in pts)
                        map2.Add(p, (Vector)p);
                });
                TestContext.WriteLine("SET\tL: {0}ms\tM: {1}ms\tM2: {2}ms", listAdd, mapAdd, map2Add);

                var listGet = timeIt(() =>
                {
                    var count = 0;
                    foreach (var p in list)
                        if (p.Key.Inside(rect.Position, rect.Size))
                            count++;
                });

                var mapGet = timeIt(() =>
                {
                    var count = map.RangeQuery(rect.Position, rect.Size).Count();
                });
                var map2Get = timeIt(() =>
                {
                    var count = map2.RangeQuery(rect.Position, rect.Size).Count();
                });
                TestContext.WriteLine("GET\tL: {0}ms\tM: {1}ms\tM2: {2}ms", listGet, mapGet, map2Get);
            }
        }
    }
}
