using Shanism.Client.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Client.UI.Chat
{
    class ChatFrame : Control
    {
        public static readonly Vector DefaultSize = new Vector(0.7, 0.4);

        public static readonly int MaxLineCount = 256;

        IChatProvider chatProvider;

        public TextureFont Font { get; set; }

        readonly LinkedList<string> lines = new LinkedList<string>();

        LinkedListNode<string> firstLineShown;

        public ChatFrame()
        {
            Size = DefaultSize;
            BackColor = Color.Black.SetAlpha(100);
            Font = Content.Fonts.SmallFont;
        }

        public void SetProvider(IChatProvider provider)
        {
            if (chatProvider != null)
                chatProvider.ChatSent -= onMessageReceived;

            chatProvider = provider;

            if (chatProvider != null)
                chatProvider.ChatSent += onMessageReceived;
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);

            if (!lines.Any())
                return;

            var curLine = firstLineShown ?? lines.First;
            var curY = Size.Y - Padding;
            var linesLeft = getNLinesShown();

            do
            {
                g.DrawString(Font, curLine.Value, Color.White, new Vector(Padding, curY), 0, 1);

                curLine = curLine.Next;
                curY -= (Padding + Font.HeightUi);
                linesLeft--;
            }
            while (curLine != null && linesLeft > 0);
        }

        int getNLinesShown()
            => (int)((Size.Y - Padding) / (Font.HeightUi + Padding));

        void onMessageReceived(string msg)
        {
            lines.AddFirst(msg);

            if (lines.Count > MaxLineCount)
                lines.RemoveLast();
        }
    }
}
