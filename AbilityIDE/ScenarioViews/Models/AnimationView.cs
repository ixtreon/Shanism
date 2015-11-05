using IO.Common;
using IO.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbilityIDE.ScenarioViews.Models
{
    partial class AnimationView : UserControl
    {
        private AnimationDef AnimationShown;

        private TextureDef SelectedTexture;
        private bool _loading;

        public event Action<AnimationDef> AnimationChanged;

        public AnimationView()
        {
            InitializeComponent();
        }


        public void SetAnimation(AnimationDef anim, IEnumerable<TextureDef> allTextures)
        {
            _loading = true;
            AnimationShown = anim;

            btnTextures.Items.Clear();
            btnTextures.Items.AddRange(allTextures.ToArray());

            //try to get the selected texture
            var texId = -1;
            if(anim.Texture != null)
                texId = btnTextures.Items.IndexOf(anim.Texture);

            //load if found
            if (texId == -1)
                btnTextures.SelectedItem = null;
            else
                btnTextures.SelectedIndex = texId;

            //ui-texview
            texView.SetModel(anim);

            nSpanX.Value = AnimationShown.Span.X;
            nSpanY.Value = AnimationShown.Span.Y;
            nSpanW.Value = AnimationShown.Span.Width;
            nSpanH.Value = AnimationShown.Span.Height;

            validate();
            _loading = false;
        }

        private void validate()
        {
            //set SelectedTexture
            SelectedTexture = btnTextures.SelectedItem as TextureDef;
            var hasTexture = SelectedTexture != null;

            //enable/disable UI elements if texture or not
            nSpanH.Enabled = nSpanW.Enabled =
            nSpanX.Enabled = nSpanY.Enabled =
            nPeriod.Enabled = hasTexture;

            if (hasTexture)
            {
                //restrain span x/y, w/h
                nSpanX.Value = Math.Min(SelectedTexture.Splits.X - 1, nSpanX.Value);
                nSpanY.Value = Math.Min(SelectedTexture.Splits.Y - 1, nSpanY.Value);

                nSpanW.Value = Math.Max(1, Math.Min(SelectedTexture.Splits.X - nSpanX.Value,
                    nSpanW.Value));
                nSpanH.Value = Math.Max(1, Math.Min(SelectedTexture.Splits.Y - nSpanY.Value,
                    nSpanH.Value));
            }
        }

        void saveToModel(Rectangle? span = null, TextureDef texture = null)
        {
            if (_loading)
                return;
            _loading = true;

            validate();

            if(span.HasValue)
            {
                //ui-boxes
                nSpanX.Value = texView.Selection.X;
                nSpanY.Value = texView.Selection.Y;
                nSpanW.Value = texView.Selection.Width;
                nSpanH.Value = texView.Selection.Height;
                //model
                AnimationShown.Span = span.Value;
                //ui-texview
                texView.SetSelection(span.Value, false);    //don't raise the event to update nSpans
            }

            if (texture != null)
            {
                texView.SetModel(texture);
                AnimationShown.Texture = texture;
            }

            AnimationChanged?.Invoke(AnimationShown);
            _loading = false;
        }

        private void btnTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveToModel(texture: btnTextures.SelectedItem as TextureDef);
        }

        private void nSpan_ValueChanged(object sender, EventArgs e)
        {
            saveToModel(new Rectangle(
                    (int)nSpanX.Value, (int)nSpanY.Value,
                    (int)nSpanW.Value, (int)nSpanH.Value));
        }

        private void texView_SelectionChanged()
        {
            saveToModel(span: texView.Selection);
        }
    }
}
