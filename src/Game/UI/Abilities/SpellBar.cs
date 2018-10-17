using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Shanism.Common.Objects;
using System.Numerics;

namespace Shanism.Client.UI
{
    class SpellBar : Control
    {
        const int MaxButtonCount = 64;


        int _buttonsPerRow = 8;

        readonly List<SpellBarButton> buttons = new List<SpellBarButton>();
        readonly int BarId;

        public static float ButtonSize { get; } = 0.1f;

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
            BackColor = UiColors.ControlBackground;
        }

        protected override void OnActionActivated(ClientActionArgs e)
        {
            base.OnActionActivated(e);
         
            if (!e.Action.IsBarAction() || e.Action.GetBarId() != BarId)
                return;

            var btnId = e.Action.GetButtonId();
            if (btnId < 0 || btnId >= buttons.Count)
                return;

            var ab = buttons[btnId]?.Ability;
            if (ab == null || ab.TargetType == AbilityTargetType.Passive)
                return;

            buttons[btnId].IsSelected = true;
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

        internal void SelectButton(int id)
        {
            buttons[id].IsSelected = true;
        }

        internal void SetAbilities(IEnumerable<IAbility> abilities)
        {
            var i = 0;
            foreach (var ab in abilities)
            {
                buttons[i].Ability = ab;
                i++;
            }
        }

        void addButton()
        {
            var id = buttons.Count;
            var btn = new SpellBarButton(BarId, id)
            {
                Size = new Vector2(ButtonSize),
                Padding = ButtonSize * 4 / 64,
            };

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

                btn.Location = new Vector2(x, y) * ButtonSize;
            }

            var w = Math.Min(ButtonCount, MaxButtonsPerRow);
            var h = (float)Math.Max(1, Math.Ceiling((double)ButtonCount / MaxButtonsPerRow));
            Size = new Vector2(w, h) * ButtonSize;
        }

    }
}
