using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using System;

namespace IconPacker
{
    class CustomPngQuantizer : PaletteQuantizer
    {

        readonly Rgba32GrayscalePaletteQuantizer quantizer;

        public CustomPngQuantizer(int nBits) : base(false)
        {
            if (nBits < 1 || nBits > 4)
                throw new ArgumentException("Number of bits must be from 1 to 4.", nameof(nBits));

            quantizer = new Rgba32GrayscalePaletteQuantizer(this, nBits);
        }

        public override IFrameQuantizer<TPixel> CreateFrameQuantizer<TPixel>()
        {
            if (typeof(TPixel) == typeof(Rgba32))
                return (IFrameQuantizer<TPixel>)quantizer;

            throw new NotImplementedException($"Cannot handle pixel type `{typeof(TPixel).FullName}`");
        }


        class Rgba32GrayscalePaletteQuantizer : FrameQuantizerBase<Rgba32>
        {
            readonly Rgba32[] colors;

            public Rgba32GrayscalePaletteQuantizer(IQuantizer quantizer, int nBits)
                : base(quantizer, true)
            {
                colors = new Rgba32[(int)Math.Pow(2, nBits)];
                for (int i = 0; i < colors.Length; i++)
                    colors[i] = new Rgba32(255, 255, 255, (byte)(255 * i / (colors.Length - 1)));
            }

            protected override Rgba32[] GetPalette() => colors;

            int GetIndex(int a) => ((colors.Length - 1) * a + 127) / 255;

            protected override void SecondPass(ImageFrame<Rgba32> source, Span<byte> output, ReadOnlySpan<Rgba32> palette, int width, int height)
            {
                for (int i = 0; i < output.Length; i++)
                {
                    var y = i / width;
                    var x = i - (y * width);

                    output[i] = (byte)GetIndex(source[x, y].A);
                }
            }
        }
    }
}
