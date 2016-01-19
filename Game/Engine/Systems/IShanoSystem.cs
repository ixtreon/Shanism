using Engine.Objects;
using IO.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems
{
    /// <summary>
    /// A system that is part of the ShanoEngine. 
    /// </summary>
    abstract class ShanoSystem
    {
        /// <summary>
        /// Gets the game engine instance this system is part of. 
        /// </summary>
        public ShanoEngine Game { get; }

        public ShanoSystem(ShanoEngine game)
        {
            Game = game;
        }


        //internal abstract bool CanParse(IOMessage msg);

        //internal abstract void Parse(IOMessage msg);

        /// <summary>
        /// A global update method that gets called once per frame. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal virtual void Update(int msElapsed) { }

        /// <summary>
        /// An object update method that gets called once per game object per frame. 
        /// </summary>
        /// <param name="obj"></param>
        internal virtual void UpdateObject(int msElapsed, GameObject obj) { }
    }
}
