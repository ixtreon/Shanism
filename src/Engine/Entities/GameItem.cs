using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Objects.Items;
using System.Numerics;

namespace Shanism.Engine.Entities
{
    /// <summary>
    /// Represents an item that lies on the ground. 
    /// </summary>
    public class GameItem : Effect
    {
        /// <summary>
        /// Gets or sets the Item instance this GameItem contains. 
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Creates a new GameItem object containing the given item, on the point specified. 
        /// </summary>
        public GameItem(Item item, Vector2 location)
        {
            Position = location;
            this.Item = item;
        }
    }
}
