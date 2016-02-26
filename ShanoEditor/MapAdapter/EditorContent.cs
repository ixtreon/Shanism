using Client;
using IO.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Color = Microsoft.Xna.Framework.Color;
namespace ShanoEditor.MapAdapter
{
    class EditorContent : ContentManager
    {
        public Texture2D Blank { get; }

        public Texture2D Circle { get; }
        public Texture2D CircleOutline { get; }


        public EditorContent(IServiceProvider serviceProvider)
            : base(serviceProvider, "EditorContent")
        {
            Blank = Load<Texture2D>("1");
            Circle = Load<Texture2D>("circle");
            CircleOutline = Load<Texture2D>("circle-outline");
        }
    }

    static class ContentExt
    {
        public static void ShanoDraw(this SpriteBatch sb, Texture2D tex, Vector pos, Vector sz, Color? c = null)
        {
            sb.Draw(tex,
                position: pos.ToVector2(),
                scale: (sz / new Vector(tex.Width, tex.Height)).ToVector2(),
                color: c ?? Color.White);
        }
    }
}
