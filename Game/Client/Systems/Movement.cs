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
        public override void Update(int msElapsed)
        {
            updateMovement();
        }

        void updateMovement()
        {
            if (!Control.FocusControl.IsRootControl)
            {
                ClientState.IsMoving = false;
                return;
            }

            var dx = Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveRight)) - Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveLeft));
            var dy = Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveDown)) - Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveUp));
            if (dx == 0 && dy == 0)
            {
                ClientState.IsMoving = false;
                return;
            }


            var keysAngle = Math.Atan2(dy, dx);
            var mouseAngle = MouseInfo.UiPosition.Angle;

            ClientState.IsMoving = true;
            ClientState.MoveAngle = (float)keysAngle;
        }
    }
}
