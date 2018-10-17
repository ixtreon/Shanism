using Shanism.Client.Game.UI.Menus;
using Shanism.Client.UI.Containers;
using Shanism.Client.UI.Menus;
using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.UI
{
    enum GameMenuButton
    {
        Keybinds, Options, Restart, Quit
    }

    class GameMenu : GameMenuBase
    {
        static readonly int NButtons = Enum<GameMenuButton>.Count;
        static readonly Vector2 ButtonSize = new Vector2(0.45f, 0.10f);
        const float MenuPadding = DefaultPadding * 3;

        public KeybindsMenu Keybinds { get; }
        public OptionsWindow Options { get; }

        public GameMenu()
        {
            Options = new OptionsWindow();
            Keybinds = new KeybindsMenu();

            AddEntry("Options", _ => Options.Show());
            AddEntry("Keybinds", _ => Keybinds.Show());
            //AddEntry("Restart", _ => View.Game.Context.RestartGame());
            AddEntry("Quit", _ => GameHelper.Quit());
        }

        protected override void OnParentChanged(ParentChangeArgs e)
        {
            if(e.PreviousParent != null)
            {
                e.PreviousParent.Remove(Options);
                e.PreviousParent.Remove(Keybinds);
            }

            base.OnParentChanged(e);

            if(e.Parent != null)
            {
                e.Parent.Add(Options);
                e.Parent.Add(Keybinds);
            }
        }
    }
}
