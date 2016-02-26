using Client;
using IO;
using IO.Common;
using MGWinForms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GameTime = Microsoft.Xna.Framework.GameTime;
using Color = Microsoft.Xna.Framework.Color;

namespace ShanoEditor.MapAdapter
{
    class EditorControl : GameControl, IEditorMapControl
    {
        static readonly string PlayerName = "WorldEdit";


        IClientEngine _engine;

        SpriteBatch _spriteBatch;

        EditorContent _editorContent;



        public event Action GameLoaded;

        public event Action OnDraw;


        #region IEditorMapControl implementation

        public IClientEngine Engine => _engine;

        public EditorContent EditorContent => _editorContent;

        public SpriteBatch SpriteBatch => _spriteBatch;

        #endregion



        #region Overrides


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _engine?.WindowSizeChanged(new Rectangle(0, 0, Width, Height));
        }

        protected override void Draw(GameTime gameTime)
        {
            _engine.Draw(gameTime);

            _spriteBatch.Begin();
            OnDraw?.Invoke();
            _spriteBatch.End();
        }

        protected override void LoadContent()
        {
            _editorContent = new EditorContent(Services);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _engine = ShanoGame.CreateClientEngine(PlayerName, GraphicsDeviceService, _editorContent);
            _engine.LoadContent();
            _engine.WindowSizeChanged(new Rectangle(0, 0, Width, Height));
            _engine.ToggleUI(false);


            GameLoaded?.Invoke();
        }

        protected override void Update(GameTime gameTime)
        {
            _engine.Update(gameTime);
        }
        #endregion
    }
}
