using Engine.Objects;
using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// Represents a player connected to a game. 
    /// </summary>
    public class Player
    {
        public Hero Hero { get; private set; }

        public readonly IClient InputDevice;

        public Player(Hero hero, IClient inputDevice)
        {
            this.Hero = hero;
            this.InputDevice = inputDevice;


            //link events
            inputDevice.OnSpecialAction += Hero.OnAction;
        }

        /// <summary>
        /// Updates the movement state of the player. 
        /// </summary>
        public void Update()
        { 
            this.Hero.MovementState = InputDevice.MovementState;
        }
    }
}
