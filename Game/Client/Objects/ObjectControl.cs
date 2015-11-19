﻿using System;
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
using Client.Assets.Sprites;

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
        public readonly IGameObject Object;

        /// <summary>
        /// Gets or sets a custom location for this object. 
        /// Really hacky!
        /// </summary>
        public Vector? CustomLocation { get; set; }

        /// <summary>
        /// Creates a new ObjectControl for the given game object. 
        /// </summary>
        /// <param name="obj">The underlying game object. </param>
        public ObjectControl(IGameObject obj)
        {
            Object = obj;

            Sprite = Content.Sprites[obj];
            //Sprite.Tint = obj.Tint;
        }

        /// <summary>
        /// Updates the control's position and performs a sprite update. 
        /// </summary>
        /// <param name="msElapsed"></param>
        public override void Update(int msElapsed)
        {
            //update the sprite
            Sprite.Update(msElapsed);

            //update object position
            var loc = CustomLocation ?? Object.Position;
            var sz = new Vector(Object.Size) / 2;
            var posLo = Screen.GameToUi(loc - sz);
            var posHi = Screen.GameToUi(loc + sz);

            this.AbsolutePosition = posLo;
            this.Size = posHi - posLo;
            this.ZOrder = (int)((posHi.Y) * (Constants.Client.WindowHeight / 2) * 10);

            base.Update(msElapsed);
        }

    }
}
