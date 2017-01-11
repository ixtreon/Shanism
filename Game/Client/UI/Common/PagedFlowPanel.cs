﻿using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    class PagedFlowPanel : Control
    {
        public readonly FlowPanel List;

        readonly Button Prev, Next;
        readonly Label Text;

        public int CurrentPage { get; private set; } = 0;

        public int ItemsPerPage { get; set; } = 5;

        public int PagesCount => (int)Math.Ceiling((double)List.Controls.Count / ItemsPerPage);

        public event Action<PagedFlowPanel> PageChanged;

        public PagedFlowPanel()
        {
            Size = new Vector(0.6, 0.5);
            ParentAnchor = AnchorMode.All;

            var barSize = 0.08;

            Add(List = new FlowPanel
            {
                Size = Size - new Vector(0, barSize),
                Location = Vector.Zero,

                ParentAnchor = AnchorMode.All,
            });
            List.ControlAdded += List_ControlAdded;

            var btnSize = new Vector(0.25, barSize - 2 * Padding);
            Add(Prev = new Button
            {
                Text = "< Prev",

                Size = btnSize,

                Left = Padding,
                Bottom = Size.Y - Padding,

                ParentAnchor = AnchorMode.Bottom | AnchorMode.Left,
            });
            Prev.MouseUp += Prev_MouseUp;

            Add(Next = new Button
            {
                Text = "Next >",

                Size = btnSize,

                Right = Size.X - Padding,
                Bottom = Size.Y - Padding,

                ParentAnchor = AnchorMode.Bottom | AnchorMode.Right,
            });
            Next.MouseUp += Next_MouseUp;

            Add(Text = new Label
            {
                Font = Content.Fonts.NormalFont,
                TextXAlign = 0.5f,
                TextColor = Color.Goldenrod,

                AutoSize = false,
                Size = new Vector(Size.X - 2 * btnSize.X - 4 * Padding, btnSize.Y),
                Location = new Vector(Size.X / 2, Size.Y - Padding - btnSize.Y),
                
                ParentAnchor = AnchorMode.Bottom | AnchorMode.Left | AnchorMode.Right,
            });
            updateText();
        }

        void Next_MouseUp(Input.MouseButtonArgs obj)
        {
            setPage(CurrentPage + 1);
        }

        void Prev_MouseUp(Input.MouseButtonArgs obj)
        {
            setPage(CurrentPage - 1);
        }

        void List_ControlAdded(Control c)
        {
            var min = CurrentPage * ItemsPerPage;
            var max = min + ItemsPerPage;
            var cid = List.Controls.Count - 1;

            c.IsVisible = (cid >= min) && cid < max;
            updateText();
        }

        void updateText()
        {
            Text.Text = $"{CurrentPage + 1} / {PagesCount}";
        }

        void setPage(int id)
        {
            id = id.Clamp(0, PagesCount - 1);
            CurrentPage = id;
            updateText();

            var min = id * ItemsPerPage;
            var max = min + ItemsPerPage;
            var lcs = List.Controls;

            for(int i = 0; i < lcs.Count; i++)
            {
                var isShown = min <= i && i < max;
                lcs[i].IsVisible = isShown;
            }

            List.Reflow();

            PageChanged?.Invoke(this);
        }

    }
}