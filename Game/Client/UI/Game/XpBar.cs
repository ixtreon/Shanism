using Shanism.Common;
using Shanism.Common.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Common
{
    class XpBar : ValueBar
    {
        public IHero Target { get; set; }

        public XpBar()
        {
            ShowText = true;
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
                ToolTip = $"Level {Target.Level}\n{Target.Experience}/{Target.ExperienceNeeded}";
            }

            base.OnUpdate(msElapsed);
        }
    }
}
