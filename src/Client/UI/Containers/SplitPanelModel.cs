using System;
using System.Diagnostics;
using System.Numerics;
using Ix.Math;
using Shanism.Common;

namespace Shanism.Client.UI.Containers
{

    class SplitPanelModel
    {
        /// <summary>
        /// Gets the axis along which the controls are laid out.
        /// </summary>
        public Axis Axis { get; }


        public Control First { get; private set; }
        public Control Second { get; private set; }

        /// <summary>
        /// Gets the size of the panel.
        /// </summary>
        public Vector2 Size { get; private set; } = new Vector2(0.1f);

        /// <summary>
        /// Gets the position of the splitter.
        /// </summary>
        public float Split { get; private set; } = 0.05f;

        /// <summary>
        /// Gets or sets the way the splitter responds to resize events.
        /// </summary>
        public SplitSizeMode SizeMode { get; set; }

        /// <summary>
        /// Gets the width of the splitter.
        /// </summary>
        public float SplitterSize { get; private set; } = 0.01f;


        public SplitPanelModel(Vector2 panelSize, Axis axis, SplitSizeMode sizeMode)
        {
            Axis = axis;

            SetSize(panelSize);
            SetSplit(panelSize.Get(axis) / 2);
            SizeMode = sizeMode;

            UpdateBoth();
        }

        public float SplitterHalfSize => SplitterSize / 2;

        public void SetFirst(Control value)
        {
            if (First != null)
                First.SizeChanged -= SetSplitFromFirst;

            First = value;
            UpdateFirst();

            if (First != null)
                First.SizeChanged += SetSplitFromFirst;

            void SetSplitFromFirst(Control sender, EventArgs args)
            {
                if(SizeMode == SplitSizeMode.SnapToFirst)
                    AutoSetSplit(First.Size.Get(Axis) + SplitterHalfSize);
                else
                    UpdateFirst();
            }
        }

        public void SetSecond(Control value)
        {
            if (Second != null)
                Second.SizeChanged -= SetSplitFromSecond;

            Second = value;
            UpdateSecond();

            if (Second != null)
                Second.SizeChanged += SetSplitFromSecond;

            void SetSplitFromSecond(Control sender, EventArgs args)
            {
                if (SizeMode == SplitSizeMode.SnapToSecond)
                    AutoSetSplit(Size.Get(Axis) - Second.Size.Get(Axis) - SplitterHalfSize);
                else
                    UpdateSecond();
            }
        }


        void AutoSetSplit(float value)
        {
            if (Split.AlmostEqualTo(value))
                return;


            SetSplit(value);
        }

        public void SetSplit(float value)
        {
            value = value.Clamp(SplitterHalfSize, Size.Get(Axis) - SplitterHalfSize);
            if (Split.AlmostEqualTo(value))
                return;

            Split = value;
            UpdateBoth();
        }


        public void SetSize(Vector2 value)
        {
            Debug.Assert(!float.IsNaN(value.X) && !float.IsNaN(value.Y));
            if (Size.Equals(value))
                return;

            var lastPrimarySize = Size.Get(Axis);
            var primarySize = value.Get(Axis);

            switch (SizeMode)
            {
                case SplitSizeMode.FixedFirst:
                    // do nothing
                    break;

                case SplitSizeMode.FixedSecond:
                    Split += primarySize - lastPrimarySize;
                    break;

                case SplitSizeMode.Proportional when primarySize <= 0:
                    Split = 0;
                    break;

                case SplitSizeMode.Proportional when lastPrimarySize <= 0:
                    Split = primarySize / 2;
                    break;

                case SplitSizeMode.Proportional:
                    Split *= primarySize / lastPrimarySize;
                    break;
            }

            Size = value;
            UpdateBoth();
        }


        public RectangleF GetSplitterBounds()
            => Axis == Axis.Horizontal
                ? new RectangleF(Split - SplitterHalfSize, 0, SplitterSize, Size.Y)
                : new RectangleF(0, Split - SplitterHalfSize, Size.X, SplitterSize);

        public bool IsOverSplit(Vector2 pos)
             => pos.IsInside(GetSplitterBounds());

        public void SetSplitterSize(float value)
        {
            SplitterSize = value;
            UpdateBoth();
        }

        void UpdateBoth() { UpdateFirst(); UpdateSecond(); }

        void UpdateFirst()
        {
            if (First == null)
                return;

            First.Bounds = GetBounds(
                0f,
                (Split - SplitterHalfSize)
            ) + new Vector2(0.01f);
        }

        void UpdateSecond()
        {
            if (Second == null)
                return;

            Second.Bounds = GetBounds(
                Split + SplitterHalfSize,
                Size.Get(Axis) - Split - SplitterHalfSize
            ) + new Vector2(0.01f);
        }


        RectangleF GetBounds(float offset, float length)
            => (Axis == Axis.Horizontal)
                ? new RectangleF(offset, 0, length.Clamp(0, Size.X - offset), Size.Y)
                : new RectangleF(0, offset, Size.X, length.Clamp(0, Size.Y - offset));
    }

}
