using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ShanoRpgClient
    {
        public static void Start(string playerName, Action<int> localServerUpdate)
        {
            var g = new MainGame(playerName, localServerUpdate);
        }
    }
}
