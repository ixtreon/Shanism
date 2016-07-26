using Shanism.Engine.Objects;
using Shanism.Engine.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Serialization;
using Shanism.Engine;

namespace UnitTests
{
    [TestClass]
    public class BsDiffTests
    {

        [TestMethod]
        public void PerfTest()
        {
            const long nTestReps = 1000;
            const int nUnitReps = 1;

            var oldGuy = new Monster { Name = "Goshko", Life = 420, Mana = 420, Model = "lala" };
            var newGuy = new Monster { Name = "Troshko", Life = 420, Mana = 420, Model = "lala" };
            GameSerializer serializer = new GameSerializer();

            var oldState = Enumerable.Range(0, nUnitReps)
                .SelectMany(_ => write(serializer, oldGuy))
                .ToArray();
            var newState = Enumerable.Range(0, nUnitReps)
                .SelectMany(_ => write(serializer, newGuy))
                .ToArray();

            var sw = Stopwatch.StartNew();
            byte[] datas = new byte[0];
            for (var i = 0; i < nTestReps; i++)
            {
                using (var ms = new MemoryStream())
                {
                    BsDiffLib.BinaryPatchUtility.Create(oldState, newState, ms);
                    datas = ms.ToArray();
                }
            }

            sw.Stop();
            Console.WriteLine($"BinDiff: {nTestReps * 1000 / sw.ElapsedMilliseconds} reps/sec. ");
            Console.WriteLine($"Time Elapsed: {sw.ElapsedMilliseconds} ms. ");
            Console.WriteLine($"Input length: {newState.Length} bytes");
            Console.WriteLine($"Patch length: {datas.Length} bytes");
        }

        byte[] write<T>(GameSerializer s, T obj)
            where T : GameObject
        {
            using (var ms = new MemoryStream())
            {
                using (var w = new BinaryWriter(ms))
                    s.Write(w, obj, obj.ObjectType);

                return ms.ToArray();
            }
        }
    }
}
