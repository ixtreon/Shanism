using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;

/// <summary>
/// A simple type of unit. 
/// </summary>
public class Monster : Unit
{
    public Monster(string name, int level)
        : base(name)
    {
        this._level = level;
        BaseLife = 90 + 10 * level;
        BaseMinDamage = 10 + 2 * level;
        BaseMaxDamage = BaseMinDamage + 5;
        BaseDefense = 2 + 0.2 * level;
        BaseMoveSpeed = 5;
        BaseDodge = 10;

        this.Life = this.MaxLife;
    }

    public Monster(Monster prototype)
        : this(prototype.Name, prototype.Level)
    {
        this.Location = prototype.Location;
    }

    private int _level { get; set; }
    public override int Level
    {
        get { return _level; }
    }

    //internal override void UpdateLocation(int msElapsed)
    //{
    //    //throw new NotImplementedException();
    //}
}