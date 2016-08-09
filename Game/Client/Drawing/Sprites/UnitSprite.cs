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

        public UnitSprite(AssetList content, IUnit unit) 
            : base(content, unit)
        {
            Unit = unit;
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);

            //update unit orientation
            if (Unit.MovementState.IsMoving)
            {
                SetOrientation(Unit.MovementState.MoveDirection);
                SetAnimation("move", true);
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
                DrawDepth = 0;
                Tint = Color.Black;
            }
        }
    }
}
