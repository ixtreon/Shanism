using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace IO.Common
{
    [ProtoContract]
    public struct Model
    {
        public static readonly Model Default = new Model("default");

        public string Name;
        public int Period;
        public Point Size;

        public Model(string name, int period = 1000)
        {
            this.Size = new Point(1, 1);
            this.Name = name;
            this.Period = period;
        }
        public Model(string name, int xFrames, int yFrames, int period = 1000)
        {
            this.Size = new Point(xFrames, yFrames);
            this.Name = name;
            this.Period = period;
        }
    }
}
