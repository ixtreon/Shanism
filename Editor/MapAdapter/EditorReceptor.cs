using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Message;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Editor.MapAdapter
{
    class EditorReceptor : IReceptor
    {
        readonly EditorEngine server;


        public uint Id { get; }

        public string Name { get; } = "ShanoEdit";

        public IReadOnlyCollection<IEntity> VisibleEntities
            => server.VisibleEntities;


        public event Action<IOMessage> MessageSent;



        public EditorReceptor(EditorEngine server, uint pId)
        {
            this.server = server;
            Id = pId;
        }


        public void SendMessage(IOMessage msg) => MessageSent?.Invoke(msg);

        public string GetDebugString() => "All is OK!";
    }
}
