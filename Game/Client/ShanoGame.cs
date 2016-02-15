using IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class ShanoGame
    {
        public static IClientInstance CreateClient(string playerName)
        {
            return new ClientInstance(playerName);
        }

        public static IClientEngine CreateClientEngine(string playerName, IGraphicsDeviceService graphics, ContentManager content)
        {
            return new ClientEngine(playerName, graphics, content);
        }
    }
}
