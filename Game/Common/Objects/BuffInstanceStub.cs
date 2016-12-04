using Shanism.Common.StubObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Interfaces.Objects;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.StubObjects
{
    /// <summary>
    /// Represents an empty buff as reconstructed by a network client. 
    /// </summary>
    public class BuffInstanceStub : ObjectStub, IBuffInstance
    {
        public int DurationLeft { get; set; }

        public BuffStub Prototype { get; } = new BuffStub();


        public BuffInstanceStub() { }

        public BuffInstanceStub(uint id)
            : base(id) { }

        IBuff IBuffInstance.Prototype => Prototype;
    }
}
