using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Events
{
    /// <summary>
    /// The arguments passed whenever something happens to an unit. 
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class UnitArgs : EventArgs
    {
        /// <summary>
        /// The unit that triggered the event. 
        /// </summary>
        public readonly Unit TriggerUnit;

        public UnitArgs(Unit triggerUnit)
        {
            TriggerUnit = triggerUnit;
        }
    }
}
