using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    /// <summary>
    /// A generic struct for the game.
    /// Contains screen data, content list.
    /// Should also contain keyboard, mouse.
    /// </summary>
    class GameStruct
    {
        public ContentList Content { get; }

        public Screen Screen { get; }
    }
}
