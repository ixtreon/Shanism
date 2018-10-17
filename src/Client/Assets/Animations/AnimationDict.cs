using Shanism.Common.Content;
using Shanism.Common.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client.Assets
{
    public class AnimationDict : IEnumerable<ShanoAnimation>
    {

        readonly Dictionary<string, ShanoAnimation> animations;

        public ShanoAnimation Default { get; }

        public AnimationDict(TextureDict textures, IEnumerable<AnimationDef> animations)
            : this(LoadAnimations(textures, animations)) { }

        public AnimationDict(IEnumerable<ShanoAnimation> animations)
        {
            this.animations = new Dictionary<string, ShanoAnimation>();
            foreach (var a in animations)
                this.animations[a.Name] = a;

            if (this.animations.TryGetValue("dummy", out var defaultAnimation))
                Default = defaultAnimation;
            else
                ClientLog.Instance.Warning("No default animation found. Game will probably misbehave...");
        }

        static IEnumerable<ShanoAnimation> LoadAnimations(TextureDict textures, IEnumerable<AnimationDef> animations)
        {
            foreach (var anim in animations)
            {
                anim.Name = ShanoPath.NormalizeAnimation(anim.Name);
                anim.Texture = ShanoPath.NormalizeTexture(anim.Texture);

                if(anim.Span.Area == 0)
                {
                    ClientLog.Instance.Warning($"Animation `{anim.Name}` has an invalid span of {anim.Span}. Skipping it...");
                    continue;
                }

                if (!textures.TryGet(anim.Texture, out var tex))
                {
                    ClientLog.Instance.Warning($"Missing texture `{anim.Texture}` for animation `{anim.Name}`.");
                    continue;
                }

                yield return new ShanoAnimation(anim, tex);
            }
        }

        public IEnumerator<ShanoAnimation> GetEnumerator() => animations.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => animations.Values.GetEnumerator();


        public ShanoAnimation GetOrDefault(string name)
        {
            if (animations.TryGetValue(name, out var anim))
                return anim;
            
            ClientLog.Instance.Warning($"Unable to find animation `{name}`");
            return Default;
        }

        /// <summary>
        /// Tries to get the model-animation, the model, then the default animation,
        /// in this order. 
        /// Returns whether the model/animation changed. 
        /// </summary>
        public bool TryGetChangedModel(in string curModel, in string newModel, in string newAnim, out ShanoAnimation anim)
        {
            var newModelAnim = ShanoPath.Combine(newModel, newAnim);
            return TryGetOrNull(curModel, newModelAnim, out anim)
                ?? TryGetOrNull(curModel, newModel, out anim)
                ?? GetDefault(curModel, out anim);
        }

        // true: just changed
        // false: already set
        // null: not found
        bool? TryGetOrNull(in string curModel, in string newModel, out ShanoAnimation anim)
        {
            if (curModel == newModel)
            {
                anim = null;
                return false;
            }

            if (animations.TryGetValue(newModel ?? string.Empty, out anim))
                return true;

            return null;
        }

        // true: just changed to default
        // false: already set to default
        bool GetDefault(in string curModel, out ShanoAnimation anim)
        {
            anim = Default;
            return curModel != Default.Name;
        }

        public bool TryGetValue(string animationName, out ShanoAnimation animation)
            => animations.TryGetValue(animationName, out animation);

    }
}
