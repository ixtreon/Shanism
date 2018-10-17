using Ix.Math;
using ProtoBuf.Meta;
using System.Numerics;

namespace Shanism.Network.ProtoBuf
{
    static class ProtoConfig
    {
        public static void Initialize()
        {
            RuntimeTypeModel.Default.Add(typeof(Point), false)
                .Add(nameof(Point.X), nameof(Point.Y));

            RuntimeTypeModel.Default.Add(typeof(Vector2), false)
                .Add(nameof(Vector2.X), nameof(Vector2.Y));

            RuntimeTypeModel.Default.Add(typeof(Rectangle), false)
                .Add(nameof(Rectangle.X), nameof(Rectangle.Y), nameof(Rectangle.Width), nameof(Rectangle.Height));

            RuntimeTypeModel.Default.Add(typeof(RectangleF), false)
                .Add(nameof(RectangleF.Size), nameof(RectangleF.Position));

        }
    }
}
