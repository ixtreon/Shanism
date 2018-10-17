using Shanism.Client;
using Shanism.Client.UI;

namespace Shanism.Editor.Models.Brushes
{
    abstract class Brush
    {
        protected bool IsApplying { get; private set; }

        public MouseButton TriggerButton { get; }

        public bool IsEnabled { get; set; }

        protected IClient Game { get; }


        protected Brush(BrushArgs args, MouseButton triggerButton)
        {
            Game = args.Game;
            TriggerButton = triggerButton;

            args.Root.MouseDown += OnMouseDown;
            args.Root.MouseMove += OnMouseMove;
            args.Root.MouseUp += OnMouseUp;
        }

        protected virtual void OnMouseDown(Control sender, MouseButtonArgs e)
        {
            if(IsEnabled && e.Button == TriggerButton)
            {
                IsApplying = true;
                ApplyStart(e);
                Apply(e);
            }
        }
        protected virtual void OnMouseMove(Control sender, MouseArgs e)
        {
            if(IsEnabled)
            {
                Hover(e);

                if(IsApplying)
                    Apply(e);
            }
        }
        protected virtual void OnMouseUp(Control sender, MouseButtonArgs e)
        {
            if(IsApplying)
            {
                ApplyEnd(e);
                IsApplying = false;
            }
        }

        protected virtual void Hover(MouseArgs e) { }

        protected virtual void ApplyStart(MouseArgs e) { }
        protected virtual void Apply(MouseArgs e) { }
        protected virtual void ApplyEnd(MouseArgs e) { }

        public abstract void Draw(Canvas c);
    }

    class BrushArgs
    {
        public IClient Game { get; }
        public Control Root { get; }

        public BrushArgs(IClient game, Control root)
        {
            Game = game;
            Root = root;
        }
    }
}
