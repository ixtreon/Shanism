using Client;
using IO;
using IO.Common;
using MGWinForms;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GameTime = Microsoft.Xna.Framework.GameTime;
using XKeys = Microsoft.Xna.Framework.Input.Keys;

namespace ShanoEditor.MapAdapter
{
    class EditorClient : GameControl
    {
        IClientEngine clientEngine;

        public event Action GameLoaded;

        public IClientEngine Engine {  get { return clientEngine; } }

        public EditorClient()
        {
            
        }


        protected override void OnMouseWheel(MouseEventArgs e)
        {

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            clientEngine?.WindowSizeChanged(new Rectangle(0, 0, Width, Height));
        }

        protected override void Draw(GameTime gameTime)
        {
            clientEngine.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            var content = new ContentManager(Services);

            clientEngine = ShanoGame.CreateClientEngine("WorldEdit", GraphicsDeviceService, content);
            clientEngine.LoadContent();
            clientEngine.WindowSizeChanged(new Rectangle(0, 0, Width, Height));

            GameLoaded?.Invoke();

            Engine.ToggleUI(false);
        }

        protected override void Update(GameTime gameTime)
        {
            clientEngine.Update(gameTime);
        }

    }
}
