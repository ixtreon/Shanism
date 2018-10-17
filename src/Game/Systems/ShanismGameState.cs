using Shanism.Client.Assets;
using Shanism.Common;
using Shanism.Common.Messages;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;

namespace Shanism.Client.Game.Systems
{
    class ShanismGameState : ClientGameState
    {

        readonly IClient game;
        readonly ShanoReceptor client;

        // systems
        public ChatProxy Chat { get; private set; } = new ChatProxy();

        FogOfWarSystem FogOfWar { get; set; }
        public FloatingTextProvider FloatingText { get; private set; }

        MoveSystem Movement { get; set; }
        public ActionSystem Actions { get; private set; }


        // events
        public event Action<string> GameDisconnected;
        public event Action<ServerMessage> UnhandledMessage;
        public event Action<AbilityCastArgs> PlayerCastAttempt;
        public event Action StartedPlayingEtc;
        // shortcuts
        public ClientState State => client.State;


        public ShanismGameState(IClient game, ShanoReceptor client) 
            : base(game, client.Receptor ?? throw new Exception())
        {
            this.game = game;
            this.client = client;
            this.client.MessageReceived += OnServerMessage;
        }


        public void InitializeGameSystems(Client.UI.Control root)
        {
            Chat = new ChatProxy();
            Chat.MessageSent += client.SendMessage;

            FogOfWar = new FogOfWarSystem(game.Screen, Entities, ScenarioContent.Shaders);

            FloatingText = new FloatingTextProvider(game.Screen, ScenarioContent.Fonts);

            Movement = new MoveSystem(game.Mouse, game.Keyboard, root, client.PlayerState);

            Actions = new ActionSystem(game.Mouse, client.PlayerState, Entities);
        }

        public override void Update(int msElapsed)
        {
            //update the server handle
            client.Update(msElapsed);

            // base systems etc.
            base.Update(msElapsed);

            // our systems
            if (client.State == ClientState.Playing)
            {
                FogOfWar.Update(msElapsed);
                FloatingText.Update(msElapsed);

                Movement.Update(msElapsed);
                Actions.Update(msElapsed);
            }
        }

        public override void Draw(CanvasStarter canvas)
        {
            base.Draw(canvas);
            if (client.State == ClientState.Playing)
            {
                FogOfWar.Draw(canvas);
                FloatingText.Draw(canvas);
            }
        }

        void OnServerMessage(ServerMessage msg)
        {
            switch (msg.Type)
            {
                case ServerMessageType.HandshakeReply:
                    ParseHandshake((HandshakeReply)msg);
                    break;

                case ServerMessageType.Disconnected:
                    Disconnect("Server said so...");
                    break;

                case ServerMessageType.PlayerStatus:
                    Entities.SetHeroId(((PlayerStatus)msg).HeroId);
                    break;

                case ServerMessageType.MapData:
                    Terrain.SetChunkData((MapData)msg);
                    break;

                case ServerMessageType.DamageEvent:
                    var ev = (DamageEvent)msg;
                    if (Entities.TryGetValue(ev.UnitId, out var unit))
                        FloatingText.AddLabel(unit.Position, $"{ev.ValueChange:0}", 
                            Color.Red, FloatingTextStyle.Sprinkle);
                    break;

                case ServerMessageType.Chat:
                    Chat.ParseMessage((ServerChat)msg);
                    break;

                default:
                    ClientLog.Instance.Warning($"Unhandled message type: {(int)msg.Type}");
                    break;
            }
        }


        public void ParseHandshake(HandshakeReply handshake)
        {
            // parse the scenario config
            var reader = new ScenarioConfigReader();
            var result = reader.TryReadBytes(handshake.ScenarioData, "CompiledScenario");
            if (!result.IsSuccessful)
            {
                Disconnect("Unable to parse the scenario config.");
                return;
            }

            // unzip the content
            if (!reader.TryExtractContent(handshake.ContentData, "Scenario/Textures"))
            {
                Disconnect("Could not extract the scenario content.");
                return;
            }

            // load the map content
            var mapContent = new ContentList(game.Screen,
                game.DefaultContent.Fonts,
                game.GraphicsDevice,
                game.ContentLoader,
                "Scenario", 
                result.Value.Content);
            var combinedContent = new ContentList(game.DefaultContent, mapContent);

            // update the content..
            Initialize(combinedContent);

            // raise an event or something
            StartedPlayingEtc?.Invoke();
        }


        public void Disconnect(string reason)
        {
            client.Disconnect();
            GameDisconnected?.Invoke(reason);
        }

        public void RestartScenario() => client.RestartScenario();
        public void StartPlaying() => client.StartPlaying();

        public IEnumerable<string> GetDebugLines()
        {
            yield return $"Client state: {client.State}";
            yield return client.GetDebugString();
        }
    }
}
