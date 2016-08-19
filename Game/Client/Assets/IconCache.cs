using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Drawing
{
    class IconCache
    {
        const string IconPrefix = "icons/";

        public IconSprite IconBorder { get; }
        public IconSprite IconBorderHover { get; }
        public IconSprite Default { get; }

        readonly Dictionary<string, IconSprite> icons = new Dictionary<string, IconSprite>();

        public IconCache(AssetList assets)
        {
            IconBorder = new IconSprite(assets, "ui/border");
            IconBorderHover = new IconSprite(assets, "ui/border-hover");
            Default = new IconSprite(assets, "ui/default");

            foreach (var anim in assets.Animations)
                if (anim.Key.StartsWith(IconPrefix, StringComparison.Ordinal))
                    icons[anim.Key.Substring(IconPrefix.Length)] = new IconSprite(assets, anim.Key);
        }

        internal IconSprite TryGet(string iconName)
        {
            IconSprite s;
            if (icons.TryGetValue(iconName, out s))
                return s;
            return null;
        }
    }
}
