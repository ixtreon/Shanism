using IO.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IO.Constants.Content;

namespace IO.Content
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ModelDef
    {
        /// <summary>
        /// A placeholder model that is present in all games. 
        /// </summary>
        public static readonly ModelDef Default = new ModelDef(DefaultValues.ModelName, AnimationDef.Default);

        /// <summary>
        /// Gets or sets the name of this model. 
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public RectangleF HitBox { get; set; } = new RectangleF(0, 0, 1, 1);

        /// <summary>
        /// Gets the collection of animations keyed by their name, defined for this model. 
        /// </summary>
        [JsonProperty]
        public readonly Dictionary<string, AnimationDef> Animations = new Dictionary<string, AnimationDef>();

        #region Default Animations
        /// <summary>
        /// Gets or sets the stand animation of this model. 
        /// </summary>
        [JsonIgnore]
        public AnimationDef Stand
        {
            get { return Animations.TryGet(DefaultValues.Animation); }
            set { Animations[DefaultValues.Animation] = value; }
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

        [JsonConstructor]
        private ModelDef() { }

        public ModelDef(string name)
        {
            Name = name;
        }

        public ModelDef(string name, AnimationDef standAnim)
        {
            Name = name;
            Animations[DefaultValues.Animation] = standAnim;
        }
    }
}
