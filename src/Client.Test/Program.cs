using Shanism.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ColorScheme.Current = ColorScheme.Dark;

            using (var g = new TestsGame())
                g.Run();
        }
    }
}
