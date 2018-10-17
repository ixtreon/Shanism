using Shanism.Common.Util;
using System.Collections.Generic;

namespace Shanism.Engine.Models.Systems
{
    class SystemList : List<GameSystem>
    {
        readonly PerfCounter perfCounter;

        public SystemList(PerfCounter perfCounter)
        {
            this.perfCounter = perfCounter;
        }

        public void Update(int msElapsed)
        {
            for(int i = 0; i < Count; i++)
            {
                var sys = this[i];

                perfCounter.Start(sys.Name);
                sys.Update(msElapsed);
            }

            perfCounter.End();
        }
    }
}
