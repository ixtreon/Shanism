using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Util;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common;

namespace Shanism.Client.Drawing
{
    class UnitSprite : EntitySprite
    {
        public readonly IUnit Unit;

        Vector lastPosition;

        public UnitSprite(ContentList content, IUnit unit) 
            : base(content, unit)
        {
            Unit = unit;
            lastPosition = unit.Position;
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);

            //update unit orientation
            if (Unit.MovementState.IsMoving)
            {
                var curPosition = Unit.Position;
                var ang = lastPosition.AngleTo(curPosition);
                var dist = curPosition.DistanceTo(lastPosition) * 1000 / msElapsed;

                if (dist > Unit.Stats[UnitStat.MoveSpeed] / 10)
                {
                    SetOrientation((float)ang);
                    SetAnimation("move", true);

                    lastPosition = Unit.Position;
                }
                else
                    SetAnimation(string.Empty, true);
            }
            else if (Unit.IsCasting())
            {
                SetOrientation((float)Input.MouseInfo.UiPosition.Angle);
                SetAnimation("attack", false);
            }
            else if (AnimationName == "move")
                SetAnimation(string.Empty, true);

            //tint black if dead
            if (Unit.IsDead)
            {
                DrawDepth = 1e-5f;
                Tint = Color.Black;
            }
        }
    }
}
