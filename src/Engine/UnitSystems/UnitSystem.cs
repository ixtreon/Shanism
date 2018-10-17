using Shanism.Engine.Objects;
using Shanism.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// A system for a unit. 
    /// </summary>
    abstract class UnitSystem
    {
        public virtual void Update(int msElapsed) { }
    }
}
