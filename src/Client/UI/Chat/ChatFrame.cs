using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Shanism.Common;

namespace Shanism.Client.UI.Chat
{
    class ChatFrame : Control
    {
        public static readonly Vector DefaultSize = new Vector(0.7, 0.4);

        public static readonly int MaxLineCount = 256;

        readonly List<IChatSource> sources = new List<IChatSource>();

        public TextureFont Font { get; set; }

        readonly LinkedList<string> lines = new LinkedList<string>();

        LinkedListNode<string> firstLineShown;

        public ChatFrame(params IChatSource[] sources)
        {
            Size = DefaultSize;
            BackColor = Color.Black.SetAlpha(100);
            Font = Content.Fonts.SmallFont;

            this.sources.AddRange(sources);
            foreach (var src in sources)
                src.ChatMessageSent += onMessageReceived;
        }

        public void AddSource(IChatSource chatSource)
        {
            Debug.Assert(!sources.Contains(chatSource));

            sources.Add(chatSource);
            chatSource.ChatMessageSent += onMessageReceived;
        }

        public override void OnDraw(Canvas g)
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
