using Client.Textures;
using Client.UI.Common;
using IO;
using IO.Common;
using IO.Objects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI
{
    class BuffBar : Control
    {
        public float BuffSize { get; set; }

        /// <summary>
        /// Gets a list of all the buffs affecting the current unit. 
        /// </summary>
        public IEnumerable<IBuffInstance> BuffList { get; private set; }


        Dictionary<IBuffInstance, BuffControl> buffDict = new Dictionary<IBuffInstance, BuffControl>();

        private int lastBuffsPerRow;
        /// <summary>
        /// Gets or sets the maximum size of the bar in terms of buffs. 
        /// </summary>
        public int BuffsPerRow
        {
            get
            {
                return (int)(Size.X / BuffSize);
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of buffs that are to be shown. 
        /// </summary>
        public int MaxBuffs { get; set; }

        /// <summary>
        /// Gets or sets whether consecutive buff rows are added to the top or the bottom. 
        /// </summary>
        public bool GrowDown { get; set; }

        /// <summary>
        /// Gets the target for the buff bar. 
        /// </summary>
        public IUnit Target { get; set; }

        public BuffBar()
        {
            this.MaxBuffs = 40;
            this.BuffSize = 0.05f;
            this.BackColor = Microsoft.Xna.Framework.Color.Pink.SetAlpha(100);
        }

        public override void Update(int msElapsed)
        {
            this.Visible = (Target != null);

            if (Visible)
            {
                //get the new buffs
                BuffList = Target.Buffs?.Take(MaxBuffs) ?? Enumerable.Empty<IBuffInstance>();

                //update the underlying controls
                buffDict.SyncValues(BuffList, addBuff);
            }
        }

        private BuffControl addBuff(IBuffInstance b)
        {
            return new BuffControl(BuffSize)
            {
                Buff = b,
                TooltipText = "some tooltip",
            };
        }


        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            if (!Visible)
                return;

            var currentPos = AbsolutePosition + new Vector(Anchor);
            var i = 0;
            var growDist = BuffSize + Anchor;

            foreach (var b in BuffList)
            {
                //get the buff's texture
                var tex = TextureCache.Get(TextureType.Icon, b.Icon);

                //draw the buff
                sb.DrawUi(tex, currentPos, new Vector(BuffSize), Color.White);

                i++;
                var isNewRow = (i % BuffsPerRow) == 0;
                if (isNewRow)
                {
                    if (GrowDown)
                        currentPos.Y += growDist;
                    else
                        currentPos.Y -= growDist;
                    currentPos.X = AbsolutePosition.X + Anchor;
                }
                else
                    currentPos.X += growDist;
            }
        }

    }
}
