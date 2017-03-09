using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common.StubObjects;
using Shanism.Common.Util;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Engine.Entities
{
    /// <summary>
    /// Represents an in-game special effect that has no life and can be attached to a GameObject. 
    /// 
    /// NYI
    /// </summary>
    public class Effect : Entity, IEffect
    {
        /// <summary>
        /// Gets or sets the object this effect is attached to. 
        /// </summary>
        public Entity AttachedTo { get; set; }

        /// <summary>
        /// Gets the offset of this effect relative to the object it is attached to. 
        /// </summary>
        public Vector AttachOffset { get; set; }

        /// <summary>
        /// Gets or sets whether this effect is attached to a particular GameObject. 
        /// </summary>
        public bool IsAttached => (AttachedTo != null);

        /// <summary>
        /// Gets the object type of this effect. 
        /// Always has a value of <see cref="ObjectType.Effect"/>. 
        /// </summary>
        public override ObjectType ObjectType { get; } = ObjectType.Effect;

        /// <summary>
        /// Gets whether this effect has collision. 
        /// Always has a value of false. 
        /// </summary>
        public override bool HasCollision => false;

        /// <summary>
        /// Creates a new effect with default values. 
        /// </summary>
        public Effect() { }

        /// <summary>
        /// Creates a new effect that is attached to the given entity. 
        /// </summary>
        /// <param name="anchor">The entity to attach to. </param>
        /// <param name="attachOffset">The offset from the entity to attach the effect at. </param>
        public Effect(Entity anchor, Vector? attachOffset = null)
        {
            AttachedTo = anchor;
            AttachOffset = attachOffset ?? Vector.Zero;
        }

        /// <summary>
        /// Creates a new effect that is a clone of the given effect. 
        /// </summary>
        /// <param name="base">The effect that is to be cloned. </param>
        public Effect(Effect @base)
            : base(@base)
        {
            AttachedTo = @base.AttachedTo;
            AttachOffset = @base.AttachOffset;
        }

        internal override void Update(int msElapsed)
        {
            if (IsAttached)
                Position = AttachedTo.Position + AttachOffset;

            base.Update(msElapsed);
        }
    }
}
