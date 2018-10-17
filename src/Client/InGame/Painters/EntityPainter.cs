using Ix.Math;
using Shanism.Common;
using System.Numerics;

namespace Shanism.Client.Systems
{
    /// <summary>
    /// Draws all objects and entities on the screen.
    /// </summary>
    public class EntityPainter
    {

        readonly EntitySystem controller;

        public EntityPainter(EntitySystem controller)
        {
            this.controller = controller;
        }

        public bool DebugMode { get; set; } = true;

        public void Draw(CanvasStarter c)
        {
            using (c.BeginInGame())
            {
                foreach (var sprite in controller.Sprites)
                {

                    if (sprite.Texture != null)
                    {
                        var bounds = sprite.InGameBounds;
                        c.SpriteBatch.ShanoDraw(sprite.Texture, sprite.SourceRectangle, bounds,
                            sprite.Tint, sprite.DrawDepth, sprite.SpriteEffects, sprite.Orientation);
                    }

                    if (DebugMode)
                    {
                        var entity = sprite.Entity;
                        var bounds = new RectangleF(entity.Position - new Vector2(entity.Scale / 2), new Vector2(entity.Scale));
                        var blank = c.BlankTexture;
                        c.SpriteBatch.ShanoDraw(blank.Texture, blank.Bounds, bounds,
                            new Color(255, 0, 0, 48), sprite.DrawDepth - 0.0001f);
                    }
                }
            }
        }
    }
}
