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
        private readonly AnimationDefOld Default;

        private readonly Dictionary<string, AnimationDefOld> models = new Dictionary<string, AnimationDefOld>();

        public AnimationDefOld this[string s]
        {
            get
            {
                AnimationDefOld m;
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
            models.Add(name, new AnimationDefOld(new TextureDef(fullName), period));
        }

        public void Include(string name, int xFrames, int yFrames, int period = 1000)
        {
            var fullName = TextureType.Model.GetDirectory(name);
            models.Add(name, new AnimationDefOld(new TextureDef(fullName, xFrames, yFrames), period));
        }
    }
}
