using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO.Common;
using IO.Objects;

namespace Engine.Entities.Objects
{
    /// <summary>
    /// Represents an in-game special effect that has no life and can be attached to a GameObject. 
    /// 
    /// NYI
    /// </summary>
    public class Effect : GameObject, IEffect
    {

        /// <summary>
        /// Gets the object this effect is attached to, if <see cref="IsAttached"/> is true. 
        /// </summary>
        public GameObject AttachedTo { get; }

        /// <summary>
        /// Gets the attachment offset of this effect relative to the object it is attached to. 
        /// </summary>
        public Vector AttachOffset { get; set; }


        /// <summary>
        /// Gets or sets whether this effect is attached to a particular GameObject. 
        /// </summary>
        public bool IsAttached { get { return AttachedTo != null; } }

        public override ObjectType Type { get { return ObjectType.Effect; } }


        public Effect(GameObject anchor)
        {
            AttachedTo = anchor;
        }

        public Effect()
        {
        }

        internal override void Update(int msElapsed)
        {
            if (IsAttached)
                Position = AttachedTo.Position + AttachOffset;

            base.Update(msElapsed);
        }
    }
}
