namespace Shanism.Common
{
    /// <summary>
    /// A game engine as seen by a player of the game.
    /// 
    /// Acts as a gateway to new players willing to join the game. 
    /// Pairs each accepted client to a corresponding <see cref="IEngineReceptor"/> to play. 
    /// </summary>
    public interface IEngine
    {
        bool IsLocal { get; }

        ServerState State { get; }

        /// <summary>
        /// Decides whether to accept the given client to the server. 
        /// If the client is accepted returns the receptor to use for communication with the server. Otherwise returns null. 
        /// </summary>
        IEngineReceptor Connect(IClientReceptor c);



        void OpenToNetwork();

        bool TryRestartScenario(out string errors);

        void Update(int msElapsed);

        bool Disconnect(string name);

    }

}
