using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Containers
{
    public class TabControl : Control
    {
        public static float DefaultBarHeight { get; } = 0.1f;

        readonly ListPanel tabBar;
        readonly List<TabNode> nodes = new List<TabNode>();

        int curId;
        float? _tabWidth;

        /// <summary>
        /// If set to a non-null value, determines the tab width.
        /// </summary>
        public float? TabWidth
        {
            get => _tabWidth;
            set => setTabWidths(value);
        }

        public float TabHeight
        {
            get => tabBar.Height;
            set => tabBar.Height = value;
        }


        void setTabWidths(float? value)
        {
            _tabWidth = value;
            foreach (var n in nodes)
                n.UpdateTabWidth(value);
        }

        public TabControl()
        {
            BackColor = UiColors.ControlBackground;
            Size = new Vector2(0.5f);
            Add(tabBar = new ListPanel(Direction.LeftToRight)
            {
                Padding = 0,

                Location = ClientBounds.Position,
                Size = new Vector2(ClientBounds.Width, 0.1f),
                ParentAnchor = AnchorMode.Top | AnchorMode.Horizontal,
                ControlSpacing = new Vector2(Padding, 0),
            });
        }

        public TabControl(params string[] tabNames)
            : this()
        {
            foreach (var tabName in tabNames)
                Add(tabName);
        }

        public Control this[int id]
        {
            get => nodes[id].Panel;
        }

        public Control this[string tabName]
        {
            get => nodes
                .Single(n => n.Button.Text.Equals(tabName, StringComparison.InvariantCulture))
                .Panel;
        }

        public void Add(string tabName)
            => Add(tabName, new Control());

        public void Add(string tabName, Control panel)
        {
            var nodeId = nodes.Count;

            // tab panel
            panel.Location = new Vector2(tabBar.Left, tabBar.Bottom);
            panel.Size = ClientBounds.Size - panel.Location;
            panel.ParentAnchor = AnchorMode.All;
            panel.IsVisible = !nodes.Any();
            Add(panel);

            var btn = new ToggleButton
            {
                Text = tabName,
                Height = tabBar.ClientBounds.Height,
                StickyToggle = true,
                ParentAnchor = AnchorMode.Left | AnchorMode.Vertical,
                BackColor = UiColors.ControlBackground.MixWith(Color.Transparent, 0.5f),
                SelectedColor = UiColors.ControlBackground,
                IsSelected = (nodes.Count == curId),
            };
            tabBar.Add(btn);

            // node = btn + panel
            var n = new TabNode { Button = btn, Panel = panel };
            n.UpdateTabWidth(TabWidth);
            nodes.Add(n);

            btn.MouseDown += (o, e) => onTabClick(nodeId);
        }

        void onTabClick(int id)
        {
            if (curId == id)
                return;

            nodes[curId].Panel.Hide();
            curId = id;
            nodes[curId].Panel.Show();

            OnTabChanged(id);
        }

        public event Action<int> TabChanged;

        protected virtual void OnTabChanged(int tabId) => TabChanged?.Invoke(tabId);
    }

    struct TabNode
    {
        public ToggleButton Button { get; set; }
        public Control Panel { get; set; }

        public void UpdateTabWidth(float? fixedSize)
        {
            if (fixedSize != null)
                Button.Width = fixedSize.Value;
            else
                Button.FitToText();
        }
    }
}
