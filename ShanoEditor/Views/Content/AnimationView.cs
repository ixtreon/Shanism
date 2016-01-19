using IO.Common;
using IO.Content;
using ShanoEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.Views.Models
{
    partial class AnimationView : ScenarioControl
    {
        AnimationViewModel AnimationModel;

        AnimationDef Animation { get { return AnimationModel.Animation; } }
        TextureViewModel AnimationTexture {  get { return AnimationModel.Texture; } }

        public AnimationView()
        {
            InitializeComponent();
        }

        public void SetAnimation(AnimationDef anim)
        {
            Loading = true;

            //update the list of textures
            var texs = Model.Content.Textures.Values.ToArray();
            btnTextures.Items.Clear();
            btnTextures.Items.AddRange(texs);

            //load up the animation. 
            //gets the texture from the model.content, if it exists
            AnimationModel = new AnimationViewModel(Model.Content, anim);
            animationBox.SetAnimation(AnimationModel);
            textureBox.SetTextureSpan(AnimationModel);

            if (AnimationModel.Texture == null)
            {
                btnTextures.SelectedIndex = -1;
                textureBox.Reset();

                Loading = false;
                return;
            }
            else
            {
                //update the dropdown
                btnTextures.SelectedItem = AnimationTexture;

                //update the span label
                txtSpan.Text = anim.Span.ToString();
            }

            //update other stuff (dynamic/static anim)
            chkDynamic.Checked = Animation.IsDynamic;
            chkLoop.Checked = Animation.IsLooping;
            nPeriod.Value = Math.Max(nPeriod.Minimum, Math.Min(nPeriod.Maximum, Animation.Period));

            Loading = false;
        }

        // An animation's texture was changed from the drop-down 
        private void btnTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Loading) return;

            var texture = (TextureViewModel)btnTextures.SelectedItem;

            AnimationModel.Texture = texture;
            textureBox.SetTextureSpan(AnimationModel);
            animationBox.SetAnimation(AnimationModel);

            MarkAsChanged();
        }

        private void textureBox_SelectionChanged()
        {
            if (Loading) return;

            //model
            Animation.Span = textureBox.Selection;
            txtSpan.Text = Animation.Span.ToString();

            animationBox.SetAnimation(AnimationModel);
            MarkAsChanged();
        }

        //check fixed or dynamic
        private void chkDynamic_CheckedChanged(object sender, EventArgs e)
        {
            nPeriod.Enabled =
            chkLoop.Enabled = chkDynamic.Checked;

            if (Loading) return;

            Animation.IsDynamic = chkDynamic.Checked;

            animationBox.SetAnimation(AnimationModel);
            MarkAsChanged();
        }

        //Change frame period. 
        private void nPeriod_ValueChanged(object sender, EventArgs e)
        {
            Animation.Period = (int)nPeriod.Value;

            animationBox.SetAnimation(AnimationModel);
            MarkAsChanged();
        }

        //Changed if animation is looping
        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (Loading) return;

            Animation.IsLooping = chkLoop.Checked;

            animationBox.SetAnimation(AnimationModel);
            MarkAsChanged();
        }
    }
}
