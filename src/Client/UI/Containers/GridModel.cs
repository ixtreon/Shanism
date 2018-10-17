using Ix.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Shanism.Client.UI.Containers
{

    public class GridElement
    {
        float _size = 1f;

        public GridCellSizeMode SizeMode { get; set; } = GridCellSizeMode.Proportional;

        public float Size
        {
            get => _size;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                _size = value;
            }
        }
    }

    public enum GridCellSizeMode
    {
        Fixed, Proportional,
    }

    class GridModel
    {
        const int InitialElements = 2;

        GridElement[] rows = new GridElement[InitialElements];
        float[] rowOffsets = new float[InitialElements];

        GridElement[] columns = new GridElement[InitialElements];
        float[] colOffsets = new float[InitialElements];


        Point gridSize = new Point(InitialElements, InitialElements);
        Vector2 controlSize;

        public Point GridSize
        {
            get => gridSize;
            set => ResizeGrid(value);
        }

        public Vector2 ControlSize
        {
            get => controlSize;
            set => ResizeControl(value);
        }

        public IReadOnlyList<GridElement> Rows => rows;
        public IReadOnlyList<GridElement> Columns => columns;

        float CellX(int x) => colOffsets[x];
        float CellY(int y) => rowOffsets[y];
        float CellWidth(int x) => (x == 0) ? colOffsets[x] : (colOffsets[x] - colOffsets[x - 1]);
        float CellHeight(int y) => (y == 0) ? rowOffsets[y] : (rowOffsets[y] - rowOffsets[y - 1]);

        public RectangleF GetCellBounds(int x, int y)
            => new RectangleF(
                CellX(x), CellY(y),
                CellWidth(x), CellHeight(y));

        void ResizeGrid(Point newSize)
        {
            var newColumnCount = expand(columns.Length, newSize.X);
            var newRowCount = expand(rows.Length, newSize.Y);

            var resizeColumns = (newColumnCount != columns.Length);
            var resizeRows = (newRowCount != rows.Length);

            if (resizeColumns)
            {
                Array.Resize(ref columns, newColumnCount);
                Array.Resize(ref colOffsets, newColumnCount);
            }

            if (resizeRows)
            {
                Array.Resize(ref rows, newRowCount);
                Array.Resize(ref rowOffsets, newRowCount);
            }

            GridSize = newSize;

            int expand(int cur, int desired)
            {
                while (cur < desired)
                    cur *= 2;
                return cur;
            }
        }

        public void ResizeControl(Vector2 newSize)
        {
            if (!ControlSize.X.Equals(newSize.X))
                doResize(columns, colOffsets, newSize.X);

            if (!ControlSize.Y.Equals(newSize.Y))
                doResize(rows, rowOffsets, newSize.Y);

            ControlSize = newSize;


            void doResize(GridElement[] elements, float[] offsets, float totalSize)
            {
                var pSize = totalSize;
                var pWeights = 0f;
                for (int i = 0; i < elements.Length; i++)
                {
                    if (elements[i].SizeMode == GridCellSizeMode.Fixed)
                        pSize -= elements[i].Size;
                    else
                        pWeights += elements[i].Size;
                }

                if (pWeights == 0)
                    pWeights = 1;

                float getSize(int i)
                {
                    if (elements[i].Size <= 0)
                        return 0;

                    switch (elements[i].SizeMode)
                    {
                        case GridCellSizeMode.Fixed:
                            return elements[i].Size;

                        case GridCellSizeMode.Proportional when pWeights == 0:
                            return 0;

                        case GridCellSizeMode.Proportional:
                            return pSize * elements[i].Size / pWeights;
                    }
                    throw new NotImplementedException();
                }


                var offset = 0f;
                for (int i = 0; i < elements.Length; i++)
                {
                    offset += getSize(i);
                    offsets[i] = offset;
                }
            }
        }
    }
}
