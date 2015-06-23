using IO;
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

        public override void Update(int msElapsed)
        {
            Visible = (Target != null);
            if(Visible)
            {
                Value = Target.Experience;
                MaxValue = Target.ExperienceNeeded;
                TooltipText = "Level {0}\n{1}/{2}".Format(Target.Level, Target.Experience, Target.ExperienceNeeded);
            }
        }
    }
}
