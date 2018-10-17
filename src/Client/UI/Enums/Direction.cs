using System;
using System.Numerics;

namespace Shanism.Client
{
    /// <summary>
    /// Determines how elements in a list wrap.
    /// </summary>
    public enum ContentWrap
    {
        /// <summary>
        /// Elements will try to fit in a single line.
        /// </summary>
        NoWrap,

        /// <summary>
        /// Elements will wrap onto multiple lines,
        /// wrapping top to bottom.
        /// </summary>
        Wrap,

        /// <summary>
        /// Elements will wrap onto multiple lines,
        /// wrapping bottom to top.
        /// </summary>
        WrapReverse,
    }

    public enum ListSizeMode
    {
        Static,
        ResizePrimary,
        ResizeBoth,
    }

    public static class ListSizeModeExt
    {
        public static bool ShouldResizePrimary(this ListSizeMode value)
            => value == ListSizeMode.ResizePrimary
            || value == ListSizeMode.ResizeBoth;

        public static bool ShouldResizeSecondary(this ListSizeMode value)
            => value == ListSizeMode.ResizeBoth;
    }

    public static class ContentWrapExt
    {

        public static bool ShouldWrap(this ContentWrap mode)
            => mode == ContentWrap.Wrap
            || mode == ContentWrap.WrapReverse;

    }


    /// <summary>
    /// An axis-aligned direction.
    /// </summary>
    public enum Direction
    {
        LeftToRight,
        RightToLeft,
        TopDown,
        BottomUp,
    }

    public enum AlignMode
    {
        Start,
        Center,
        End,
        Stretch,
    }


    public static class DirectionExt
    {
        public static Vector2 Unit(this Direction d)
        {
            switch (d)
            {
                case Direction.LeftToRight:
                    return new Vector2(1, 0);

                case Direction.RightToLeft:
                    return new Vector2(-1, 0);

                case Direction.TopDown:
                    return new Vector2(0, 1);

                case Direction.BottomUp:
                    return new Vector2(0, -1);
            }
            throw new NotImplementedException();
        }

        public static Vector2 SecondaryUnit(this Direction d)
        {
            switch (d)
            {
                case Direction.LeftToRight:
                case Direction.RightToLeft:
                    return Vector2.UnitY;

                case Direction.TopDown:
                case Direction.BottomUp:
                    return Vector2.UnitX;
            }
            throw new ArgumentOutOfRangeException();
        }

        public static Axis GetAxis(this Direction d)
        {
            switch (d)
            {
                case Direction.LeftToRight:
                case Direction.RightToLeft:
                    return Axis.Horizontal;

                case Direction.TopDown:
                case Direction.BottomUp:
                    return Axis.Vertical;
            }
            throw new NotImplementedException();
        }
    }
}
