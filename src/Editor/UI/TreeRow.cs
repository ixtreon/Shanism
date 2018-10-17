using Microsoft.Xna.Framework.Input;
using Shanism.Client;
using Shanism.Client.Sprites;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Common;
using Shanism.Editor.Models.Content;
using System;
using System.Linq;
using System.Numerics;

namespace Shanism.Editor.UI
{

    /// <summary>
    /// A folder within a list-panel.
    /// Consists of a header (the actual row) and zero or more children. 
    /// </summary>
    class TreeRow : ListPanel
    {
        const string ExpandedText = "-";
        const string CollapsedText = "+";

        public ITreeNode Node { get; }

        readonly int depth;
        readonly ITreeEventHandler handler;
        readonly TreeRowHeader header;

        bool _isExpanded = true;
        bool _isSelected;

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (value == _isExpanded)
                    return;

                _isExpanded = value;

                // update btn text + controls
                header.ButtonText = IsExpanded ? ExpandedText : CollapsedText;
                for (int i = 1; i < Controls.Count; i++)
                    Controls[i].IsVisible = IsExpanded;
                Reflow();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;

                if (header != null)
                    header.BackColor = value ? UiColors.HoverOverlay : Color.Transparent;
            }
        }

        public TreeRow(ITreeNode node, ITreeEventHandler handler)
            : this(node, handler, 0, false) { }


        protected TreeRow(ITreeNode node, ITreeEventHandler handler, int depth, bool createHeader)
            : base(Direction.TopDown, sizeMode: ListSizeMode.ResizePrimary)
        {
            this.handler = handler;
            this.depth = depth;
            this.Node = node;

            Size = new Vector2(0.1f, 0.06f);
            ParentAnchor = AnchorMode.Top | AnchorMode.Horizontal;
            Padding = 0;
            ControlSpacing = Vector2.Zero;
            CanHover = false;

            // header
            if (createHeader)
            {
                Add(header = new TreeRowHeader(depth, node)
                {
                    Width = Size.X,
                    ParentAnchor = AnchorMode.Top | AnchorMode.Horizontal,
                });
            }

            // children
            var childDepth = createHeader ? (depth + 1) : depth;
            foreach (var ch in node.Children)
                Add(new TreeRow(ch, handler, childDepth, true));

            // events
            if (createHeader)
            {
                header.FocusGained += (o, e) => handler.OnRowSelected(this);
                header.ButtonClicked += () => handler.OnRowExpanded(this);
            }
        }

        protected override void OnKeyPress(KeyboardArgs e)
        {
            base.OnKeyPress(e);
            switch (e.Key)
            {
                case Keys.Left:
                    IsExpanded = false;
                    break;

                case Keys.Right:
                    IsExpanded = true;
                    break;
            }
        }

        class TreeRowHeader : Control
        {

            readonly Label text;
            readonly Button button;
            readonly SpriteBox icon;

            public float RowHeight { get; set; } = 0.06f;
            public float HorizontalLevelOffset { get; set; } = 0.05f;

            public event Action ButtonClicked;

            public string ButtonText
            {
                get => button.Text;
                set => button.Text = value;
            }

            public TreeRowHeader(int level, ITreeNode node)
            {
                Size = new Vector2(0.5f, RowHeight);
                CanFocus = node.HasItem;

                Add(button = new Button
                {
                    Location = new Vector2(level * HorizontalLevelOffset, 0.005f),
                    Size = new Vector2(RowHeight - 0.01f),
                    ParentAnchor = AnchorMode.Left,

                    CanFocus = node.Children.Any(),
                    CanHover = node.Children.Any(),

                    Text = node.Children.Any() ? ExpandedText : " ",
                });
                Add(icon = new SpriteBox
                {
                    Left = button.Right + Padding,
                    Size = new Vector2(0, RowHeight),

                    SpriteSizeMode = TextureSizeMode.FitZoom,
                    ParentAnchor = AnchorMode.Left,

                });
                Add(text = new Label
                {
                    Font = Content.Fonts.NormalFont,
                    Left = icon.Right + Padding,
                    Size = new Vector2(Size.X - icon.Right - Padding, RowHeight),

                    Text = node.Name,
                    CanHover = false,

                    ParentAnchor = AnchorMode.All,
                });

                button.MouseClick += (o, e) => ButtonClicked?.Invoke();
                text.SizeChanged += (o, e) => Height = text.Height;
            }

        }
    }
}
