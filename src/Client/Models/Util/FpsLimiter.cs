using System;
using System.Diagnostics;
using System.Threading;

namespace Shanism.Client
{
    class FpsLimiter
    {
        readonly Stopwatch gameTimer = Stopwatch.StartNew();

        float lastFrame;
        float nextFrame;

        public float TargetUpdatePeriod { get; set; }

        public int ActNow()
        {
            // if we were quicker, sleep some
            var d = nextFrame - gameTimer.Elapsed.TotalMilliseconds;
            if (d > 0)
                Thread.Sleep((int)Math.Max(d - 1, 0));

            // get new msElapsed
            var t = (float)gameTimer.Elapsed.TotalMilliseconds;
            var msElapsed = (int)(t - lastFrame + 0.5f);

            // don't let nextFrame fall behind
            if (nextFrame < t - 1000)
                nextFrame = t;

            // update last/next frame
            nextFrame += TargetUpdatePeriod;
            lastFrame = t;

            return msElapsed;
        }
    }
}
