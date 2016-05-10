using Shanism.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    /// <summary>
    /// A system for the client engine. 
    /// Can handle messages and update itself. 
    /// </summary>
    interface IClientSystem
    {
        void HandleMessage(IOMessage msg);

        void Update(int msElapsed);
    }
}
