using System.Collections.Generic;

namespace Shanism.Editor.Models.Brushes
{
    class BrushCollection : Dictionary<EditorBrushType, Brush>
    {

        public EditorBrushType CurrentType { get; private set; }

        public Brush CurrentBrush { get; private set; }


        public void SetBrush(EditorBrushType value)
        {
            if (CurrentBrush != null)
                CurrentBrush.IsEnabled = false;

            CurrentType = value;
            CurrentBrush = this[value];

            CurrentBrush.IsEnabled = true;
        }
    }
}
