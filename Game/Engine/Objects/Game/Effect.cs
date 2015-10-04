using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO.Common;

namespace Engine.Objects.Game
{
    /// <summary>
    /// Represents an in-game special effect that has no life and can be attached to a GameObject. 
    /// 
    /// NYI
    /// </summary>
    public class Effect : GameObject
    {

        public override ObjectType ObjectType
        {
            get { return ObjectType.Effect; }
        }

        /// <summary>
        /// Gets the texture file name for this effect. 
        /// </summary>
        public string FileName;

        /// <summary>
        /// Gets or sets the speed at which this effect plays. 
        /// </summary>
        public double Speed;

        /// <summary>
        /// Gets or sets whether this effect is attached to a particular GameObject. 
        /// </summary>
        public bool IsAttached { get; set; }

        public GameObject AttachedTo { get; set; }

        public Vector Offset { get; set; }

        public Effect(GameObject anchor)
        {
            this.AttachedTo = anchor;
            this.IsAttached = true;
        }

        public Effect(Vector loc)
        {
            this.Position = loc;
        }

        
    }
}
