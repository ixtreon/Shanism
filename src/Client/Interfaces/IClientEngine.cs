using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Drawing;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Shanism.Client
{
    /// <summary>
    /// An instance of the game client engine. 
    /// Contains the logic necessary to draw and update the ShanoRpg. 
    /// Implements the <see cref="IShanoClient"/> interface to play on a <see cref="IShanoEngine"/>. 
    /// </summary>
    public interface IClientEngine : IShanoClient
    {

        TextureCache Textures { get; }

        RenderTarget2D RenderTarget { get; set; }

        /// <summary>
        /// Attempts to connect to the specified engine. Returns the receptor in case we succeeded.
        /// </summary>
        /// <param name="engine">The engine to connect to.</param>
        /// <param name="receptor">The receptor for the engine, in case we connected to it.</param>
        bool TryConnect(IShanoEngine engine, string playerName, out IReceptor receptor);

        void RestartScenario();

        /// <summary>
        /// Causes the client to update its state. 
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        void Update(int msElapsed);

        /// <summary>
        /// Causes the client to re-draw itself. 
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        void Draw();

        /// <summary>
        /// Sets the size of the game-window buffers in pixels.
        /// </summary>
        /// <param name="sz">The new size of the window.</param>
        void SetWindowSize(Point sz);

        /// <summary>
        /// Moves the camera.
        /// </summary>
        /// <param name="inGamePos">The in game position.</param>
        /// <param name="inGameSz">The in game sz.</param>
        void MoveCamera(Vector? inGamePos, Vector? inGameSz);

        /// <summary>
        /// Toggles the visibility of the default UI.
        /// </summary>
        /// <param name="isDesignMode">if set to <c>true</c> the UI gets invisible.</param>
        void SetDesignMode(bool isDesignMode);

        /// <summary>
        /// Converts the given in-game point to an on-screen point. 
        /// </summary>
        /// <param name="gamePos">The in-game position.</param>
        /// <returns>The on-screen position of the given point. </returns>
        Vector GameToScreen(Vector gamePos);

        /// <summary>
        /// Converts the given on-screen point to an in-game point. 
        /// </summary>
        /// <param name="screenPos">The on-screen position.</param>
        /// <returns>The in-game position of the given point. </returns>
        Vector ScreenToGame(Vector screenPos);
    }
}
