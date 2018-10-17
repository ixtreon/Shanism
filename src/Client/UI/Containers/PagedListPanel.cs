using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Shanism.Client.UI.Containers
{
    public class PagedListPanel : Control
    {

        public ListPanel List { get; }

        readonly Button Prev, Next;
        readonly Label Text;

        public int CurrentPage { get; private set; } = 0;

        public int ItemsPerPage { get; set; } = 5;

        public int PagesCount => (int)Math.Ceiling((double)List.Controls.Count / ItemsPerPage);

        public event Action<PagedListPanel> PageChanged;

        public PagedListPanel()
        {
            Size = new Vector2(0.6f, 0.5f);
            ParentAnchor = AnchorMode.All;

            const float barSize = 0.08f;
            var btnSize = new Vector2(0.25f, barSize - 2 * Padding);

            Add(List = new ListPanel
            {
                Size = Size - new Vector2(0, barSize),
                Location = Vector2.Zero,

                ParentAnchor = AnchorMode.All,
                Direction = Direction.TopDown,
            });
            Add(Prev = new Button
            {
                Text = "< Prev",

                Size = btnSize,

                Left = Padding,
                Bottom = Size.Y - Padding,

                IsVisible = false,
                ParentAnchor = AnchorMode.Bottom | AnchorMode.Left,
            });
            Add(Next = new Button
            {
                Text = "Next >",

                Size = btnSize,

                Right = Size.X - Padding,
                Bottom = Size.Y - Padding,

                IsVisible = false,
                ParentAnchor = AnchorMode.Bottom | AnchorMode.Right,
            });
            Add(Text = new Label
            {
                Font = Content.Fonts.NormalFont,
                TextAlign = AnchorPoint.Center,
                TextColor = UiColors.TextTitle,

                Size = new Vector2(Size.X - 2 * btnSize.X - 4 * Padding, btnSize.Y),
                Location = new Vector2(Size.X / 2, Size.Y - Padding - btnSize.Y),
                
                ParentAnchor = AnchorMode.Bottom | AnchorMode.Left | AnchorMode.Right,
            });

            List.ControlAdded += updateChildControl;
            Prev.MouseClick += (o, e) => setPage(CurrentPage - 1);
            Next.MouseClick += (o, e) => setPage(CurrentPage + 1);

            updateText();
        }

        void updateChildControl(Control sender, ControlChildArgs e)
        {
            var min = CurrentPage * ItemsPerPage;
            var max = min + ItemsPerPage;
            var cid = List.Controls.Count - 1;

            e.Child.IsVisible = (cid >= min) && cid < max;
            updateText();
        }

        void updateText()
        {
            Text.Text = $"{CurrentPage + 1} / {PagesCount}";
            Prev.IsVisible = PagesCount > 1 && CurrentPage > 0;
            Next.IsVisible = PagesCount > 1 && CurrentPage < PagesCount - 1;
        }

        void setPage(int id)
        {
            setCurrentPageVisible(false);
            CurrentPage = id.Clamp(0, PagesCount - 1);
            setCurrentPageVisible(true);

            List.Reflow();
            updateText();
            PageChanged?.Invoke(this);
        }

        void setCurrentPageVisible(bool isVisible)
            => setPageVisible(List.Controls, CurrentPage, ItemsPerPage, isVisible);

        static void setPageVisible(IReadOnlyList<Control> controls, 
            int pageId, int itemsPerPage,
            bool isVisible)
        {
            var start = pageId * itemsPerPage;
            var end = Math.Min(controls.Count, start + itemsPerPage);

            for (int i = start; i < end; i++)
                controls[i].IsVisible = isVisible;
        }
    }
}
