using Shanism.Common.Game;
using Shanism.Common.StubObjects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Message.Server;
using System.IO;

namespace Shanism.Common.Message.Network
{
    /// <summary>
    /// A custom message type containing an array of bytes.
    /// Used when beaming the client/server frames. 
    /// </summary>
    [ProtoContract]
    public class GameFrameMessage : IOMessage
    {
        public override MessageType Type => MessageType.GameFrame;

        [ProtoMember(1)]
        public uint FrameNumber;

        [ProtoMember(2)]
        public readonly byte[] Data;

        GameFrameMessage() { }

        public GameFrameMessage(uint frameNumber, byte[] datas)
        {
            FrameNumber = frameNumber;
            Data = datas;
        }
    }
}
