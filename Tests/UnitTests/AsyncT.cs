using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shanism.Engine.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class AsyncTests
    {
        [TestMethod]
        public void TestNormal()
        {
            testNormal();


            Thread.Sleep(500);
            Console.WriteLine("End");
        }



        SingleThreadedSynchronizationContext stContext;

        [TestMethod]
        public void TestSpiked()
        {
            stContext = new SingleThreadedSynchronizationContext(Thread.CurrentThread);
            //SynchronizationContext.SetSynchronizationContext(stContext);

            testNormal();


            var maxTimeout = 500;
            var tStep = 10;

            while (maxTimeout > 0)
            {
                stContext.ExecutePendingWorkItems();

                Thread.Sleep(tStep);
                maxTimeout -= tStep;
            }

            Console.WriteLine("End");
        }

        public void testNormal()
        {
            var f = new Foo();


            Console.WriteLine("Start");
            for (int i = 0; i < 100; i++)
            {
                stContext.Post((_) => f.Bar(), null);
                //f.Bar();

            }
            Console.WriteLine("Done");
        }

    }

    abstract class AFoo
    {
        public abstract void Bar();
    }

    class Foo : AFoo
    {
        readonly int ticks;

        public Foo()
        {
            ticks = Environment.TickCount;
        }

        public override async void Bar()
        {
            await boo();
        }

        async Task boo()
        {
            Console.WriteLine($"s #{Thread.CurrentThread.ManagedThreadId} T+{Environment.TickCount - ticks}");

            await Task.Delay(100);

            Console.WriteLine($"e #{Thread.CurrentThread.ManagedThreadId} T+{Environment.TickCount - ticks}");
        }
    }
}
