using Shanism.Client;
using Shanism.Common;
using MGWinForms;
using Microsoft.Xna.Framework.Graphics;
using System;
using Shanism.Client.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Shanism.Editor.MapAdapter
{
    /// <summary>
    /// A map control used in the editor. 
    /// Provides a <see cref="SpriteBatch"/> for custom drawing
    /// and a link to the custom content loaded by the editor. 
    /// </summary>
    /// <seealso cref="GameControl" />
    /// <seealso cref="IEditorMapControl" />
    class EditorControl : GameControl, IEditorMapControl
    {


        IClientEngine _client;
        SpriteBatch _spriteBatch;
        EditorContent _editorContent;


        public event Action ClientLoaded;
        public event Action OnDraw;


        #region IEditorMapControl implementation

        public IClientEngine GameClient => _client;

        public TextureCache DefaultContent => _client.Textures;

        public EditorContent EditorContent => _editorContent;

        public SpriteBatch SpriteBatch => _spriteBatch;

        #endregion


        #region Overrides

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _client?.SetWindowSize(new Point(Width, Height));
        }

        protected override void Draw(GameTime gameTime)
        {
            _client.Draw();

            _spriteBatch.Begin();
            OnDraw?.Invoke();
            _spriteBatch.End();
        }

        protected override void LoadContent()
        {
            _editorContent = new EditorContent(Services);
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //create the client, load its content
            _client = ClientFactory.CreateGameEngine(GraphicsDeviceService, _editorContent);
            _client.SetWindowSize(new Point(Width, Height));
            _client.SetDesignMode(true);


            ClientLoaded?.Invoke();
        }

        protected override void Update(GameTime gameTime)
        {
            var msElapsed = (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            _client.Update(msElapsed);
        }
        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // EditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "EditorControl";
            this.Size = new System.Drawing.Size(723, 421);
            this.ResumeLayout(false);

        }
    }
}
