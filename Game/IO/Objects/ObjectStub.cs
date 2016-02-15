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

namespace IO.Objects
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

        public uint Id { get; set; }

        public ObjectType Type { get; set; }

        public Vector Position { get; set; }
        
        public string Name { get; set; }

        public double Scale { get; set; }


        public string ModelName { get; set; }

        public string AnimationName { get; set; }

        public RectangleF Bounds { get; set; }

        public RectangleF TextureBounds { get; set; }

        public ObjectStub() { }

        public ObjectStub(uint id)
        {
            this.Id = id;
        }
    }
}
