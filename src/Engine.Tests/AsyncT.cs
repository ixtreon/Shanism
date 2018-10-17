using NUnit.Framework;
using Shanism.Engine.Systems.Scripts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    public class AsyncTests
    {
        [Test]
        public void TestNormal()
        {
            testNormal();

            Thread.Sleep(500);
            Console.WriteLine("End");
        }



        SingleThreadedSynchronizationContext stContext;

        [Test]
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
