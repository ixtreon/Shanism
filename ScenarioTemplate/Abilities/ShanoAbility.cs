using IO.Common;
using Engine.Events;
using Engine.Systems.Abilities;

class ShanoAbility : Ability
{
    public ShanoAbility()
    {
        // This code runs once the ability is created. 
        // Put initialization logic here. 

        TargetType = AbilityTargetType.PointTarget;

        Name = "Dummy Ability";
        Description = "Dummy Description";

        Cooldown = 1000;
        ManaCost = 5;
    }
    protected override void OnCast(AbilityCastArgs e)
    {
        // This code is executed whenever the ability is cast. 
        // The variable `e` contains useful data about the event. 
    }

    protected override void OnLearned()
    {
        // This code is executed once when the ability is given to some unit. 
        // this.Owner refers to the unit that learned the ability. 
    }

    protected override void OnUpdate(int msElapsed)
    {
        // This code is executed every frame once the ability is learned by some unit. 
        // Be careful what you put here...
    }

}
