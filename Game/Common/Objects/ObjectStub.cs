using Shanism.Common.Interfaces.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.Objects
{
    public class ObjectStub : IGameObject
    {
        public uint Id { get; set; }

        public ObjectType ObjectType { get; set; }

        public ObjectStub() { }

        public ObjectStub(uint id)
        {
            this.Id = id;
        }
    }
}
