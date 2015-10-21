using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class ShanoGame
    {
        public static IShanoClient Create(string playerName)
        {
            return new MainGame(playerName);
        }
    }
}
