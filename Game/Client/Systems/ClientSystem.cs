﻿using Shanism.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Systems
{
    /// <summary>
    /// An abstract system for the client engine. 
    /// Can handle messages and update itself. 
    /// </summary>
    abstract class ClientSystem
    {

        public event Action<IOMessage> MessageSent;


        public virtual void HandleMessage(IOMessage ioMsg) { }

        public virtual void Update(int msElapsed) { }

        internal void SendMessage(IOMessage msg) => MessageSent?.Invoke(msg);
    }

}