using System;
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
        readonly UiSystem ui;

        public MoveSystem(GameComponent game, UiSystem ui)
            : base(game)
        {
            this.ui = ui;
        }
        
        public override void Update(int msElapsed)
        {
            if (!ui.Root.HasFocus)
            {
                ClientState.IsMoving = false;
                return;
            }

            /// Keyboard movement
            var dx = Convert.ToInt32(Keyboard.IsDown(ClientAction.MoveRight)) - Convert.ToInt32(Keyboard.IsDown(ClientAction.MoveLeft));
            var dy = Convert.ToInt32(Keyboard.IsDown(ClientAction.MoveDown)) - Convert.ToInt32(Keyboard.IsDown(ClientAction.MoveUp));
            ClientState.IsMoving = (dx != 0 || dy != 0);

            if (ClientState.IsMoving)
            {
                var moveAngle = (float)(Math.Atan2(dy, dx));

                ClientState.MoveAngle = moveAngle;
            }
        }
    }
}
