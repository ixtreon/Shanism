using Shanism.Common.Objects;

namespace Shanism.Common.ObjectStubs
{
    /// <summary>
    /// Represents an empty buff as reconstructed by a network client. 
    /// </summary>
    public class BuffInstanceStub : ObjectStub, IBuffInstance
    {
        public override ObjectType ObjectType => ObjectType.BuffInstance;

        public int DurationLeft { get; set; }

        public BuffStub Prototype { get; } = new BuffStub();


        public BuffInstanceStub() { }

        public BuffInstanceStub(uint id)
            : base(id) { }

        IBuff IBuffInstance.Prototype => Prototype;
    }
}
