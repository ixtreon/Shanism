using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Shanism.Editor.ViewModels;
using Shanism.ScenarioLib;
using Shanism.Engine.Entities;

namespace Shanism.Editor.Views.Maps
{
    partial class PropPanel : UserControl
    {
        static readonly string DoodadType = typeof(Doodad).FullName;
        static readonly string EffectType = typeof(Effect).FullName;

        public event Action<ObjectConstructor> BrushChanged;

        ObjectConstructor constr = new ObjectConstructor
        {
            TypeName = DoodadType,
            Model = string.Empty,
            Owner = string.Empty,
            Size = 2.5f,
            Tint = Common.Color.White,
        };

        public void LoadModel(AnimationsViewModel animations)
        {
            animTree.Load(animations);
            animTree.AnimationSelected += onAnimationSelected;
        }

        void onAnimationSelected(AnimationViewModel anim)
        {
            if (anim != null)
            {
                constr.Model = anim.Name;

                BrushChanged?.Invoke(constr);
            }
        }


        void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                btnColor.BackColor = colorDialog.Color;
                constr.Tint = colorDialog.Color.ToShanoColor();

                BrushChanged?.Invoke(constr);
            }
        }


        public PropPanel()
        {
            InitializeComponent();
        }

        void btnEffect_CheckedChanged(object sender, EventArgs e)
        {
            if (btnEffect.Checked)
            {
                constr.TypeName = EffectType;

                BrushChanged?.Invoke(constr);
            }
        }

        void btnDoodad_CheckedChanged(object sender, EventArgs e)
        {
            if (btnDoodad.Checked)
            {
                constr.TypeName = DoodadType;

                BrushChanged?.Invoke(constr);
            }
        }

        void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            constr.Size = (float)btnSize.Value;

            BrushChanged?.Invoke(constr);
        }
    }
}
