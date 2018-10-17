using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.IO;
using Shanism.Client.Systems;
using Shanism.Common;
using System.Numerics;

namespace Shanism.Client.Game.Systems
{
    class FogOfWarSystem : ISystem
    {
        readonly EntitySystem entities;
        readonly ScreenSystem screen;
        readonly Effect fogShader;

        public FogOfWarSystem(ScreenSystem screen, EntitySystem entities, ShaderCache shaders)
        {
            this.entities = entities;
            this.screen = screen;

            fogShader = shaders.FogOfWar;
        }


        public void Draw(CanvasStarter starter)
        {
            if(fogShader != null)
            {
                using (var c = starter.BeginShader(fogShader))
                    c.FillRectangle(Vector2.Zero, screen.UiSize, Color.White);
            }
        }

        public void Update(int msElapsed)
        {
            if(fogShader != null && entities.Hero != null)
            {
                fogShader.Parameters["TexSize"].SetValue(screen.Game.Size.ToXnaVector());
                fogShader.Parameters["SightRange"].SetValue(entities.Hero.VisionRange);
            }
        }
    }
}
