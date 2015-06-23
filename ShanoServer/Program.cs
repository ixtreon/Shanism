using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the ShanoServer. ");

            var z = new ShanoEngine(123);

            z.OpenToNetwork();

            z.Start();
        }
    }
}
