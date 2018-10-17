using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Assets;
using Shanism.Client.IO;
using Shanism.Client.Systems;
using System;

namespace Shanism.Client
{

    public interface IGameWindow : IActivatable
    {

        /// <summary>
        /// Gets all available info about the game window.
        /// </summary>
        ScreenSystem Screen { get; }

        /// <summary>
        /// Gets the current state of the mouse.
        /// </summary>
        MouseSystem Mouse { get; }

        /// <summary>
        /// Gets the current state of the keyboard.
        /// </summary>
        KeyboardSystem Keyboard { get; }

    }

    /// <summary>
    /// The shanism game as seen by the in-game components.
    /// Provides access to basically all IO. 
    /// </summary>
    public interface IClient : IGameWindow
    {

        /// <summary>
        /// Gets the current hierarchy of views displayed in the game.
        /// </summary>

        RenderBuffer DrawBuffer { get; }

        GraphicsDevice GraphicsDevice { get; }

        ContentList DefaultContent { get; }

        ContentManager ContentLoader { get; }

        /// <summary>
        /// Exits the game. Unused
        /// </summary>
        void Exit();
    }

    /// <summary>
    /// Info and controls for the currently running game.
    /// </summary>
    public interface IGameContext
    {
        //bool IsConnected { get; }

        //IEngine Engine { get; }

        //IClientReceptor Client { get; }

        //ClientGameState StartPlaying(IEngine engine);

        //void RestartGame();

        //void Update(int msElapsed);

        //event Action<GameViewBase> GameStarted;

    }
}
