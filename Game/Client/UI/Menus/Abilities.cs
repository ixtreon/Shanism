using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common.Game;
using Shanism.Client.Input;
using Shanism.Client.UI.Common;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;


using Color = Microsoft.Xna.Framework.Color;

namespace Shanism.Client.UI
{
    class SpellBook : Window
    {
        static readonly Point DefaultPageSize = new Point(2, 5);
        static readonly Vector DefaultButtonSize = new Vector(0.12);

        public IHero Target { get; set; }

        int _currentPage;

        SortedSet<SpellButton> spellButtons = new SortedSet<SpellButton>(
            new GenericComparer<SpellButton>((x, y) => string.Compare(x.Ability.Name, y.Ability.Name, StringComparison.Ordinal)));

        public Point PageSize { get; set; } = DefaultPageSize;

        public Vector ButtonSize { get; set; } = DefaultButtonSize;

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
                    sb.IsVisible = false;
                foreach (var sb in spellButtons
                    .Skip(AbilitiesPerPage * value)
                    .Take(AbilitiesPerPage))
                    sb.IsVisible = true;
                _currentPage = value;
            }
        }

        Control pagePanel;
        Button prevPage, nextPage;
        Label pageText;

        const double PagePanelHeight = 0.08;

        public SpellBook()
            : base(AnchorMode.Right)
        {
            ToggleAction = ClientAction.ToggleAbilityMenu;
            TitleText = "Abilities";
            IsVisible = false;
            CanFocus = false;

            VisibleChanged += SpellBook_VisibleChanged;

            pagePanel = new Control
            {
                Location = new Vector(LargePadding, Size.Y - PagePanelHeight - LargePadding),
                Size = new Vector(Size.X - 2 * LargePadding, PagePanelHeight),
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Bottom,

                BackColor = Color.Green,
            };

            Add(pagePanel);
        }

        void SpellBook_VisibleChanged(Control obj)
        {
            ButtonSize = new Vector(0.12);
            //update contents
            if (Target != null && IsVisible)
            {
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
                    var sb = new SpellButton
                    {
                        CanSelect = false,
                        Size = ButtonSize,
                    };
                    sb.Ability = ab;
                    spellButtons.Add(sb);
                    this.Add(sb);
                }

                var menuPos = new Vector(0, TitleHeight) + LargePadding;
                var menuSize = new Vector(Size.X, pagePanel.Top) - LargePadding - menuPos;

                var unit = (menuSize - ButtonSize * PageSize) / (PageSize * 2);
                var baseOffset = menuPos + unit;
                var cumulativeOffset = ButtonSize + unit * 2;

                // update their positions
                var i = 0;
                var page = 0;
                foreach (var sb in spellButtons)
                {
                    var id = new Vector(i / PageSize.Y, i % PageSize.Y);

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
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);
        }

        protected override void OnUpdate(int msElapsed)
        {
            IsVisible &= (Target != null);

            base.OnUpdate(msElapsed);
        }
    }
}
