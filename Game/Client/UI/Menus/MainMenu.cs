using Shanism.Client.Input;
using Shanism.Client.UI.Common;
using Shanism.Client.UI.Menus;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Shanism.Client.UI
{
    enum MenuButtonType
    {
        Keybinds, Options, Restart, Exit
    }

    class MainMenu : Window
    {
        class MenuButton : Button
        {
            public MenuButtonType Type { get; set; }
        }

        static readonly int NButtons = Enum<MenuButtonType>.Count;
        static readonly Vector ButtonSize = new Vector(0.45, 0.10);
        static readonly double MenuPadding = Padding * 3;


        public event Action<MenuButtonType> ButtonClicked;


        public MainMenu()
        {
            HasTitleBar = true;
            CanFocus = false;
            TitleText = "Menu";
            ToggleAction = ClientAction.ToggleMenus;

            Location = new Vector(0.75, 0.6);
            Size = new Vector(ButtonSize.X, TitleHeight + (Padding + ButtonSize.Y) * NButtons) + 2 * MenuPadding;
            ParentAnchor = AnchorMode.None;

            foreach (var ty in Enum<MenuButtonType>.Values)
                addButton(ty);
        }

        MenuButton lastButton;

        MenuButton addButton(MenuButtonType btnType)
        {
            var btnY = (lastButton?.Bottom ?? TitleHeight + MenuPadding) + Padding;
            var btnPos = new Vector(MenuPadding, btnY);

            lastButton = new MenuButton()
            {
                Type = btnType,
                Text = btnType.ToString(),

                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = btnPos,
                Size = ButtonSize,
            };
            lastButton.MouseUp += onButtonClick;
            Add(lastButton);

            return lastButton;
        }

        void onButtonClick(MouseButtonArgs ev)
        {
            var btn = (MenuButton)ev.Control;
            var btnType = btn.Type;

            IsVisible = false;
            ButtonClicked?.Invoke(btnType);
        }
    }
}
