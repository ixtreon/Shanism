using Shanism.Client.IO;
using Shanism.Client.UI;
using Shanism.Common.Messages;
using System;

namespace Shanism.Client.Game.Systems
{
    class MoveSystem : ISystem
    {
        readonly PlayerState playerState;

        Control root;
        KeyboardSystem keyboard;
        MouseSystem mouse;

        public MoveSystem(MouseSystem mouse, KeyboardSystem keyboard, Control root, 
            PlayerState playerState)
        {
            this.playerState = playerState;

            this.keyboard = keyboard;
            this.mouse = mouse;
            this.root = root;
        }

        public void Update(int msElapsed)
        {
            playerState.CursorPosition = mouse.InGamePosition;
            
            // movement
            if (!root.IsFocusControl)
            {
                playerState.SetMovement(null);
                return;
            }

            var dx = b2i(keyboard.IsDown(ClientAction.MoveRight)) - b2i(keyboard.IsDown(ClientAction.MoveLeft));
            var dy = b2i(keyboard.IsDown(ClientAction.MoveDown)) - b2i(keyboard.IsDown(ClientAction.MoveUp));

            if(dx != 0 || dy != 0)
                playerState.SetMovement((float)Math.Atan2(dy, dx));
            else
                playerState.SetMovement(null);
        }

        static int b2i(bool b) => b ? 1 : 0;
    }
}
