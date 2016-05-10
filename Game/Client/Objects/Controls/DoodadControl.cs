using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Objects;
using Shanism.Common.Game;
using Shanism.Common;

namespace Shanism.Client.Objects
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
            CanHover = true;
        }

        protected override void OnUpdate(int msElapsed)
        {
            base.OnUpdate(msElapsed);
        }

        public override void OnDraw(Graphics g)
        {
            g.Draw(Sprite, Vector.Zero, Size, Object.CurrentTint.ToColor());
        }
    }
}
 