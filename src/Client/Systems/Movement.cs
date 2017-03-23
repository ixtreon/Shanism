using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Input;
using Shanism.Client.UI;
using Shanism.Common.Message.Client;

namespace Shanism.Client.Systems
{
    class MoveSystem : IClientSystem
    {
        readonly KeyboardInfo keyboard;
        readonly PlayerState playerState;
        readonly GameRoot root;

        public MoveSystem(GameRoot root, KeyboardInfo keyboard, PlayerState playerState)
        {
            this.playerState = playerState;
            this.keyboard = keyboard;
            this.root = root;
        }
        
        public void Update(int msElapsed)
        {
            if (!root.HasFocus)
            {
                playerState.IsMoving = false;
                return;
            }

            // Keyboard movement
            var dx = Convert.ToInt32(keyboard.IsDown(ClientAction.MoveRight)) 
                - Convert.ToInt32(keyboard.IsDown(ClientAction.MoveLeft));
            var dy = Convert.ToInt32(keyboard.IsDown(ClientAction.MoveDown)) 
                - Convert.ToInt32(keyboard.IsDown(ClientAction.MoveUp));
            playerState.IsMoving = (dx != 0 || dy != 0);

            if (playerState.IsMoving)
            {
                var moveAngle = (float)(Math.Atan2(dy, dx));
                playerState.MoveAngle = moveAngle;
            }
        }
    }
}
