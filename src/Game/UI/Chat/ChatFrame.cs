using Ix.Math;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Shanism.Client.UI.Chat
{
    class ChatFrame : Control
    {
        public static readonly int MaxLineCount = 256;

        readonly HashSet<IChatSource> sources = new HashSet<IChatSource>();

        public Font Font { get; set; }

        readonly LinkedList<string> lines = new LinkedList<string>();

        public ChatFrame(params IChatSource[] sources)
        {
            BackColor = Color.Black.SetAlpha(100);
            Font = Content.Fonts.SmallFont;

            foreach(var src in sources)
                AddSource(src);
        }

        public void AddSource(IChatSource chatSource)
        {
            if(!sources.Add(chatSource))
                throw new InvalidOperationException($"The chat source `{chatSource}` is already present.");

            chatSource.ChatMessageReceived += onMessageReceived;
        }

        protected override void OnMouseScroll(MouseScrollArgs e)
        {
            base.OnMouseScroll(e);
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

            var yOffset = ClientBounds.Height;
            var l = lines.Last;

            while(l != null && yOffset >= 0)
            {
                var sz = Font.MeasureString(l.Value, ClientBounds.Width);
                yOffset -= sz.Y;

                var offset = ClientBounds.Position + new Vector2(0, yOffset);
                c.DrawString(Font, l.Value, UiColors.Text, new RectangleF(offset, sz));

                l = l.Previous;
            }
        }

        void onMessageReceived(string msg)
        {
            lines.AddFirst(msg);

            if(lines.Count > MaxLineCount)
                lines.RemoveLast();
        }
    }
}
