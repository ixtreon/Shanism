using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Entities;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


namespace Shanism.Common.ObjectStubs
{
    public class DoodadStub : EntityStub, IDoodad
    {
        public override ObjectType ObjectType => ObjectType.Doodad;

        public DoodadStub() { }

        public DoodadStub(uint guid)
            : base(guid)
        {

        }
    }
}
