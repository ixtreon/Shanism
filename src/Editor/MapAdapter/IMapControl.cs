using Shanism.Client;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Assets;

namespace Shanism.Editor.MapAdapter
{
    /// <summary>
    /// A map control used in the editor. 
    /// Provides a <see cref="SpriteBatch"/> for custom drawing
    /// and a link to the custom content loaded by the editor. 
    /// </summary>
    interface IEditorMapControl
    {
        SpriteBatch SpriteBatch { get; }

        EditorContent EditorContent { get; }

        TextureCache DefaultContent { get; }

        IClientEngine GameClient { get; }

    }
}
