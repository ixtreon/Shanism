using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using Engine.Objects.Entities;

namespace Engine.Objects.Entities
{
    /// <summary>
    /// Represents an item that lies on the ground. 
    /// </summary>
    class GameItem : Effect
    {
        /// <summary>
        /// Gets or sets the Item instance this GameItem contains. 
        /// </summary>
        public InventoryItem Item { get; set; }

        /// <summary>
        /// Creates a new GameItem object containing the given item, on the point specified. 
        /// </summary>
        public GameItem(InventoryItem item, Vector location)
        {
            Position = location;
            this.Item = item;
        }
    }
}
