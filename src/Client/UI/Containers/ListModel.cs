using System.Numerics;

namespace Shanism.Client.UI.Containers
{
    struct ListRow
    {
        public Vector2 LargestControlSize;

        public int ControlCount;

        public void AddItem(Vector2 itemSize)
        {
            LargestControlSize = Vector2.Max(LargestControlSize, itemSize);
            ControlCount++;
        }
    }

    struct ListFlowModel
    {
        /// <summary>
        /// The place at which to put the next control.
        /// </summary>
        public Vector2 CurrentPosition;

        /// <summary>
        /// The size of the list's bounding box so far.
        /// </summary>
        public Vector2 ListSize;

        /// <summary>
        /// Info about the current row.
        /// </summary>
        public ListRow CurrentRow;

        public void Add(Vector2 itemSize, Vector2 spacing, Direction direction)
        {
            ListSize = Vector2.Max(ListSize, CurrentPosition + itemSize);

            CurrentPosition += (itemSize + spacing) * direction.Unit();
            CurrentRow.AddItem(itemSize);
        }

        /// <summary>
        /// Moves to the next line, offset determined by largest control in line so far.
        /// </summary>
        public void NewRow(Vector2 spacing, Direction direction)
        {
            CurrentPosition = (CurrentPosition + CurrentRow.LargestControlSize + spacing) * direction.SecondaryUnit();
            CurrentRow = new ListRow();
        }

    }
}
