﻿using Shanism.Common;
using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Entities
{
    partial class Unit
    {
        /// <summary>
        /// Gets the enumeration of states currently affecting the unit. 
        /// </summary>
        public StateFlags States { get; protected internal set; }

        /// <summary>
        /// Gets the base states of the unit. 
        /// </summary>
        public StateFlags BaseStates { get; protected set; } = StateFlags.None;
    }
}