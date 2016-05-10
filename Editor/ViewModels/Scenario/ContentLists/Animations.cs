using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.ScenarioLib;
using Shanism.Common.Content;

namespace Shanism.Editor.ViewModels
{
    class AnimationsViewModel : IReadOnlyDictionary<string, AnimationViewModel>
    {
        readonly ContentConfig content;
        readonly TexturesViewModel textures;

        readonly Dictionary<string, AnimationViewModel> animations = new Dictionary<string, AnimationViewModel>();

        #region IReadOnlyDictionary implementation
        public AnimationViewModel this[string key] => animations[key];

        public int Count => animations.Count;

        public IEnumerable<string> Keys => animations.Keys;

        public IEnumerable<AnimationViewModel> Values => animations.Values;

        public bool ContainsKey(string key) => animations.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, AnimationViewModel>> GetEnumerator() => animations.GetEnumerator();

        public bool TryGetValue(string key, out AnimationViewModel value) => animations.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => animations.GetEnumerator();
        #endregion



        public AnimationsViewModel(ContentConfig content, TexturesViewModel textures)
        {
            this.textures = textures;
            this.content = content;
        }



        public AnimationViewModel Create(string key)
        {
            var animDef = new AnimationDef { Name = key };
            var anim = new AnimationViewModel(textures, animDef);

            Add(anim);
            return anim;
        }

        public void Add(AnimationViewModel anim)
        {
            anim.NameChanged += onAnimNameChanged;
            animations.Add(anim.Name, anim);
        }

        void onAnimNameChanged(AnimationViewModel anim, string oldName)
        {
            if (animations.ContainsKey(anim.Name))
                throw new InvalidOperationException("Animation with such name already exists!");

            if (!Remove(oldName))
                throw new InvalidOperationException("Animation was not found in the ViewModel!");

            animations.Add(anim.Name, anim);
        }

        public bool Remove(string key)
            => animations.Remove(key);

        public async Task Reload()
        {
            animations.Clear();
            foreach (var animDef in content.Animations)
                Add(new AnimationViewModel(textures, animDef));
        }

        public void Save()
        {
            content.Animations.Clear();
            foreach (var anim in animations.Values)
                anim.AddToContent(content);
        }
    }
}
