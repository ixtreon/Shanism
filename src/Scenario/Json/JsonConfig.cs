using Ix.Math;
using Newtonsoft.Json;
using System;
using System.Numerics;

namespace Client.Common.Json
{
    public static class JsonConfig
    {


        static readonly TypedJsonConverter RectangleConverter = new JsonSpanConverter<Rectangle, int>(4,
            rect => new[] { rect.X, rect.Y, rect.Width, rect.Height },
            arr => new Rectangle(arr[0], arr[1], arr[2], arr[3])
        );

        static readonly TypedJsonConverter RectangleFConverter = new JsonSpanConverter<RectangleF, float>(4,
            rect => new[] { rect.X, rect.Y, rect.Width, rect.Height },
            arr => new RectangleF(arr[0], arr[1], arr[2], arr[3])
        );

        static readonly TypedJsonConverter PointConverter = new JsonSpanConverter<Point, int>(2,
            rect => new[] { rect.X, rect.Y },
            arr => new Point(arr[0], arr[1])
        );

        static readonly TypedJsonConverter Vector2Converter = new JsonSpanConverter<Vector2, float>(2,
            rect => new[] { rect.X, rect.Y },
            arr => new Vector2(arr[0], arr[1])
        );

        public static void Initialize()
        {
            JsonConvert.DefaultSettings = CreateSettings(new[]
            {
                PointConverter,
                Vector2Converter,
                RectangleConverter,
                RectangleFConverter,
            });
        }

        static Func<JsonSerializerSettings> CreateSettings(params TypedJsonConverter[] converters)
            => () => new JsonSerializerSettings
            {
                ContractResolver = new GenericJsonResolver(converters),
            };
    }


}
