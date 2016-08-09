using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Client.Input
{
    class InputThread
    {
        const int FPS = 60;

        Thread thread;

        Stopwatch sw;

        public bool ContinueRunning { get; set; } = true;

        long lastTick = 0;
        long nowTick;

        public void Start()
        {
            sw = Stopwatch.StartNew();
            lastTick = 0;
            tick();

            thread = new Thread(loop);
            thread.Start();
        }

        void loop()
        {

            while (ContinueRunning)
                tick();
        }

        void tick()
        {
            nowTick = sw.ElapsedMilliseconds;
            var msElapsed = (int)(nowTick - lastTick);

            KeyboardInfo.Update(msElapsed);
            MouseInfo.Update(msElapsed);

            lastTick = nowTick;
        }
    }
}
