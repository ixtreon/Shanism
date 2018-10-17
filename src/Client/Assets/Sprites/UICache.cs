using Shanism.Client.Sprites;

namespace Shanism.Client.Assets
{
    public class UICache
    {
        public Sprite MenuClose { get; }
        public Sprite IconBorder { get; }
        public Sprite IconBorderHover { get; }

        public UICache(AnimationDict animations, string prefix)
        {
            Sprite Create(string name)
            {
                var fullName = $"{prefix}{name}";
                var anim = animations.TryGetValue(fullName, out var a) ? a : animations.Default;

                if (anim == null)
                    return null;

                return new Sprite(anim.Texture.Texture, anim.Frames[0], name);
            }

            MenuClose = Create("close");
            IconBorder = Create("border");
            IconBorderHover = Create("border-hover");
        }

    }
}
