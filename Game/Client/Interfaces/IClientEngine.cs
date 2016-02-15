using IO;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Client
{
    /// <summary>
    /// A client engine. Contains the logic necessary to draw the game. 
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
        void SetCameraParams(Vector? cameraPos = null, Vector? windowSz = null);

        void ToggleUI(bool visible);


        Vector GameToScreen(Vector gamePos);

        Vector ScreenToGame(Vector screenPos);
    }
}
