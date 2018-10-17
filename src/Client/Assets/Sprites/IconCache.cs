using Shanism.Client.Sprites;
using System;
using System.Collections.Generic;

namespace Shanism.Client.Assets
{
    public class IconCache
    {
        readonly Dictionary<string, Sprite> lookup = new Dictionary<string, Sprite>();

        public Sprite Default { get; }

        public IconCache(AnimationDict source, string prefix, string defaultKey)
        {
            foreach(var anim in source)
            {
                if (!anim.Name.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                var iconName = anim.Name.Substring(prefix.Length);
                var sprite = new Sprite(anim.Texture.Texture, anim.Frames[0], iconName);

                lookup[iconName] = sprite;
                if (iconName.Equals(defaultKey))
                    Default = sprite;
            }

            if (Default == null)
                ClientLog.Instance.Warning("No default icon found. Game will probably misbehave...");

        }

        public Sprite Get(string key)
            => lookup.TryGetValue(key, out var sprite) ? sprite : Default;


    }
}
