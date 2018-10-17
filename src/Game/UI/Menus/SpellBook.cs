using Ix.Math;
using Shanism.Client.UI.Abilities;
using Shanism.Client.UI.Containers;
using Shanism.Common;
using Shanism.Common.Entities;
using Shanism.Common.Objects;
using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Shanism.Client.UI.Menus
{
    class SpellBook : Window
    {
        const float PagePanelHeight = 0.08f;

        static readonly Point DefaultPageSize = new Point(2, 5);
        static readonly Vector2 DefaultButtonSize = new Vector2(0.12f);

        public IHero Target { get; set; }

        int _currentPage;

        SortedSet<SpellButton> spellButtons = new SortedSet<SpellButton>(
            new GenericComparer<SpellButton>((x, y) => string.Compare(x.Ability.Name, y.Ability.Name, StringComparison.Ordinal)));

        public Point PageSize { get; set; } = DefaultPageSize;

        public Vector2 ButtonSize { get; set; } = DefaultButtonSize;

        int AbilitiesPerPage => PageSize.X * PageSize.Y;

        public int Pages
        {
            get
            {
                return 1 + (spellButtons.Count - 1) / AbilitiesPerPage;
            }
        }

        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                foreach (var sb in spellButtons)
                    sb.Hide();
                foreach (var sb in spellButtons
                    .Skip(AbilitiesPerPage * value)
                    .Take(AbilitiesPerPage))
                    sb.Show();
                _currentPage = value;
            }
        }

        Control pagePanel;
        Button prevPage, nextPage;
        Label pageText;


        public SpellBook()
            : base(AnchorMode.Right)
        {
            ToggleAction = ClientAction.ToggleAbilityMenu;
            TitleText = "Abilities";
            IsVisible = false;

            Shown += onShown;

            pagePanel = new Control
            {
                Location = new Vector2(LargePadding, Size.Y - PagePanelHeight - LargePadding),
                Size = new Vector2(Size.X - 2 * LargePadding, PagePanelHeight),
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Bottom,

                BackColor = Color.Green,
            };

            Add(pagePanel);
        }

        void onShown(Control sender, EventArgs e)
        {
            if (Target == null)
                return;

            var newAbils = new HashSet<IAbility>(Target.Abilities ?? Enumerable.Empty<IAbility>());

            //remove old abilities and mark existing ones
            foreach (var sb in spellButtons.ToList())
                if (newAbils.Contains(sb.Ability))  //ability is already in the book; don't add it again
                    newAbils.Remove(sb.Ability);
                else    //ability is in the book but not in the new list; remove it
                {
                    spellButtons.Remove(sb);
                    Remove(sb);
                }

            foreach (var ab in newAbils)     // add the actually new abilities
            {
                var sb = new SpellBookButton(ab, ButtonSize);
                spellButtons.Add(sb);
                Add(sb);
            }

            var menuPos = new Vector2(0, DefaultTitleHeight).Offset(LargePadding);
            var menuSize = new Vector2(Size.X, pagePanel.Top) - menuPos - new Vector2(LargePadding);

            var unit = (menuSize - ButtonSize * PageSize) / (PageSize * 2);
            var baseOffset = menuPos + unit;
            var cumulativeOffset = ButtonSize + unit * 2;

            // update their positions
            var i = 0;
            var page = 0;
            foreach (var sb in spellButtons)
            {
                var id = new Vector2(i / PageSize.Y, i % PageSize.Y);

                sb.Location = baseOffset + id * cumulativeOffset;

                if (++i == AbilitiesPerPage)
                {
                    i = 0;
                    page++;
                }
            }

            //go to page 0
            CurrentPage = 0;
        }

        public override void Draw(Canvas g)
        {
            base.Draw(g);
        }

        public override void Update(int msElapsed)
        {
            IsVisible &= (Target != null);

            base.Update(msElapsed);
        }
    }
}
