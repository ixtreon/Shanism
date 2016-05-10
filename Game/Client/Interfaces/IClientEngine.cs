using Shanism.Client.Textures;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Objects;
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
    /// Is also a <see cref="IShanoClient"/> that can play on a <see cref="IShanoEngine"/>. 
    /// </summary>
    public interface IClientEngine : IShanoClient
    {

        TextureCache Textures { get; }

        /// <summary>
        /// Sets the receptor that allows a client to communicate with an underlying <see cref="IShanoEngine"/>. 
        /// </summary>
        /// <param name="receptor">The receptor as supplied by a game engine.</param>
        void SetServer(IReceptor receptor);

        /// <summary>
        /// Loads the default game content.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Causes the client to update its state. 
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Causes the client to re-draw itself. 
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        void Draw(GameTime gameTime);

        /// <summary>
        /// Informs the client of changes to the size of
        /// the underlying game window. 
        /// </summary>
        /// <param name="rect">The rect.</param>
        void SetWindowSize(Point sz);

        /// <summary>
        /// Modifies the current state of the in-game camera. 
        /// </summary>
        /// <param name="cameraPos">The new center of the camera, in in-game coordinates.</param>
        /// <param name="windowSz">The new window size, in in-game units.</param>
        void SetCameraParams(Vector? cameraPos = null, IEntity lockedEntity = null, Vector? windowSz = null);

        /// <summary>
        /// Toggles the visibility of the default UI.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> the UI gets visible.</param>
        void ToggleUI(bool visible);

        /// <summary>
        /// Converts the given in-game point to an on-screen point. 
        /// </summary>
        /// <param name="gamePos">The in-game position.</param>
        /// <returns>The on-screen position of the given point. </returns>
        Vector GameToScreen(Vector gamePos);

        /// <summary>
        /// Converts the given on-screen point to an in-game point. 
        /// </summary>
        /// <param name="gamePos">The on-screen position.</param>
        /// <returns>The in-game position of the given point. </returns>
        Vector ScreenToGame(Vector screenPos);
    }
}
