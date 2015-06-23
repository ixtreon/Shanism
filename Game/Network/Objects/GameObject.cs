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
    class GameObject : IGameObject
    {
        public int Guid { get; set; }
        public Vector Location { get; set; }

        public AnimationDef Model { get; set; }

        public string Name { get; set; }

        public double Size { get; set; }
    }
}
