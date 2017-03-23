using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Sprites;

namespace Shanism.Client.Assets
{
    public class IconCache
    {
        const string IconPrefix = "icons/";

        public StaticSprite Default { get; }

        readonly Dictionary<string, StaticSprite> icons = new Dictionary<string, StaticSprite>();

        public IconCache(ContentList assets)
        {
            Default = new StaticSprite(assets, "ui/default");

            foreach (var anim in assets.Animations)
                if (anim.Key.StartsWith(IconPrefix, StringComparison.Ordinal))
                    icons[anim.Key.Substring(IconPrefix.Length)] = new StaticSprite(assets, anim.Key);
        }

        public StaticSprite TryGet(string iconName)
        {
            StaticSprite s;
            if (icons.TryGetValue(iconName, out s))
                return s;
            return null;
        }
    }
}
