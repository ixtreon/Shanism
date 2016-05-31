using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;
using Shanism.Common.Util;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.Objects
{
    public class EntityStub : ObjectStub, IEntity
    {

        public Vector Position { get; set; }

        public string Name { get; set; }

        public double Scale { get; set; }

        public double Orientation { get; set; }



        public string AnimationName { get; set; }

        public string AnimationSuffix { get; set; }

        public ShanoColor CurrentTint { get; set; }


        public EntityStub() { }

        public EntityStub(uint id)
            : base(id)
        {
        }
    }
}
