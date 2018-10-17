using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Entities;
using System.Numerics;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.ObjectStubs
{
    public abstract class EntityStub : ObjectStub, IEntity
    {
        public Vector2 Position { get; set; }

        public string Name { get; set; }

        public float Scale { get; set; }

        public float Orientation { get; set; }

        public string Model { get; set; }


        public Color CurrentTint { get; set; }
        

        public EntityStub() { }

        public EntityStub(uint id)
            : base(id)
        {
        }
    }
}
