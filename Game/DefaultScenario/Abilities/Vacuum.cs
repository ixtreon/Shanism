using Engine.Events;
using Engine.Objects.Game;
using Engine.Systems;
using Engine.Systems.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Vacuum : Ability
{
    public double VacuumRange { get; set; }

    const int vacuumDuration = 300;

    const int vacuumInterval = 20;

    public int SuckInSpeed { get; set; }

    /// <summary>
    /// Creates a new ability, sets its cooldown n shit
    /// </summary>
    public Vacuum()
        : base(IO.Common.AbilityTargetType.PointTarget)
    {
        this.Cooldown = 500;
        this.ManaCost = 1;
        this.Name = "Vacuum";
        this.Description = "wtf";
        this.Icon = "lightning-1";

        SuckInSpeed = 10;
        VacuumRange = 0.3;
    }


    public async override void OnCast(AbilityCastArgs e)
    {
        var target = e.TargetLocation;
        var badUnits = Map.GetUnitsInRange(target, 3)
            .Where(u => u != Owner)
            .ToArray();

        var t = 0;
        var distPerFrame = (double)SuckInSpeed * vacuumInterval / 1000;
        foreach (var u in badUnits)
            u.ApplyState(IO.Common.UnitState.Stunned);
        while(t < vacuumDuration)
        {
            foreach(var u in badUnits)
            {
                var angle = u.Location.AngleTo(target);
                var dist = Math.Min(distPerFrame, u.Location.DistanceTo(target));
                var newPos = u.Location.PolarProjection(angle, dist);
                u.Location = newPos;
            }
            await Task.Delay(vacuumInterval);
            t += vacuumInterval;
        }
        foreach (var u in badUnits)
            u.RemoveState(IO.Common.UnitState.Stunned);


    }
}
