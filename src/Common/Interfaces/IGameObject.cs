﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Objects
{
    public interface IGameObject
    {
        /// <summary>
        /// Gets the ID of the object. 
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        ObjectType ObjectType { get; }
    }
}
