using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Client.Drawing
{
    class UnitVision
    {
        readonly IUnit target;

        int texSize;
        Texture2D shadowTex;

        public UnitVision(GraphicsDevice gd, IUnit target)
        {
            this.target = target;

            texSize = (int)(target.VisionRange * 4);
            shadowTex = new Texture2D(gd, texSize, texSize);
        }

        void updateTexSize()
        {
            var gd = shadowTex.GraphicsDevice;
            shadowTex.Dispose();

            texSize = (int)(target.VisionRange * 4);
            shadowTex = new Texture2D(gd, texSize, texSize);
        }


        public void Update(List<IEntity> pvs, int pid)
        {
            if (target.VisionRange * 2 > texSize)
                updateTexSize();

            //do shit
            foreach (var e in pvs)
            {
                //calc trapezoid shadow 

                //apply it to the bitmap

            }
        }
    }
}
