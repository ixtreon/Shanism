using IO;
using IO.Common;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Client
{
    /// <summary>
    /// An instance of the game client engine. 
    /// Contains the logic necessary to draw and update the ShanoRpg. 
    /// Is also a <see cref="IShanoClient"/> that can play on a <see cref="IShanoEngine"/>. 
    /// </summary>
    public interface IClientEngine : IShanoClient
    {
        void SetServer(IReceptor receptor);

        void Draw(GameTime gameTime);

        void Update(GameTime gameTime);

        void LoadContent();

        void WindowSizeChanged(Rectangle rect);

        /// <summary>
        /// Modifies the current state (position and span) of the camera. 
        /// </summary>
        /// <param name="cameraPos">The in-game center of the camera. </param>
        /// <param name="windowSz">The window size. </param>
        void SetCameraParams(Vector? cameraPos = null, IEntity lockedEntity = null, Vector? windowSz = null);

        void ToggleUI(bool visible);


        Vector GameToScreen(Vector gamePos);

        Vector ScreenToGame(Vector screenPos);
    }
}
