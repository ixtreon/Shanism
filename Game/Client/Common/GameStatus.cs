using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// The states a game client can be in. 
    /// 
    /// Should really live in the IO project. 
    /// </summary>
    public enum GameStatus
    {
        Loading,
        Playing,
        Observing,
    }
}
