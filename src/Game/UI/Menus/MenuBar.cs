using Shanism.Client.UI.Containers;
using Shanism.Common.Entities;
using System.Linq;

namespace Shanism.Client.UI.Menus
{
    /// <summary>
    /// A hacky control that adds/removes menus to its parent.
    /// </summary>
    class MenuBar : Control
    {
        readonly CharacterDetails character;
        readonly GameMenu mainMenu;
        readonly SpellBook spellBook;

        readonly Window[] menus;

        IHero target;
        public IHero Target
        {
            get => target;
            set
            {
                target = value;
                character.Target = value;
                spellBook.Target = value;
            }
        }

        public MenuBar()
        {
            menus = new Window[]
            {
                (character = new CharacterDetails()),
                (spellBook = new SpellBook()),
                (mainMenu = new GameMenu()
                {
                    CanFocus = false,
                    TitleText = "Menu",
                    ParentAnchor = AnchorMode.None,
                    CenterBoth = true,
                }),
            };
        }

        protected override void OnParentChanged(ParentChangeArgs e)
        {
            if(e.PreviousParent != null)
                e.PreviousParent.RemoveRange(menus);

            base.OnParentChanged(e);

            if(e.Parent != null)
                e.Parent.AddRange(menus);
        }

        protected override void OnActionActivated(ClientActionArgs e)
        {
            switch (e.Action)
            {
                case ClientAction.HideMenus:

                    // focus parent first time
                    if (!Parent.IsFocusControl)
                    {
                        Parent.SetFocus();
                        break;
                    }

                    // remove windows 1 by 1 otherwise
                    var win = Parent.Controls.OfType<Window>().FirstOrDefault(w => w.IsVisible);
                    if(win != null)
                    {
                        win.Hide();
                        break;
                    }

                    // show menu in the end
                    mainMenu.IsVisible = true;
                    break;

                default:
                    foreach(var m in menus)
                        if(m.ToggleAction == e.Action)
                            m.IsVisible = !m.IsVisible;
                    break;
            }

            base.OnActionActivated(e);
        }
    }
}
