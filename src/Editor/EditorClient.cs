using Microsoft.Xna.Framework;
using Shanism.Client;
using Shanism.Editor.Views;

namespace Shanism.Editor
{
    /// <summary>
    /// This is the main type for our editor.
    /// </summary>
    public class EditorClient : ShanismClient
    {
        MainMenu mainView;

        public string StartupMap { get; set; }

        public EditorClient()
        {
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void LoadGame()
        {
            base.LoadGame();
        }

        protected override void OnGameLoaded()
        {
            Views.SetMain(mainView = new MainMenu { StartupMap = StartupMap });
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
