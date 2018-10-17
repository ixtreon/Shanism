using Microsoft.Xna.Framework.Graphics;

namespace Shanism.Client.Systems
{

    /// <summary>
    /// Displays terrain chunks and sends requests for new ones. 
    /// </summary>
    public class TerrainPainter
    {

        readonly TerrainSystem controller;

        GraphicsDevice graphics => controller.GraphicsDevice;

        public TerrainPainter(TerrainSystem controller)
        {
            this.controller = controller;
        }


        public void Draw()
        {
            controller.UpdateScreen();

            graphics.SetRenderTarget(null);
            graphics.SamplerStates[0] = SamplerState.PointClamp;
            
            //draw all chunks around us
            graphics.Clear(Microsoft.Xna.Framework.Color.Gold);
            foreach (var pass in controller.GetEffectPasses())
            {
                pass.Apply();
                foreach(var chunk in controller.GetNearbyChunks())
                    if(chunk.HasBuffer)
                    {
                        graphics.SetVertexBuffer(chunk.Buffer);
                        graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, 2 * chunk.Area);
                    }
            }
        }
    }

}
