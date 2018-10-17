using Shanism.Client.Assets;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Editor.Actions;
using System.Collections.Generic;
using System.Numerics;

namespace Shanism.Editor.UI
{
    /// <summary>
    /// History undo redo etc.
    /// </summary>
    class HistoryBar
    {
        readonly ActionList history;

        public Button Undo { get; }
        public Button Redo { get; }
        public Button Save { get; }

        public IEnumerable<Button> All()
        {
            yield return Undo;
            yield return Save;
            yield return Redo;
        }

        public HistoryBar(ActionList history, IconCache icons)
        {
            this.history = history;

            Button btn(string tooltip, string icon) => new Button
            {
                Padding = Control.DefaultPadding,
                Size = new Vector2(0.10f),
                ToolTip = tooltip,
                IconName = icon,
            };

            Undo = btn("Undo", "anticlockwise-rotation");
            Save = btn("Save", "save");
            Redo = btn("Redo", "clockwise-rotation");

            Undo.MouseClick += (o, e) => history.Undo();
            Save.MouseClick += (o, e) => { /* TODO */};
            Redo.MouseClick += (o, e) => history.Redo();

            history.ActionDone += _ => RefreshUI();
            history.ActionUndone += _ => RefreshUI();

            RefreshUI();
        }

        void RefreshUI()
        {
            Undo.CanHover = history.Current != null;
            Undo.ToolTip = Undo.CanHover ? $"Undo {history.Current.Description}" : null;

            Save.CanHover = history.Current != null;

            Redo.CanHover = history.Next != null;
            Redo.ToolTip = Redo.CanHover ? $"Redo {history.Next.Description}" : null;
        }
    }
}
