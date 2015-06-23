using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Objects;
using IO.Common;

namespace Engine.Objects.Game
{
    /// <summary>
    /// Represents all in-game objects which are not units, such as trees, rocks, barrels, projectiles, special effects. 
    /// 
    /// Can be either destructible or not. 
    /// </summary>
    public class Doodad : GameObject, IDoodad
    {
        private double _maxLife = 5;

        public double MaxLife
        {
            get { return _maxLife; }
            set
            {
                Life = Life / _maxLife * value;
                _maxLife = value;
            }
        }

        public double Life = 5;

        public bool Invulnerable = false;

        public Doodad(string model, Vector location)
            : base(model, location)
        {

        }
    }
}
