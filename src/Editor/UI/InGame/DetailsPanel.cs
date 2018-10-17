using Shanism.Client;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Editor.Controllers;
using System;

namespace Shanism.Editor.UI.InGame
{
    /// <summary>
    /// Lets the user change basic scenario properties like name or author.
    /// </summary>
    class DetailsPanel : ListPanel
    {

        readonly TextLabel title;
        readonly TextLabel author;
        readonly TextLabel description;
        readonly DetailsController controller;

        public DetailsPanel(DetailsController controller)
        {
            this.controller = controller;

            BackColor = UiColors.ControlBackground;
            Bounds = new Ix.Math.RectangleF(0, 0, 0.5f, 0.5f);
            Direction = Direction.TopDown;

            title = CreateRow("Title", controller.Name);
            author = CreateRow("Author", controller.Author);
            description = CreateRow("Description", controller.Description);

            title.TextChanged += (o, e) => controller.SetName(title.ValueText);
            author.TextChanged += (o, e) => controller.SetAuthor(author.ValueText);
            description.TextChanged += (o, e) => controller.SetDescription(description.ValueText);

            //VisibleChanged += ScenarioMenu_VisibleChanged;
        }

        TextLabel CreateRow(string text, string value)
        {
            var row = new TextLabel
            {
                Text = text,

                Width = ClientBounds.Width,
                SplitAt = 0.4f,

                ParentAnchor = AnchorMode.Top | AnchorMode.Horizontal,

                ValueText = value,
            };

            Add(row);

            return row;
        }

    }
}
