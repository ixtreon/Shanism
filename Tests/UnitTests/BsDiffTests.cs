using Shanism.Engine.Objects;
using Shanism.Engine.Objects.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class BsDiffTests
    {
        [TestMethod]
        public void PerfTest()
        {
            const long count = 1000;

            var oa = new Monster { Name = "Goshko", Life = 420, Mana = 420, ModelName = "lala", AnimationSuffix = "blala" };
            var ob = new Monster { Name = "Troshko", Life = 420, Mana = 420, ModelName = "lala", AnimationSuffix = "blala" };

            var da = Enumerable.Range(0, 100).SelectMany(_ => Shanism.Engine.Serialization.ShanoReader.QuickSerialize(oa)).ToArray();
            var db = Enumerable.Range(0, 100).SelectMany(_ => Shanism.Engine.Serialization.ShanoReader.QuickSerialize(ob)).ToArray();

            var sw = Stopwatch.StartNew();
            byte[] datas = new byte[0];
            for (var i = 0; i < count; i++)
            {
                using (var ms = new MemoryStream())
                {
                    BsDiffLib.BinaryPatchUtility.Create(da, db, ms);
                    datas = ms.ToArray();
                }
            }

            sw.Stop();
            Console.WriteLine($"BinDiff: {count * 1000 / sw.ElapsedMilliseconds} reps/sec. ");
            Console.WriteLine($"Time Elapsed: {sw.ElapsedMilliseconds} ms. ");
            Console.WriteLine($"Input length: {da.Length + db.Length} ({da.Length} + {db.Length}) bytes");
            Console.WriteLine($"Patch length: {datas.Length} bytes");
        }
    }
}
