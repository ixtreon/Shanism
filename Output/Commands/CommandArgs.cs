using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace IO.Commands
{
    [ProtoContract]
    [ProtoInclude(1, typeof(ActionArgs))]
    [ProtoInclude(2, typeof(MovementArgs))]
    public abstract class CommandArgs
    {
        public readonly CommandType CommandType;

        protected CommandArgs(CommandType cmd)
        {
            CommandType = cmd;
        }
    }
}
