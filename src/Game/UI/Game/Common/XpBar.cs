using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common.Entities;

namespace Shanism.Client.UI.Game
{
    class XpBar : ValueBar
    {
        public IHero Target { get; set; }

        public XpBar()
        {
            ShowText = true;
            ForeColor = Color.Purple;
            BackColor = UiColors.ControlBackground;
        }

        public override void Update(int msElapsed)
        {
            IsVisible = (Target != null);
            if(IsVisible)
            {
                Value = Target.Experience;
                MaxValue = Target.ExperienceNeeded;
                ToolTip = $"Level {Target.Level}\n{Target.Experience}/{Target.ExperienceNeeded}";
            }

            base.Update(msElapsed);
        }
    }
}
