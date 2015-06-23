using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using Engine.Objects.Game;

namespace Engine.Objects.Game
{
    /// <summary>
    /// Represents an item which lies on the ground. 
    /// </summary>
    public class GameItem : Doodad
    {
        /// <summary>
        /// Gets or sets the Item instance this GameItem contains. 
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Creates a new GameItem object containing the given item, on the point specified. 
        /// </summary>
        public GameItem(Item item, Vector location)
            : base("item", location)
        {
            this.Item = item;

            this.Invulnerable = true;   //for now
        }
    }
}
