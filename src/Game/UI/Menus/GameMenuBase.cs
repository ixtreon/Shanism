using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using System;
using System.Numerics;

namespace Shanism.Client.Game.UI.Menus
{
    class GameMenuBase : Window
    {

        static readonly Vector2 DefaultButtonSize = new Vector2(0.45f, 0.10f);

        readonly ListPanel entries;

        public GameMenuBase()
        {
            ToggleAction = ClientAction.HideMenus;
            Padding = 3 * DefaultPadding;

            Add(entries = new ListPanel(Direction.TopDown, sizeMode: ListSizeMode.ResizeBoth)
            {
                Location = ClientOffset,
            });

            entries.SizeChanged += resizeSelf;
        }

        public void AddEntry(string name, Action<Button> handler)
        {
            var button = new Button
            {
                Text = name,

                Size = DefaultButtonSize,
            };

            button.MouseClick += (o, e) => handler(button);

            entries.Add(button);
        }

        void resizeSelf(Control sender, EventArgs args)
        {
            Size = ClientOffset + entries.Size + new Vector2(Padding);
        }
    }
}
