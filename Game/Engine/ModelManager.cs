using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Content;
using IO;

namespace Engine
{
    /// <summary>
    /// Specifies all textures used within the game, their frames and shit.
    /// </summary>
    public class ModelManager
    {
        private readonly AnimationDef Default;

        private readonly Dictionary<string, AnimationDef> models = new Dictionary<string, AnimationDef>();

        public AnimationDef this[string s]
        {
            get
            {
                AnimationDef m;
                if (models.TryGetValue(s, out m))
                    return m;
                return Default;
            }
        }

        public ModelManager()
        {
            this.Include("default");
            Default = this["default"];
        }

        public void Include(string name, int period = 1000)
        {
            var fullName = TextureType.Model.GetDirectory(name);
            models.Add(name, new AnimationDef(new TextureDef(fullName), period));
        }

        public void Include(string name, int xFrames, int yFrames, int period = 1000)
        {
            var fullName = TextureType.Model.GetDirectory(name);
            models.Add(name, new AnimationDef(new TextureDef(fullName, xFrames, yFrames), period));
        }
    }
}
