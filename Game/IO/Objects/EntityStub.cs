using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Content;
using IO.Objects;
using IO.Serialization;
using ProtoBuf;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace IO.Objects
{
    public class EntityStub : ObjectStub, IEntity
    {

        public Vector Position { get; set; }

        public string Name { get; set; }

        public double Scale { get; set; }



        public string AnimationName { get; set; }

        public ShanoColor CurrentTint { get; set; }


        public EntityStub() { }

        public EntityStub(uint id)
            : base(id)
        {
        }
    }
}
