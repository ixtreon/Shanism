using Engine.Objects;
using Engine.Objects.Game;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Events
{
    public class AbilityCastArgs
    {
        //public readonly Unit TargetUnit;

        public readonly object Target;

        public readonly AbilityTargetType TargetType;

        public Unit TargetUnit
        {
            get { return Target as Unit; }
        }

        public Vector TargetLocation
        {
            get
            {
                return TargetUnit?.Position ?? ((Vector)Target);
            }
        }

        /// <summary>
        /// Gets or sets whether the spell cast was successful. True by default. 
        /// </summary>
        public bool Success = true;


        public AbilityCastArgs(Ability a, object target)
        {
            TargetType = a.TargetType;

            Target = target;
        }
    }

}
