using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;

namespace Shanism.Client.UI
{
    class SpellBar : Control
    {
        const int MaxButtonCount = 64;


        int _buttonsPerRow = 8;

        readonly List<SpellBarButton> buttons = new List<SpellBarButton>();

        readonly int BarId;

        public static double ButtonSize { get; } = 0.1;

        public event Action<IAbility> AbilityActivated;

        public int MaxButtonsPerRow
        {
            get { return _buttonsPerRow; }
            set
            {
                _buttonsPerRow = value;
                reflow();
            }
        }

        public int ButtonCount
        {
            get { return Controls.Count(); }
            set
            {
                updateButtonCount(value);
            }
        }

        public SpellBar(int barId)
        {
            BarId = barId;
            BackColor = new Color(50, 50, 50, 200);

            ButtonCount = 16;
            MaxButtonsPerRow = 8;

            GameActionActivated += onActionActivated;
        }

        void onActionActivated(Input.ClientAction act)
        {
            if (!act.IsBarAction() || act.GetBarId() != BarId)
                return;

            var btnId = act.GetButtonId();
            if (btnId < 0 || btnId >= buttons.Count)
                return;

            var btn = buttons[btnId];
            var ab = btn?.Ability;
            if (ab?.TargetType == AbilityTargetType.NoTarget)
            {
                AbilityActivated?.Invoke(ab);
            }
            else
            {
                buttons[btnId].IsSelected = true;
            }

        }

        void updateButtonCount(int newCount)
        {
            var oldCount = Controls.Count();
            if (oldCount == newCount)
                return;

            while (buttons.Count < newCount)
                addButton();

            while (buttons.Count > newCount)
                removeLastButton();

            reflow();
        }

        void addButton()
        {
            var id = buttons.Count;
            var btn = new SpellBarButton(BarId, id) { Size = new Vector(ButtonSize) };

            buttons.Add(btn);
            Add(btn);
        }

        void removeLastButton()
        {
            var id = buttons.Count - 1;
            var btn = buttons[id];

            buttons.RemoveAt(id);
            Remove(btn);
        }

        void reflow()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                var btn = buttons[i];
                var x = (i % MaxButtonsPerRow);
                var y = (i / MaxButtonsPerRow);

                btn.Location = new Vector(x, y) * ButtonSize;
            }

            var w = Math.Min(ButtonCount, MaxButtonsPerRow);
            var h = Math.Max(1, Math.Ceiling((double)ButtonCount / MaxButtonsPerRow));
            Size = new Vector(w, h) * ButtonSize;
        }

    }
}
