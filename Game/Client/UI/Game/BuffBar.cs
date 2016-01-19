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


        SortedDictionary<IBuffInstance, BuffControl> buffDict = new SortedDictionary<IBuffInstance, BuffControl>(new BuffComparer());

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
            this.BackColor = Color.Black.SetAlpha(100);
        }

        protected override void OnUpdate(int msElapsed)
        {
            this.Visible = (Target != null);

            if (Visible)
            {
                //get the new buffs
                BuffList = Target.Buffs?.Take(MaxBuffs) ?? Enumerable.Empty<IBuffInstance>();

                //update the underlying controls
                buffDict.SyncValues(BuffList, addBuff, removeBuff);

                var i = 0;
                foreach(var bc in buffDict.Values)
                {
                    var x = (i % BuffsPerRow);
                    var y = (i / BuffsPerRow);
                    bc.Location = new Vector(x, y) * (BuffSize + Padding);
                    i++;
                }
            }
        }

        void removeBuff(IBuffInstance b, BuffControl c)
        {
            Remove(c);
        }

        private BuffControl addBuff(IBuffInstance b)
        {
            var bc = new BuffControl
            {
                Size = BuffSize,
                Buff = b,
                ToolTip = b.Name + "\n\n" + b.Description,
            };
            Add(bc);
            return bc;
        }


        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);
        }

    }
}
