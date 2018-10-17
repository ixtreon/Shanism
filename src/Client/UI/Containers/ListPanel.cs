using Ix.Math;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using System;

namespace Shanism.Client.UI.Containers
{


    /// <summary>
    /// Orders controls one after the other.
    /// Similar to an HTML flexbox.
    /// </summary>
    public class ListPanel : Control, IEnumerable<Control>
    {
        bool reflowInProgress;

        Direction _direction;
        ContentWrap _wrap;
        ListSizeMode _sizeMode;

        public Vector2 ControlSpacing { get; set; } = new Vector2(DefaultPadding);

        /// <summary>
        /// Gets or sets the direction in which the controls are laid out.
        /// </summary>
        public Direction Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                Reflow();
            }
        }

        public ContentWrap Wrap
        {
            get => _wrap;
            set
            {
                _wrap = value;
                Reflow();
            }
        }

        public ListSizeMode SizeMode
        {
            get => _sizeMode;
            set
            {
                _sizeMode = value;
                Reflow();
            }
        }

        Axis ListAxis => Direction.GetAxis();

        public ListPanel(Direction direction = Direction.LeftToRight,
            ContentWrap wrapMode = ContentWrap.NoWrap,
            ListSizeMode sizeMode = ListSizeMode.Static)
        {
            SizeChanged += reflowEvent;

            _direction = direction;
            _wrap = wrapMode;
            _sizeMode = sizeMode;
        }

        protected override void OnParentChanged(ParentChangeArgs e)
        {
            if (e.PreviousParent != null)
                e.PreviousParent.SizeChanged -= reflowEvent;

            Reflow();

            if (e.Parent != null)
                e.Parent.SizeChanged += reflowEvent;

            base.OnParentChanged(e);
        }

        protected override void OnShown(EventArgs e)
        {
            Reflow();
            base.OnShown(e);
        }

        protected override void OnControlAdded(ControlChildArgs e)
        {
            e.Child.SizeChanged += reflowEvent;

            Reflow();
            base.OnControlAdded(e);
        }

        protected override void OnControlRemoved(ControlChildArgs e)
        {
            e.Child.SizeChanged -= reflowEvent;

            Reflow();
            base.OnControlRemoved(e);
        }

        void reflowEvent(Control o, EventArgs e) => Reflow();

        public void Reflow()
        {
            if (reflowInProgress || Parent == null)
                return;

            reflowInProgress = true;
            ReflowUnsafe();
            reflowInProgress = false;
        }

        void ReflowUnsafe()
        {
            var model = new ListFlowModel();

            var (canWrap, maxPrimarySize) = GetWrapDetails();

            // reflow kiddos
            for (int i = 0; i < Controls.Count; i++)
            {
                var c = Controls[i];
                if (!c.IsVisible)
                    continue;

                var primarySize = (model.CurrentPosition + c.Size).Get(Direction.GetAxis());
                var needToWrap = canWrap
                    && model.CurrentRow.ControlCount > 0
                    && primarySize > maxPrimarySize;

                // see if wrap (newline) is required
                if (needToWrap)
                    model.NewRow(ControlSpacing, Direction);

                c.Location = new Vector2(Padding) + model.CurrentPosition;
                model.Add(c.Size, ControlSpacing, Direction);
            }

            // resize us
            var newSize = new Vector2(2 * Padding) + model.ListSize;
            switch (SizeMode)
            {
                case ListSizeMode.ResizeBoth:
                    ResizeAnchor = ParentAnchor;    // BAAAAAAAAAAAAAAAAAAAAAD
                    Size = newSize;
                    break;

                case ListSizeMode.ResizePrimary when ListAxis == Axis.Horizontal:
                    ResizeAnchor = ParentAnchor;
                    Size = new Vector2(newSize.X, Size.Y);
                    break;

                case ListSizeMode.ResizePrimary when ListAxis == Axis.Vertical:
                    ResizeAnchor = ParentAnchor;
                    Size = new Vector2(Size.X, newSize.Y);
                    break;

            }
        }

        (bool CanWrap, float? MaxSize) GetWrapDetails()
        {
            switch (SizeMode)
            {
                case ListSizeMode.Static:
                    return (Wrap.ShouldWrap(), Size.Get(ListAxis));

                case ListSizeMode.ResizePrimary:
                case ListSizeMode.ResizeBoth:
                    return (Wrap.ShouldWrap(), MaximumSize?.Get(ListAxis) - 2 * Padding);
            }
            throw new ArgumentException(nameof(SizeMode));
        }


        public IEnumerator<Control> GetEnumerator()
            => Controls.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Controls.GetEnumerator();
    }
}
