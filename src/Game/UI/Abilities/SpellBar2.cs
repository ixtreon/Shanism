using Shanism.Client.Game;
using Shanism.Client.UI.Containers;
using Shanism.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Abilities
{
    class SpellBar2 : ListPanel
    {
        public event UiEventHandler<EventArgs> ButtonActivated;

        int? selectedButtonID;

        public int ButtonCount
        {
            get => Controls.Count;
            set => setButtonCount(value);
        }

        public SpellBar2() : base(Direction.LeftToRight, sizeMode: ListSizeMode.ResizeBoth)
        {
            ButtonCount = 8;
        }

        void setButtonCount(int newCount)
        {
            while (Controls.Count < newCount)
            {
                var buttonID = Controls.Count;
                var button = new SpellBarButton2();

                Add(button);

                button.Selected += (o, e) => selectButton(buttonID);

            }
            while (Controls.Count > newCount)
                RemoveAt(Controls.Count - 1);
        }

        void selectButton(int? buttonID)
        {
            if (selectedButtonID == buttonID)
                return;

            if (selectedButtonID != null)
                GetButton(selectedButtonID.Value).IsSelected = false;

            selectedButtonID = buttonID;

            if (selectedButtonID != null)
                GetButton(selectedButtonID.Value).IsSelected = true;
        }

        public void SetKeybinds(GameSettings settings, int barId)
        {
            for(int btnId = 0; btnId < Controls.Count; btnId++)
            {
                var kb = settings.Keybinds[barId, btnId];
                GetButton(btnId).SetKeybind(kb);
            }
        }


        void onButtonSelected(EventArgs obj)
        {
            throw new NotImplementedException();
        }


        SpellBarButton2 GetButton(int btnId) => (SpellBarButton2)Controls[btnId];

        public void SetAbility(int buttonId, IAbility ability)
        {
            GetButton(buttonId).Ability = ability;
        }
    }
}
