using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Microsoft.Xna.Framework.Graphics;
using Client.UI;
using IO.Content;
using IO.Objects;
using IO.Common;
using Client.Assets;

namespace Client.Objects
{
    /// <summary>
    /// The in-game control for any in-game object. 
    /// </summary>
    abstract class ObjectControl : Control
    {
        /// <summary>
        /// Gets the sprite used to draw this object on the screen. 
        /// </summary>
        public readonly Sprite Sprite;
        
        /// <summary>
        /// Gets the underlying game object of this control. 
        /// </summary>
        public readonly IEntity Object;

        /// <summary>
        /// Creates a new ObjectControl for the given game object. 
        /// </summary>
        /// <param name="obj">The underlying game object. </param>
        protected ObjectControl(IEntity obj)
        {
            Object = obj;

            Sprite = Content.Sprites[obj];
            //Sprite.Tint = obj.Tint;
        }

        /// <summary>
        /// Updates the control's position and performs a sprite update. 
        /// </summary>
        /// <param name="msElapsed"></param>
        protected override void OnUpdate(int msElapsed)
        {
            //update the sprite
            Sprite.Update(msElapsed);

            //update object bounds, z-order
            var posLo = Screen.GameToUi(Object.Position - Object.Scale / 2);
            var posHi = Screen.GameToUi(Object.Position + Object.Scale / 2);
            
            this.AbsolutePosition = posLo;
            this.Size = posHi - posLo;
            ZOrder = (posHi.Y) / Screen.UiSize.Y + 0.5;

            //TODO: thiis should not be here
            ZOrder = Math.Min(1, Math.Max(0, ZOrder));

        }

    }
}
