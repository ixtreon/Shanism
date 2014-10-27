using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;

namespace Engine
{
    public class ModelManager
    {
        private readonly Model Default;
        private readonly Dictionary<string, Model> models = new Dictionary<string, Model>();

        public Model this[string s]
        {
            get
            {
                Model m;
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
            models.Add(name, new Model(name, period));
        }

        public void Include(string name, int xFrames, int yFrames, int period = 1000)
        {
            models.Add(name, new Model(name, xFrames, yFrames, period));
        }
    }
}
