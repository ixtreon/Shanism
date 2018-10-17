using Shanism.Common.Objects;

namespace Shanism.Common.ObjectStubs
{
    public class ObjectStub : IGameObject
    {
        public static readonly ObjectStub Default = new ObjectStub();
        

        public uint Id { get; }

        public virtual ObjectType ObjectType { get; }

        public ObjectStub() { }

        public ObjectStub(uint id)
        {
            this.Id = id;
        }
        
    }
}
