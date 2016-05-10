using Shanism.Engine.Objects.Buffs;
using Shanism.Common.Objects;
using Shanism.Common.Serialization;
using Shanism.Common.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

namespace UnitTests
{
    [TestClass]
    public class CloningPlantPerf
    {

        class Foo { public StringBuilder X { get; set; } }

        int reps = 1000000;
        Foo testObj = new Foo { X = new StringBuilder() };

        PropertyInfo pi = typeof(Foo).GetProperty("X");

        [TestMethod]
        public void PerfUnsafeCopy()
        {
            var reps = 50000;
            var buffche = new Buff
            {
                Agility = 42,
                AttackSpeedPercentage = 66,
                Name = "Buff Name Goes Here",
                Description = "Blahblah"
            };
            var stubche = new BuffInstanceStub();

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < reps; i++)
            {
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);

                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);

                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);

                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
                CloningPlant.UnsafeCopy(buffche, stubche);
            }

            sw.Stop();

            var objPerSec = (reps * 20) * 1000 / sw.ElapsedMilliseconds;
            Console.WriteLine("{0} buffs created per second! ({1} total)", objPerSec, reps * 20);
        }


        [TestMethod]
        public void PerfGetValue()
        {


            var obj = new Vector(42, 24);


            var fis = typeof(Vector).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var msDirect = getDirectValue();
            var msRefl = getReflectionValue();
            var msRefl2 = getMethodInvoke();
            var msPropCaller = getPropValue();


            Console.WriteLine("Speed for {0} reps (20 gets each):", reps);
            Console.WriteLine("Reflection: {0} ms", msRefl);
            Console.WriteLine("Delegate: {0} ms", msRefl2);
            Console.WriteLine("PropCaller: {0} ms", msPropCaller);
            Console.WriteLine("Direct: {0} ms", msDirect);
        }

        long getPropValue()
        {
            var sw = Stopwatch.StartNew();
            object retVal = null;

            //var adapter = PropertyCaller.GetInstance(typeof(Foo), "X");
            var adapter = PropertyCaller.GetInstance<Foo>("X");

            for (var i = 0; i < reps; i++)
            {
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);

                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);

                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);

                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);
                retVal = adapter.InvokeGet(testObj);

            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        long getMethodInvoke()
        {
            var sw = Stopwatch.StartNew();
            object retVal = null;

            var getterA = pi.GetGetMethod(true);
            Func<Foo, StringBuilder> getter = (Func<Foo, StringBuilder>)Delegate.CreateDelegate(typeof(Func<Foo, StringBuilder>), getterA);

            for (var i = 0; i < reps; i++)
            {
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);

                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);

                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);

                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);
                retVal = getter.Invoke(testObj);

            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        long getDirectValue()
        {
            var sw = Stopwatch.StartNew();
            object retVal = null;

            for (var i = 0; i < reps; i++)
            {
                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;

                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;

                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;

                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;
                retVal = testObj.X;
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        long getReflectionValue()
        {
            pi.GetValue(testObj);
            var sw = Stopwatch.StartNew();
            object retVal = null;

            for (var i = 0; i < reps; i++)
            {
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);

                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);

                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);

                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
                retVal = pi.GetValue(testObj);
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
    }
}
