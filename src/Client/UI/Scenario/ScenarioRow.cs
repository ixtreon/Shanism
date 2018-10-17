using Shanism.Common;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.IO;
using Microsoft.Xna.Framework.Input;

namespace Shanism.Client.UI.Scenario
{
    class ScenarioRow : Button
    {
        public const float CollapsedHeight = 0.07f + 2 * LargePadding;
        public const float ExpandedHeight = 3 * CollapsedHeight;

        public readonly ScenarioConfig Scenario;

        readonly Label author;
        readonly Label name;
        readonly Label description;
        readonly Button select;

        public event Action<ScenarioConfig> ScenarioSelected;

        string authorName => string.IsNullOrWhiteSpace(Scenario.Author)
            ? "Unknown"
            : Scenario.Author;


        public ScenarioRow(ScenarioConfig sc)
        {
            //Padding = 0;
            Scenario = sc;
            Size = new Vector2(0.6f, 0.1f);
            CanFocus = true;

            Add(name = new Label
            {
                Text = sc.Name,
                Font = Content.Fonts.FancyFont,
                TextColor = Color.Black,

                Padding = 0,
                LineHeight = 1,

                Location = ClientBounds.Position,
                Width = ClientBounds.Width,
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,

                CanHover = false,
            });

            Add(author = new Label
            {
                Text = $"Author: {authorName}",
                Font = Content.Fonts.NormalFont,
                TextColor = Color.Black,
                TextAlign = AnchorPoint.TopRight,

                Padding = DefaultPadding,
                LineHeight = 1,

                Top = ClientBounds.Top,
                Right = ClientBounds.Right + DefaultPadding,
                ParentAnchor = AnchorMode.Top | AnchorMode.Right,

                CanHover = false,
                IsVisible = false,
            });

            Add(select = new Button
            {
                Text = "Play",
                BackColor = UiColors.ButtonConfirm,

                Width = 0.20f,
                Right = ClientBounds.Right,
                Bottom = ClientBounds.Bottom,
                ParentAnchor = AnchorMode.Bottom | AnchorMode.Right,

                IsVisible = false,
            });

            select.MouseClick += (o, e) => ScenarioSelected?.Invoke(Scenario);

            ToolTip = sc.BaseDirectory;
        }

        protected override void OnKeyPress(KeyboardArgs e)
        {
            base.OnKeyPress(e);

            if (e.Key == Keys.Escape)
                ClearFocus();
        }

        protected override void OnMouseClick(MouseButtonArgs e)
        {
            base.OnMouseClick(e);

            SetFocus();
        }
        protected override void OnFocusLost(EventArgs e)
        {
            base.OnFocusLost(e);

            Height = CollapsedHeight;
            author.IsVisible = select.IsVisible = false;
            Cursor = GameCursor.ClickMe;
        }

        protected override void OnFocusGained(EventArgs e)
        {
            base.OnFocusGained(e);

            Height = ExpandedHeight;
            author.IsVisible = select.IsVisible = true;
            Cursor = GameCursor.Default;
        }
    }
}
