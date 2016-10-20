using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Util;
using Shanism.Common.Interfaces.Entities;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.StubObjects
{
    public class EntityStub : ObjectStub, IEntity
    {

        public Vector Position { get; set; }

        public string Name { get; set; }

        public double Scale { get; set; }

        public float Orientation { get; set; }



        public string Model { get; set; }

        public string Animation { get; set; }
        public bool LoopAnimation { get; set; }

        public Color CurrentTint { get; set; }


        public EntityStub() { }

        public EntityStub(uint id)
            : base(id)
        {
        }
    }
}
