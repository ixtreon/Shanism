using IO.Common;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.Menus
{
    /// <summary>
    /// A control that contains all the menus in the game, as well as a visualisation for 'em. 
    /// The menus are added to the main window, rather than the menu bar
    /// so they get updated even if the bar is not visible. 
    /// </summary>
    class MenuBar : Control
    {
        public CharacterMenu Character { get; } = new CharacterMenu();
        public MainMenu Main { get; } = new MainMenu();
        public SpellBook SpellBook { get; } = new SpellBook();


        IHero _target;
        public IHero OurHero
        {
            get { return _target; }
            set
            {
                _target = value;

                Character.Target = _target;
                SpellBook.Target = _target;
            }
        }

        public MenuBar(Control mainContainer)
        {
            mainContainer.Add(Character);
            mainContainer.Add(Main);
            mainContainer.Add(SpellBook);

            Size = new Vector(0.5, 0.2);
        }
    }
}
