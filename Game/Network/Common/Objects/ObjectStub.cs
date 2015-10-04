using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Content;

namespace Network.Objects
{
    /// <summary>
    /// Represents an abstract empty game object as reconstructed by a network client. 
    /// </summary>
    abstract class ObjectStub : IGameObject
    {
        //public readonly int Guid;

        public Vector Position { get; internal set; }

        public AnimationDef Model { get; internal set; }

        public string Name { get; internal set; }

        public double Size { get; internal set; }

        public int Guid { get; internal set; }

        public ObjectType ObjectType { get; internal set; }

        public IEnumerable<IUnit> SeenBy { get; internal set; }


        public ObjectStub(int guid)
        {
            this.Guid = guid;
        }
    }
}
