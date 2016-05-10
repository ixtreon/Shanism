using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shanism.Common.Content;
using Shanism.Editor.ViewModels;
using Shanism.Common.Util;

namespace Shanism.Editor.Views
{
    partial class TexturesView : ScenarioControl
    {

        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Textures;

        TextureViewModel CurrentTexture { get; set; }

        public TexturesView()
        {
            InitializeComponent();

            texBrowser.TextureSelected += onTextureSelected;
            texBrowser.TextureCheckedChanged += onTextureCheckedChanged;
        }

        protected override async Task LoadModel()
        {
            texBrowser.SetModel(Model.Content.Textures);
        }

        void onTextureCheckedChanged(TextureViewModel tex, bool isChecked)
        {
            pTexProps.Enabled = tex.Included;
            MarkAsChanged();
        }

        void onTextureSelected(TextureViewModel texData)
        {
            pTexProps.Enabled = texData?.Included ?? false;

            if (texData == null)
            {
                pTexProps.SelectedObject = null;
                texView.Reset();
                return;
            }

            //update the right panel
            texView.SetTexture(texData);
            pTexProps.SelectedObject = texData;
        }

        void pTexProps_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var it = e.ChangedItem;

            texView.Invalidate();
            MarkAsChanged();
        }

        private void texView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && texView.Selection.Area > 0)
                texStrip.Show(Cursor.Position);

        }

        private void aDynamicAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tex = texView.Texture;
            var selection = texView.Selection;

            createSingleAnim(tex, selection, true, false);
        }

        private void aStaticAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tex = texView.Texture;
            var selection = texView.Selection;

            createSingleAnim(tex, selection, false, false);
        }

        private void multipleStaticAnimationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tex = texView.Texture;
            var selection = texView.Selection;

            //create an animation out of each cell
            foreach (var pt in selection.Iterate())
                createSingleAnim(tex, new Common.Rectangle(pt.X, pt.Y, 1, 1), false, true);

        }

        void createSingleAnim(TextureViewModel tex, Common.Rectangle selection, bool isDynamic, bool isSubAnim)
        {
            //get animation name
            var animName = isSubAnim ?
                getSubName(Model.Content.Animations, tex.Path) :
                getTexName(Model.Content.Animations, tex.Path);

            //create a static animation
            AnimationDef animDef;
            if (isDynamic)
                animDef = new AnimationDef(animName, tex.Path, selection, 100, true);
            else
                animDef = new AnimationDef(animName, tex.Path, selection);

            var anim = new AnimationViewModel(Model.Content.Textures, animDef);

            Model.Content.Animations.Add(anim);

            MarkAsChanged();
        }

        static string getSubName<T>(IReadOnlyDictionary<string, T> dict, string baseName)
        {
            var i = 1;
            string curPath = AnimPath.Combine(baseName, i.ToString());
            while (dict.ContainsKey(curPath))
                curPath = AnimPath.Combine(baseName, (++i).ToString());

            return curPath;
        }

        static string getTexName<T>(IReadOnlyDictionary<string, T> dict, string texName)
        {
            var i = 1;
            string curPath = texName;
            while (dict.ContainsKey(curPath))
                curPath = $"{texName}-{++i}";

            return curPath;
        }
    }
}
