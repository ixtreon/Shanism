﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Objects;

namespace Engine.Objects
{
    /// <summary>
    /// Represents all in-game objects which are not units, such as trees, rocks, barrels, projectiles, special effects. 
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

        public Doodad(string name)
            : base(name)
        {

        }
    }
}