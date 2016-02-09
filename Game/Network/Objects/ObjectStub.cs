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

namespace Network.Objects
{
    /// <summary>
    /// Represents an abstract empty game object as reconstructed by a network client. 
    /// </summary>
    public class ObjectStub : IGameObject
    {
        static ObjectStub()
        {
            ProtoConverter.Default.AddMappingFromTo<IGameObject, ObjectStub>();
            ProtoConverter.Default.AddMappingFromTo<IDoodad, DoodadStub>();
            ProtoConverter.Default.AddMappingFromTo<IUnit, UnitStub>();
            ProtoConverter.Default.AddMappingFromTo<IHero, HeroStub>();
        }


        public ObjectType Type { get; internal set; }

        public Vector Position { get; internal set; }
        
        public string Name { get; internal set; }

        public double Scale { get; internal set; }

        public uint Guid { get; internal set; }


        public string ModelName { get; internal set; }

        public string AnimationName { get; internal set; }

        public RectangleF Bounds { get; internal set; }

        public RectangleF TextureBounds { get; internal set; }

        public ObjectStub() { }

        public ObjectStub(uint guid)
        {
            this.Guid = guid;
        }
    }
}
