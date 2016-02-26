using Client;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoEditor.MapAdapter
{
    /// <summary>
    /// A map control used in the editor. 
    /// Provides a spritebatch for custom drawing
    /// and a link to the custom content loaded by the editor. 
    /// </summary>
    interface IEditorMapControl
    {
        SpriteBatch SpriteBatch { get; }

        EditorContent EditorContent { get; }

        IClientEngine Engine { get; }
    }
}
