using Shanism.Common;
using Shanism.Common.Entities;
using Shanism.Common.Messages;
using Shanism.Common.ObjectStubs;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Editor.Game
{
    /// <summary>
    /// Represents both a receptor and an engine.
    /// </summary>
    class EditorEngine : IEngineReceptor
    {

        readonly ITerrainMap map;
        readonly List<IEntity> entities = new List<IEntity>();

        public event Action<ServerMessage> MessageHandler;

        public EditorEngine(ScenarioConfig scenario)
        {
            map = scenario.Map.GetTerrainMap();
            entities = scenario.Map.Objects
                .Select((x, i) => new UnitStub
                {
                    Position = x.Location,
                    Model = x.Model,
                    CurrentTint = x.Tint ?? Color.White,
                    Scale = x.Size,
                })
                .Cast<IEntity>()
                .ToList();
        }

        public void HandleMessage(ClientMessage msg)
        {
            if (MessageHandler == null)
                throw new Exception("Noone to handle our messagess.....");

            switch (msg.Type)
            {
                case ClientMessageType.MapRequest:
                    var span = ((MapRequest)msg).Chunk.Bounds;
                    var buf = new TerrainType[span.Area];

                    map.Get(span, ref buf);

                    MessageHandler(new MapData(span, buf));
                    break;
            }
        }

        // IReceptor implementation

        bool IEngineReceptor.IsHost => true;
        uint IEngineReceptor.PlayerId { get; } = 1;
        IReadOnlyCollection<IEntity> IEngineReceptor.VisibleEntities => entities;
        void IEngineReceptor.Disconnect() => throw new InvalidOperationException();
        void IEngineReceptor.StartPlaying() => throw new InvalidOperationException();
        string IEngineReceptor.GetDebugString() => "Shanism is life...";
    }
}
