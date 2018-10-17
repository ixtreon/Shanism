using Ix.Math;
using Shanism.Client.UI;
using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.Models.UI
{
    class MouseClickTracker
    {
        // config
        public static TimeSpan DoubleClickMaxDelay { get; } = TimeSpan.FromMilliseconds(500);
        public static float MaxClickDistance { get; } = 0.01f;

        // default instance
        public static MouseClickTracker Default { get; } = new MouseClickTracker();

        // state
        readonly ClickInfo[] lastClicks = new ClickInfo[(int)Enum<MouseButton>.MaxValue + 1];

        public bool IsDoubleClick(MouseButton button, Control target, Vector2 position)
        {
            var last = lastClicks[(int)button];
            var cur = new ClickInfo(target, DateTime.UtcNow, position);
            
            lastClicks[(int)button] = cur;

            return cur.Target == last.Target
                && (cur.Timestamp - last.Timestamp) < DoubleClickMaxDelay
                && (cur.Position - last.Position).NormL1() < MaxClickDistance;
        }

        struct ClickInfo
        {
            public ClickInfo(Control target, DateTime timestamp, Vector2 position)
            {
                Target = target;
                Timestamp = timestamp;
                Position = position;
            }

            public Control Target { get; }
            public DateTime Timestamp { get; }
            public Vector2 Position { get; }
        }
    }
}
