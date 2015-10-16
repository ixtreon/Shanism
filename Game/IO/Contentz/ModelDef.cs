using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Content
{
    public class ModelDef
    {
        public readonly string Name;

        public readonly Dictionary<string, AnimationDef> Animations = new Dictionary<string, AnimationDef>();

        #region specific animations
        /// <summary>
        /// Gets or sets the stand animation of this model. 
        /// </summary>
        public AnimationDef Stand
        {
            get { return Animations.TryGet("stand"); }
            set { Animations["stand"] = value; }
        }

        /// <summary>
        /// Gets or sets the stand animation of this model. 
        /// </summary>
        public AnimationDef Walk
        {
            get { return Animations.TryGet("walk"); }
            set { Animations["walk"] = value; }
        }

        /// <summary>
        /// Gets or sets the stand animation of this model. 
        /// </summary>
        public AnimationDef Attack
        {
            get { return Animations.TryGet("attack"); }
            set { Animations["attack"] = value; }
        }

        /// <summary>
        /// Gets or sets the stand animation of this model. 
        /// </summary>
        public AnimationDef Casting
        {
            get { return Animations.TryGet("casting"); }
            set { Animations["casting"] = value; }
        }
        #endregion

        public ModelDef(string name, AnimationDef stand,
            AnimationDef attack = null,
            AnimationDef walk = null,
            AnimationDef cast = null)
        {
            Name = name;

            Stand   = stand;
            Attack  = attack ?? stand;
            Walk    = walk ?? stand;
            Casting = cast ?? stand;
        }
    }
}
