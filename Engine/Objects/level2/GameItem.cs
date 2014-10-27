using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;

namespace Engine.Objects
{
    /// <summary>
    /// Represents an item which lies on the ground. 
    /// </summary>
    public class GameItem : Doodad
    {
        /// <summary>
        /// Gets or sets the Item instance this GameItem actually is. 
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Creates a new GameItem object containing the given item, on the point specified. 
        /// </summary>
        public GameItem(Item item, Vector location)
            : base(item.Name)
        {
            this.Item = item;
            this.Location = location;

            this.Invulnerable = true;   //for now
        }
    }
}
