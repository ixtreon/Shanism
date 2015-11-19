using Engine.Objects.Game;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Events;


class Teleport : Ability
{
    public Teleport()
        : base(AbilityTargetType.PointTarget)
    {
        Name = "Teleport";
        Description = "Teleports to the selected location";
        Icon = "air-burst-jade-3";

        CastRange = 10;
        ManaCost = 1;
        Cooldown = 100;
    }

    public override void OnCast(AbilityCastArgs e)
    {
        Owner.Position = e.TargetLocation;
    }
}
