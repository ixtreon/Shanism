using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.GameScreens
{
    abstract class GameScreen
    {

        public abstract void Draw();

        public abstract void Update(int msElapsed);

        public virtual void Shown() { }
    }
}
