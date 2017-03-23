using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common;

namespace Shanism.Client.Sprites
{
    public class UnitSprite : EntitySprite
    {
        public readonly IUnit Unit;

        Vector lastPosition;

        public UnitSprite(IShanoComponent game, IUnit unit) 
            : base(game, unit)
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
                SetAnimation("move", true);
                SetOrientation(Unit.MovementState.MoveDirection);
            }
            else if (Unit.IsCasting())
            {
                SetOrientation((float)Game.Mouse.UiPosition.Angle);
                SetAnimation("attack", false);
            }
            else
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
