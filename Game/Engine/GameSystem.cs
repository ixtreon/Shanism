using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine
{
    abstract class GameSystem
    {

        internal abstract void Update(int msElapsed);
    }
}
