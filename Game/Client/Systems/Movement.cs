using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Message;
using Shanism.Common.Game;
using Shanism.Client.Input;
using Shanism.Common.Message.Client;
using Shanism.Client.UI;

namespace Shanism.Client.Systems
{
    class MoveSystem : ClientSystem
    {
        MovementState movementState;


        public override void Update(int msElapsed)
        {
            updateMovement();
        }

        void updateMovement()
        {
            var newMovementState = MovementState.Stand;

            if (Control.FocusControl.IsRootControl)
            {
                var dx = Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveRight)) - Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveLeft));
                var dy = Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveDown)) - Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveUp));

                newMovementState = new MovementState(dx, dy);
            }

            if (newMovementState != movementState)
            {
                movementState = newMovementState;

                SendMessage(new MoveMessage(newMovementState));
            }
        }
    }
}
