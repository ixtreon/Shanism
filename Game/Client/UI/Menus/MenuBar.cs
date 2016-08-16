using Shanism.Client.Input;
using Shanism.Client.UI.Common;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.StubObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Menus
{
    /// <summary>
    /// A control that contains all the menus in the game, as well as a visualisation for 'em. 
    /// The menus are added to the main window, rather than the menu bar
    /// so they get updated even if the bar is not visible. 
    /// </summary>
    class MenuBar : Control
    {
        public KeybindsMenu Keybinds { get; }
        public OptionsWindow Options { get; }
        public CharacterMenu Character { get; }
        public MainMenu MainMenu { get; }
        public SpellBook SpellBook { get; }

        readonly Window[] menus;

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
            IsVisible = false;

            GameActionActivated += onActionActivated;

            menus = new Window[]
            {
                (Character = new CharacterMenu()),
                (Options = new OptionsWindow()),
                (MainMenu = new MainMenu()),
                (SpellBook = new SpellBook()),
                (Keybinds = new KeybindsMenu()),
            };

            foreach (var w in menus)
                mainContainer.Add(w);

            MainMenu.ButtonClicked += onMainMenuButtonClick;
        }

        void onMainMenuButtonClick(MenuButtonType btnType)
        {
            switch (btnType)
            {
                case MenuButtonType.Keybinds:
                    Keybinds.IsVisible = true;
                    break;

                case MenuButtonType.Options:
                    Options.IsVisible = true;
                    break;

                case MenuButtonType.Restart:
                    GameHelper.Restart();
                    break;

                case MenuButtonType.Exit:
                    GameHelper.Exit();
                    break;
            }
        }

        void onActionActivated(ClientAction ga)
        {
            switch (ga)
            {
                case ClientAction.ToggleMenus:
                    if (menus.Any(m => m.IsVisible))
                        foreach (var m in menus)
                            m.IsVisible = false;
                    else
                        MainMenu.IsVisible = true;
                    break;

                default:
                    foreach (var m in menus.Where(m => m.ToggleAction == ga))
                        m.IsVisible = !m.IsVisible;
                    break;
            }
        }
    }
}
