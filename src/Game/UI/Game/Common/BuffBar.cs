using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common.Entities;
using Shanism.Common.Objects;
using Shanism.Common.Util;
using System.Numerics;

namespace Shanism.Client.UI.Game
{
    class BuffBar : Control
    {
        const float DefaultBuffSize = 0.05f;
        const int DefaultMaxBuffs = 40;

        static readonly GenericComparer<IBuffInstance> buffComparer = new GenericComparer<IBuffInstance>(
            (x, y) =>
            {
                var cmpId = x.Id.CompareTo(y.Id);
                return cmpId;
            });



        readonly SortedDictionary<IBuffInstance, BuffBox> buffDict = new SortedDictionary<IBuffInstance, BuffBox>(
            buffComparer);

        public float BuffSize { get; set; } = DefaultBuffSize;

        /// <summary>
        /// Gets a list of all the buffs affecting the current unit. 
        /// </summary>
        public IEnumerable<IBuffInstance> BuffList { get; private set; }

        /// <summary>
        /// Gets or sets the maximum number of buffs that are to be shown. 
        /// </summary>
        public int MaxBuffs { get; set; } = DefaultMaxBuffs;

        /// <summary>
        /// Gets the target for the buff bar. 
        /// </summary>
        public IUnit Target { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the bar in terms of buffs. 
        /// </summary>
        public int BuffsPerRow => (int)(Size.X / BuffSize);

        public BuffBar()
        {

        }

        public override void Update(int msElapsed)
        {
            if (IsVisible = (Target?.Buffs?.Count ?? 0) > 0)
            {
                //get the new buffs
                BuffList = Target.Buffs
                    .Where(b => b.Prototype.HasIcon)
                    .Take(MaxBuffs);

                //update the underlying controls
                buffDict.SyncValues(BuffList, addBuff, removeBuff);

                var i = 0;
                var buffPos = Vector2.Zero;
                foreach(var bc in buffDict.Values)
                {
                    var x = (i % BuffsPerRow);
                    var y = (i / BuffsPerRow);
                    buffPos = new Vector2(x, y) * (BuffSize + Padding);
                    bc.Location = buffPos;
                    i++;
                }
                
                //Size = new Vector2(Size.X, buffPos.Y + BuffSize + Padding);
            }
        }

        void removeBuff(IBuffInstance b, BuffBox c)
        {
            Remove(c);
        }

        BuffBox addBuff(IBuffInstance b)
        {
            var bc = new BuffBox(b)
            {
                Size = BuffSize,
                ToolTip = $"{b.Prototype.Name}\n\n{b.Prototype.Description}",
            };
            Add(bc);
            return bc;
        }


        public override void Draw(Canvas g)
        {
            base.Draw(g);
        }

    }
}
