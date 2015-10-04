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
            const int ptsPerIter = 1000;
            const int n_tests = 10;
            for(int i = 0; i < n_tests; i++)
                testLists(ptsPerIter * (i+1));
        }

        public void testLists(int ptsPerIter)
        {
            const int rectSz = 100;
            const int spreadSz = 10;
            const int nIters = 50;

            long[] t_set = new long[3];
            long[] t_get = new long[3];

            var rect = new Rectangle(10, 10, rectSz, rectSz);

            var sw = new Stopwatch();

            var rnd = new Random();

            Func<int, IEnumerable<Vector>> mkPts = 
                (v) => 
                Enumerable.Range(0, ptsPerIter)
                .Select(i => new Vector(rnd.NextDouble() * spreadSz, rnd.NextDouble() * spreadSz));

            Func<Action, long> timeIt = (act) =>
            {
                sw.Reset();
                sw.Start();
                act();
                sw.Stop();
                return sw.ElapsedTicks;
            };

            TestContext.WriteLine("[SET:GET], n={0}, area={1}, L then M then M2", ptsPerIter, rectSz * rectSz);

            for (int i = 0; i < nIters; i++)
            {
                var list = new List<Vector>();
                var map = new Engine.Maps.Concurrent.HashMap<Vector>(new Vector(7));
                var map2 = new HashMap<Vector>(new Vector(7));

                var pts = mkPts(i);

                t_set[0] += timeIt(() =>
                {
                    foreach (var p in pts)
                        list.Add(p);
                });

                t_set[1] += timeIt(() =>
                {
                    foreach (var p in pts)
                        map.Add(p, p);
                });

                t_set[2] += timeIt(() =>
                {
                    foreach (var p in pts)
                        map2.Add(p, p);
                });


                t_get[0] += timeIt(() =>
                {
                    var count = 0;
                    foreach (var p in list)
                        if (p.Inside(rect.Position, rect.Size))
                            count++;
                });

                t_get[1] += timeIt(() =>
                {
                    var count = map.RangeQuery(rect.Position, rect.Size).Count();
                });

                t_get[2] += timeIt(() =>
                {
                    var count = map2.RangeQuery(rect.Position, rect.Size).Count();
                });
                
            }

            for(int i = 0; i < 3; i++)
            {
                t_get[i] /= nIters;
                t_set[i] /= nIters;
            }

            TestContext.WriteLine("[{0:n0}|{1:n0}]\t[{2:n0}|{3:n0}]\t[{4:n0}|{5:n0}]", t_set[0], t_set[1], t_set[2], t_get[0], t_get[1], t_get[2]);
        }
    }
}
