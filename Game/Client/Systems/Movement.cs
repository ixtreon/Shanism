﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Message;
using Shanism.Client.Input;
using Shanism.Common.Message.Client;
using Shanism.Client.UI;
using Shanism.Common;

namespace Shanism.Client.Systems
{
    class MoveSystem : ClientSystem
    {
        private UiSystem ui;

        public MoveSystem(UiSystem ui)
        {
            this.ui = ui;
        }
        
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

            /// Keyboard movement
            var dx = Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveRight)) - Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveLeft));
            var dy = Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveDown)) - Convert.ToInt32(KeyboardInfo.IsDown(ClientAction.MoveUp));
            ClientState.IsMoving = (dx != 0 || dy != 0);

            if (ClientState.IsMoving)
            {
                var moveAngle = (float)(Math.Atan2(dy, dx));

                ClientState.MoveAngle = moveAngle;
            }
        }
    }
}
