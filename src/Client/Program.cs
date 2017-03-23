using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Client
{
    class Program
    {
        static bool isRunning = true;

        static int Main(string[] args)
        {
            var g = new ClientGame();
            g.Run();

            Environment.Exit(0);        //fix for MonoGame GL failing to stop all processes
            return 0;
        }
    }
}
