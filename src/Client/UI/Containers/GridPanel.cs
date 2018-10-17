using Ix.Math;
using System;
using System.Collections.Generic;

namespace Shanism.Client.UI.Containers
{
    public partial class GridPanel : Control
    {
        const int MaxCells = 256;

        Control[,] cells = new Control[MaxCells, MaxCells];

        GridModel model = new GridModel();

        public Point GridSize
        {
            get => model.GridSize;
            set
            {
                var oldSize = model.GridSize;

                // model
                model.GridSize = value;

                // view
                var newCells = new Control[value.X, value.Y];
                for (int ix = 0; ix < value.X; ix++)
                    for (int iy = 0; iy < value.Y; iy++)
                        if (ix < oldSize.X && iy < oldSize.Y)
                            newCells[ix, iy] = cells[ix, iy];
                        else
                            newCells[ix, iy] = new GridCell();

                cells = newCells;
            }
        }

        public IReadOnlyList<GridElement> Columns => model.Columns;
        public IReadOnlyList<GridElement> Rows => model.Rows;

        public GridPanel()
        {

        }

        protected override void OnSizeChanged(EventArgs e)
        {
            // model
            model.ControlSize = Size;

            // view
            for (int ix = 0; ix < model.GridSize.X; ix++)
                for (int iy = 0; iy < model.GridSize.Y; iy++)
                    cells[ix, iy].Bounds = model.GetCellBounds(ix, iy);

            base.OnSizeChanged(e);
        }


        public Control this[int x, int y]
        {
            get => cells[x, y];
        }
    }

    class GridCell : Control
    {

    }

}
