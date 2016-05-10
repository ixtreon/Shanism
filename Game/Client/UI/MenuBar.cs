using Shanism.Client.Input;
using Shanism.Client.UI.Common;
using Shanism.Common.Objects;
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
                (MainMenu = new MainMenu()),
                (SpellBook = new SpellBook()),
                (Keybinds = new KeybindsMenu()),
            };

            foreach (var w in menus)
                mainContainer.Add(w);

            MainMenu.ExitClicked += closeTheGame;
            MainMenu.KeybindsClicked += showKeybindMenu;
            MainMenu.OptionsClicked += showOptionsMenu;
        }

        void showOptionsMenu()
        {

        }

        void showKeybindMenu()
        {
            Keybinds.IsVisible = true;
        }

        void closeTheGame()
        {
            ExitHelper.Exit();
        }

        void onActionActivated(GameAction ga)
        {
            switch (ga)
            {
                case GameAction.ToggleMenus:
                    if (menus.Any(m => m.IsVisible))
                        foreach (var m in menus)
                            m.Hide();
                    else
                        MainMenu.Show();
                    break;

                default:
                    foreach (var m in menus.Where(m => m.ToggleAction == ga))
                        m.IsVisible = !m.IsVisible;
                    break;
            }
        }
    }
}
