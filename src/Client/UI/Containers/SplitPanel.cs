using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

namespace Shanism.Client.UI.Containers
{

    public class SplitPanel : Control
    {

        readonly SplitPanelModel model;


        /// <summary>
        /// Gets or sets whether the user can change the split ratio. 
        /// True by default.
        /// </summary>
        public bool AllowUserResize { get; set; } = true;

        public Color SplitterColor { get; set; }

        public SplitSizeMode SizeMode
        {
            get => model.SizeMode;
            set => model.SizeMode = value;
        }

        public float SplitterWidth
        {
            get => model.SplitterSize;
            set => model.SetSplitterSize(value);
        }

        /// <summary>
        /// Gets or sets the point at which the split is done. 
        /// </summary>
        public float SplitAt
        {
            get => model.Split;
            set => model.SetSplit(value);
        }

        public Control First
        {
            get => model.First;
            set
            {
                if (model.First != null) Remove(model.First);
                model.SetFirst(value);
                if (model.First != null) Add(model.First);
            }
        }

        public Control Second
        {
            get => model.Second;
            set
            {
                if (model.Second != null) Remove(model.Second);
                model.SetSecond(value);
                if (model.Second != null) Add(model.Second);
            }
        }

        public SplitPanel(Axis axis)
        {
            SplitterColor = UiColors.WindowActiveTitle; // some bullshit color

            Size = new Vector2(0.1f);
            model = new SplitPanelModel(ClientBounds.Size, axis, SplitSizeMode.Proportional);

            // resize updates model & view
            SizeChanged += (o, e) => model.SetSize(ClientBounds.Size);

            // mouse move updates the cursor
            MouseMove += (o, e) =>
            {
                if (!CanResizeNow(e))
                    Cursor = GameCursor.Default;
                else
                    Cursor = model.Axis == Axis.Horizontal ? GameCursor.SizeH : GameCursor.SizeV;
            };

            // mouse drag updates split position
            (this).AddMouseDragEvent(MouseButton.Left,
                (sender, args) => CanResizeNow(args),
                (sender, args) => model.SetSplit(GetPosition(args).Get(model.Axis))
            );

        }

        Vector2 GetPosition(MouseArgs args) => args.Position - ClientBounds.Position;

        bool CanResizeNow(MouseArgs args) => AllowUserResize && model.IsOverSplit(GetPosition(args));

        public override void Draw(Canvas c)
        {
            // draw splitter
            if (AllowUserResize)
                c.FillRectangle(model.GetSplitterBounds() + ClientBounds.Position, SplitterColor);

            base.Draw(c);
        }
    }
}
