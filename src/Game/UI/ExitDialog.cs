using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    class ExitDialog : MessageBox
    {
        public ExitDialog()
            : base("Exit", "Are you serious?", 
                  MessageBoxButtons.Yes | MessageBoxButtons.No)
        {
            IsVisible = false;
            RemoveOnButtonClick = false;

            ButtonClicked += onButtonClicked;
        }

        void onButtonClicked(MessageBoxButtons btn)
        {
            switch (btn)
            {
                case MessageBoxButtons.Yes:
                    GameHelper.Exit();
                    break;
            }
        }
    }
}
