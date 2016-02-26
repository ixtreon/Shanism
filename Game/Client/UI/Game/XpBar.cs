using IO;
using IO.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.Common
{
    class XpBar : ValueBar
    {
        public IHero Target { get; set; }

        public XpBar()
        {
            this.ShowText = false;
            ForeColor = Color.Purple;
            BackColor = Color.Black.SetAlpha(200);
        }

        protected override void OnUpdate(int msElapsed)
        {
            IsVisible = (Target != null);
            if(IsVisible)
            {
                Value = Target.Experience;
                MaxValue = Target.ExperienceNeeded;
                ToolTip = "Level {0}\n{1}/{2}".F(Target.Level, Target.Experience, Target.ExperienceNeeded);
            }
        }
    }
}
