using Shanism.Client;
using Shanism.Common;
using Shanism.Common.Game;
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
using Shanism.Editor.Views.Maps;
using Shanism.Client.Textures;

namespace Shanism.Editor.MapAdapter
{
    class EditorControl : GameControl, IEditorMapControl
    {
        static readonly string PlayerName = "WorldEdit";


        IClientEngine _client;
        SpriteBatch _spriteBatch;
        EditorContent _editorContent;


        public event Action ClientLoaded;
        public event Action OnDraw;


        #region IEditorMapControl implementation

        public IClientEngine Client => _client;

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
            _client.Draw(gameTime);

            _spriteBatch.Begin();
            OnDraw?.Invoke();
            _spriteBatch.End();
        }

        protected override void LoadContent()
        {
            _editorContent = new EditorContent(Services);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //create the client, load its content
            _client = ShanoGame.CreateClientEngine(PlayerName, GraphicsDeviceService, _editorContent);
            _client.LoadContent();
            _client.SetWindowSize(new Point(Width, Height));
            _client.ToggleUI(false);


            ClientLoaded?.Invoke();
        }

        protected override void Update(GameTime gameTime)
        {
            _client.Update(gameTime);
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
