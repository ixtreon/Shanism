using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;
using Shanism.ScenarioLib;
using Microsoft.Xna.Framework.Graphics;

using Color = Microsoft.Xna.Framework.Color;
using Shanism.Client;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using System.Windows.Forms;
using Shanism.Common;

namespace Shanism.Editor.MapAdapter
{
    class TerrainBrush : Brush
    {

        readonly TerrainType Terrain;
        readonly int Size;

        readonly bool IsCircle;

        Point lastBrushPlaced = new Point(int.MaxValue);

        bool isPlacing;

        public TerrainBrush(IEditorEngine engine, TerrainType tty, int sz, bool isCircle)
            : base(engine)
        {
            IsCircle = isCircle;
            Terrain = tty;
            Size = sz;
            IsCircle = isCircle;
        }

        public override IOMessage Place(Vector inGamePos)
        {
            if (lastBrushPlaced == inGamePos.ToPoint())
                return null;
            lastBrushPlaced = inGamePos.ToPoint();

            //change the scenario 
            foreach (var p in GetShape(inGamePos))
                Map.Terrain[p.X, p.Y] = Terrain;

            //prepare the client message
            var span = GetSpan(inGamePos);
            var mapData = new TerrainType[span.Area];
            foreach (var pt in span.Iterate())
            {
                var localPt = pt - span.Position;
                var id = localPt.X + span.Width * localPt.Y;
                mapData[id] = Map.Terrain[pt.X, pt.Y];
            }

            var msg = new MapDataMessage(span, mapData);
            return msg;
        }

        IEnumerable<Point> GetShape(Vector pos)
        {
            var span = GetSpan(pos);
            if (IsCircle)
            {
                var d = Math.Pow(Size / 2.0, 2.0);
                foreach (var p in span.Iterate())
                    if (span.Center.DistanceToSquared((Vector)p + 0.5) <= d)
                        yield return p;
            }
            else
            {
                foreach (var p in span.Iterate())
                    yield return p;
            }
        }

        public Rectangle GetSpan(Vector inGamePos)
        {
            var ll = (inGamePos - (Size / 2.0)).Round()
                .Clamp(Point.Zero, Map.Size - 1);
            var ur = (inGamePos + (Size / 2.0)).Round()
                .Clamp(Point.One, Map.Size);
            return new Rectangle(ll, ur - ll);
        }

        public override void OnDraw(IEditorMapControl control, Vector inGamePos)
        {
            var span = GetSpan(inGamePos);

            var blankTex = control.EditorContent.Blank;
            foreach (var p in GetShape(inGamePos))
            {
                var ll = control.Client.GameToScreen(p);
                var ur = control.Client.GameToScreen(p + 1);

                control.SpriteBatch.ShanoDraw(blankTex, ll, ur - ll, Color.Blue.SetAlpha(100));
            }
        }
    }
}
