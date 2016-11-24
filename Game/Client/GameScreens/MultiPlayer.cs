using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.UI;
using Shanism.Common;

namespace Shanism.Client.GameScreens
{
    class MultiPlayer : UiScreen
    {
        static readonly Vector panelSize = new Vector(0.6, 0.7);

        public event Action<IShanoEngine> GameStarted;

        public MultiPlayer(GraphicsDevice device)
            : base(device)
        {
            SubTitle = "Multi Player";

        }
    }
}
