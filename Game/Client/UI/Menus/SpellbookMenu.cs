using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using IO.Common;
using Client.Input;
using Client.UI.Common;
using IO.Objects;

namespace Client.UI
{
    class SpellBook : Window
    {
        const int AbilitiesPerPage = 10;
        const int AbilitiesPerColumn = 5;

        public IHero Target { get; set; }

        int _currentPage = 0;

        SortedSet<SpellButton> spellButtons = new SortedSet<SpellButton>(
            new GenericComparer<SpellButton>((x, y) => string.Compare(x.Ability.Name, y.Ability.Name, StringComparison.Ordinal)));

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
                if (value != _currentPage)
                {
                    foreach (var sb in spellButtons)
                        sb.Visible = false;
                    foreach (var sb in spellButtons.Skip(AbilitiesPerPage * value).Take(value))
                        sb.Visible = true;
                    _currentPage = value;
                }
            }
        }

        public SpellBook()
            : base(AnchorMode.Right)
        {
            this.Action = GameAction.ToggleSpellBook;
            this.TitleText = "Spell Book";
            this.Visible = false;
            this.VisibleChanged += SpellBook_VisibleChanged;
        }

        void SpellBook_VisibleChanged(Control obj)
        {

            //update contents
            if (Target != null && this.Visible)
            {
                var newAbils = new HashSet<IAbility>(Target.Abilities);
                //remove old abilities and mark existing ones
                foreach (var sb in spellButtons.ToArray())
                    if (newAbils.Contains(sb.Ability))  //ability is already in the book; don't add it again
                        newAbils.Remove(sb.Ability);
                    else    //ability is in the book but not in the new list; remove it
                    {
                        spellButtons.Remove(sb);
                        this.Remove(sb);
                    }
                foreach (var ab in newAbils)     // add the actually new abilities
                {
                    var sb = new SpellButton
                    {
                        CanSelect = false,
                        Size = new Vector(0.15),
                    };
                    sb.Ability = ab;
                    spellButtons.Add(sb);
                    this.Add(sb);
                }

                var i = 0;
                var page = 0;

                // update their positions
                foreach (var sb in spellButtons)
                {
                    var id = new Vector(i / AbilitiesPerColumn, i % AbilitiesPerColumn);
                    //var relativePos = new Vector(0.075, 0.1) + id * new Vector(0.5, 0.2);
                    sb.Location = new Vector(0.075f + (i / AbilitiesPerColumn) * 0.5f,
                        0.1f + (i % AbilitiesPerColumn) * 0.2f);

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
            Visible &= (Target != null);

            base.OnUpdate(msElapsed);
        }
    }
}
