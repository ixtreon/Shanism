using Engine.Entities;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;

namespace Engine.Systems.Buffs
{
    /// <summary>
    /// Represents an instance of a buff applied to a unit. 
    /// </summary>
    public class BuffInstance : Buff, IBuffInstance
    {
        /// <summary>
        /// Gets the buff prototype of this instance. 
        /// </summary>
        public Buff Prototype { get; }

        /// <summary>
        /// Gets the target unit this buff instance is applied to. 
        /// </summary>
        public Unit Target { get; }

        /// <summary>
        /// Gets the duration left, in milliseconds, before this buff instance expires. 
        /// </summary>
        public int DurationLeft { get; private set; }

        /// <summary>
        /// Gets whether this buff should be removed from its target unit. 
        /// </summary>
        public bool ShouldDestroy
        {
            get { return Prototype.IsTimed && DurationLeft <= 0; }
        }


        public BuffInstance(Buff buff, Unit target)
            : base(buff)
        {
            Prototype = buff;
            Target = target;
            DurationLeft = buff.FullDuration;

            Prototype.OnApplied(this);
        }



        internal override void Update(int msElapsed)
        {
            if (Prototype.IsTimed)
            {
                DurationLeft -= msElapsed;
                if (ShouldDestroy)
                {
                    Prototype.OnExpired(this);
                    return;
                }
            }

            Prototype.OnUpdate(this);
        }

        /// <summary>
        /// Refreshes the duration of this buff instance. 
        /// </summary>
        public void RefreshDuration()
        {
            DurationLeft = Prototype.FullDuration;
            Prototype.OnRefresh(this);
        }
       
    }
}
