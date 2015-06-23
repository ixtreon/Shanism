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
    class ObjectStub : IGameObject
    {
        public readonly int Guid;

        public Vector Location { get; internal set; }

        public AnimationDef Model { get; internal set; }

        public string Name { get; internal set; }

        public double Size { get; internal set; }

        int IGameObject.Guid {  get { return Guid; } }

        public ObjectStub(int guid)
        {
            this.Guid = guid;
        }
    }
}
