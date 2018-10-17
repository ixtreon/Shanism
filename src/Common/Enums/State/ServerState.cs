using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// The state of a game engine as seen by its owner.
    /// </summary>
    public enum ServerState
    {
        Stopped,
        Loading,
        LoadFailure,
        Playing,
    }
}
