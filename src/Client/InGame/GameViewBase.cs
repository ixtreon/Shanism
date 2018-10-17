using Shanism.Common.Messages;

namespace Shanism.Client.Views
{
    public abstract class GameViewBase : View
    {

        readonly ClientGameState game;

        public GameViewBase(ClientGameState game)
        {
            this.game = game;
        }

        protected override void OnReload()
        {
        }

        public void SetChunkData(MapData message) => game.SetChunkData(message);

        /// <summary>
        /// Draws the terrain and UI
        /// </summary>
        /// <param name="canvas"></param>
        protected override void OnDraw(CanvasStarter canvas)
        {
            game.Draw(canvas);
        }

        /// <summary>
        /// Updates the UI and the in-game controller.
        /// </summary>
        /// <param name="msElapsed"></param>
        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
            game.Update(msElapsed);
        }
    }
}
