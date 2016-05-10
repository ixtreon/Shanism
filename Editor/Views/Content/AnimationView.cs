using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Content;
using Shanism.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor.Views.Models
{
    partial class AnimationView : UserControl
    {
        public AnimationViewModel Animation { get; private set; }

        bool isLoading;

        TextureViewModel AnimationTexture => Animation.Texture;

        bool HasAnimation => Animation != null;


        public event Action AnimationChanged;

        public event Action<string> AnimationRenameRequest;


        public AnimationView()
        {
            InitializeComponent();

            SetAnimation(null, null);
        }

        public void SetAnimation(TexturesViewModel texs, AnimationViewModel anim)
        {
            Animation = anim;
            Reload(texs);
        }

        /// <summary>
        /// Refreshes all values in the view. 
        /// </summary>
        public void Reload(TexturesViewModel texs)
        {
            isLoading = true;
            {
                //update all UI elems
                foreach (Control c in Controls)
                    c.Enabled = HasAnimation;

                //update the list of textures
                if (texs != null)
                {
                    btnTextures.Items.Clear();
                    btnTextures.Items.AddRange(texs.Textures.Values.ToArray());
                }

                //update right panel
                animationBox.SetAnimation(Animation);
                textureBox.SetAnimation(Animation);

                //update left panel
                txtName.Text = Animation?.Name;
                chkDynamic.Checked = Animation?.IsDynamic ?? false;
                chkLoop.Checked = Animation?.IsLooping ?? false;
                nPeriod.Value = (Animation?.Period ?? 0)
                    .Clamp((int)nPeriod.Minimum, (int)nPeriod.Maximum);
                txtSpan.Text = Animation?.Span.ToString();

                //left panel dropdown
                if (Animation?.Texture == null)
                {
                    btnTextures.SelectedIndex = -1;
                    textureBox.Reset();
                }
                else
                    btnTextures.SelectedItem = AnimationTexture;
            }
            isLoading = false;
        }

        // An animation's texture was changed from the drop-down 
        private void btnTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            var texture = (TextureViewModel)btnTextures.SelectedItem;

            Animation.Texture = texture;
            textureBox.SetAnimation(Animation);
            animationBox.SetAnimation(Animation);

            AnimationChanged?.Invoke();
        }

        private void textureBox_SelectionChanged()
        {
            if (isLoading) return;

            //model
            Animation.SetSpan(textureBox.Selection);
            txtSpan.Text = Animation.Span.ToString();

            animationBox.SetAnimation(Animation);
            AnimationChanged?.Invoke();
        }

        //check fixed or dynamic
        void onDynamicChanged(object sender, EventArgs e)
        {
            nPeriod.Enabled =
            chkLoop.Enabled = chkDynamic.Checked;

            if (isLoading) return;

            //model
            Animation?.SetParams(isDynamic: chkDynamic.Checked);
            AnimationChanged?.Invoke();

            //view
            animationBox.SetAnimation(Animation);
        }

        //Change frame period. 
        void onPeriodChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            //model
            Animation?.SetParams(period: (int)nPeriod.Value);
            AnimationChanged?.Invoke();

            //view
            animationBox.SetAnimation(Animation);
        }

        //Changed if animation is looping
        void onLoopingChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            //model
            Animation?.SetParams(isLooping: chkLoop.Checked);
            AnimationChanged?.Invoke();

            //view
            animationBox.SetAnimation(Animation);
        }

        void txtName_TextChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            //model
            if (Animation.Name != txtName.Text)
            {
                AnimationRenameRequest?.Invoke(txtName.Text);

                txtName.Text = Animation.Name;
            }
        }
    }
}
