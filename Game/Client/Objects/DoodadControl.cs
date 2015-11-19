using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Objects;
using IO.Common;

namespace Client.Objects
{
    /// <summary>
    /// Represents the in-game control of a specific doodad. 
    /// </summary>
    class DoodadControl : ObjectControl
    {
        /// <summary>
        /// Gets the doodad associated with this DoodadControl. 
        /// </summary>
        public IDoodad Doodad
        {
            get { return (IDoodad)Object; }
        }


        public DoodadControl(IDoodad d)
            : base(d)
        {
            ClickThrough = true;
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }

        public override void Draw(Graphics g)
        {
            g.Draw(Sprite, Vector.Zero, Size);
        }
    }
}
 