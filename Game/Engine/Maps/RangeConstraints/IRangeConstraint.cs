using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps
{
    interface IRangeConstraint
    {
        /// <summary>
        /// The range events this constraint responds to. 
        /// </summary>
        EventType ConstraintType { get; }

        /// <summary>
        /// Gets the type of the second object in the constraint. 
        /// </summary>
        OriginType OriginType { get; }


        object Origin { get; }

        void Check(GameObject triggerObject);
    }
}
