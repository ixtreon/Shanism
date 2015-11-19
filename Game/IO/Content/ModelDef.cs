using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IO.Constants.Content;

namespace IO.Content
{
    public class ModelDef
    {
        public string Name { get; set; }

        public readonly Dictionary<string, AnimationDef> Animations = new Dictionary<string, AnimationDef>();

        #region specific animations
        /// <summary>
        /// Gets or sets the stand animation of this model. 
        /// </summary>
        [JsonIgnore]
        public AnimationDef Stand
        {
            get { return Animations.TryGet(DefaultAnimation); }
            set { Animations[DefaultAnimation] = value; }
        }

        /// <summary>
        /// Gets or sets the stand animation of this model. 
        /// </summary>
        [JsonIgnore]
        public AnimationDef Walk
        {
            get { return Animations.TryGet("walk"); }
            set { Animations["walk"] = value; }
        }

        /// <summary>
        /// Gets or sets the stand animation of this model. 
        /// </summary>
        [JsonIgnore]
        public AnimationDef Attack
        {
            get { return Animations.TryGet("attack"); }
            set { Animations["attack"] = value; }
        }

        /// <summary>
        /// Gets or sets the stand animation of this model. 
        /// </summary>
        [JsonIgnore]
        public AnimationDef Cast
        {
            get { return Animations.TryGet("cast"); }
            set { Animations["cast"] = value; }
        }
        #endregion

        public ModelDef(string name)
        {
            Name = name;
        }
    }
}
