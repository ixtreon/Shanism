using IO.Common;
using IO.Content;
using ShanoEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.ScenarioViews.Models
{
    partial class AnimationView : ScenarioView
    {
        private AnimationDef AnimationShown;

        private TextureDef SelectedTexture;

        IEnumerable<TextureViewModel> allTextures
        {
            get { return Model.Content.Textures.Values; }
        }

        public AnimationView()
        {
            InitializeComponent();
        }

        public void SetAnimation(AnimationDef anim)
        {
            Loading = true;

            AnimationShown = anim;

            //update the list of textures
            var texs = allTextures.ToArray();
            btnTextures.Items.Clear();
            btnTextures.Items.AddRange(texs);

            //see if there is a texture
            var tex = texs.FirstOrDefault(t => t.Path == anim.Texture?.Name);
            if (tex == null)
            {
                btnTextures.SelectedIndex = -1;
                textureBox.ResetTexture();
                return;
            }

            //update the dropdown
            btnTextures.SelectedItem = tex;

            //update the TextureBox, span label
            textureBox.SetTextureSpan(tex, anim.Span);
            txtSpan.Text = anim.Span.ToString();

            //update other stuff (dynamic/static anim)
            chkDynamic.Checked = AnimationShown.IsDynamic;
            chkLoop.Checked = AnimationShown.IsLooping;
            nPeriod.Value = Math.Max(nPeriod.Minimum, Math.Min(nPeriod.Maximum, AnimationShown.Period));

            Loading = false;
        }

        // An animation's texture was changed from the drop-down 
        private void btnTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Loading) return;

            var texture = btnTextures.SelectedItem as TextureViewModel;

            AnimationShown.Texture = texture.Data;
            textureBox.SetTextureSpan(texture, Rectangle.Empty);

            MarkAsChanged();
        }

        private void textureBox_SelectionChanged()
        {
            if (Loading) return;

            //model
            AnimationShown.Span = textureBox.Selection;
            txtSpan.Text = AnimationShown.Span.ToString();

            MarkAsChanged();
        }

        private void chkDynamic_CheckedChanged(object sender, EventArgs e)
        {
            if (Loading) return;

            nPeriod.Enabled =
            chkLoop.Enabled = chkDynamic.Checked;

            AnimationShown.IsDynamic = chkDynamic.Checked;

            MarkAsChanged();
        }

        private void nPeriod_ValueChanged(object sender, EventArgs e)
        {
            AnimationShown.Period = (int)nPeriod.Value;

            MarkAsChanged();
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (Loading) return;

            AnimationShown.IsLooping = chkLoop.Checked;

            MarkAsChanged();
        }
    }
}
