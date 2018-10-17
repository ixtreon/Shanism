using Ix.Math;
using Shanism.Common.Content;
using System;
using System.Collections.Generic;

namespace IconPacker
{
    class ContentBlob
    {
        public TextureDef Texture { get; set; }

        public IReadOnlyCollection<AnimationDef> Animations { get; set; }

        [Newtonsoft.Json.JsonConstructor]
        public ContentBlob() { }

        public ContentBlob(string textureName, Point logicalSize, Func<TextureDef, IReadOnlyCollection<AnimationDef>> animationFactory = null)
            : this(new TextureDef(textureName, logicalSize), animationFactory)
        {
            // nothing here
        }

        public ContentBlob(TextureDef texture, IReadOnlyCollection<AnimationDef> animations)
            : this(texture, _ => animations)
        {
            // nothing here
        }

        public ContentBlob(TextureDef texture, Func<TextureDef, IReadOnlyCollection<AnimationDef>> animationFactory = null)
        {
            Texture = texture ?? throw new ArgumentNullException(nameof(texture));
            Animations = animationFactory?.Invoke(texture) ?? new AnimationDef[0];
        }
    }
}
