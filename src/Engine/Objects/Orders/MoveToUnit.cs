using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using System.Numerics;

namespace Shanism.Engine.Objects.Orders
{
    /// <summary>
    /// A behaviour that makes the controlled unit go to the specified target unit. 
    /// </summary>
    class MoveToUnit : MoveToGround
    {
        public new Unit Target { get; set; }

        public MoveToUnit(Unit u) : base(u, Vector2.Zero) { }

        public MoveToUnit(Unit u, Unit target) : this(u)
        {
            Target = target;
        }

        public override bool TakeControl()
        {
            if (Target == null)
                return false;

            base.Target = Target.Position;
            return base.TakeControl();
        }

        public override void Update(int msElapsed)
        {
            base.Target = Target.Position;
            base.Update(msElapsed);
        }

        public override string ToString() => $"Following {Target}";
    }
}
