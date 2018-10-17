using Ix.Math;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Shanism.Client.IO
{
    public class UiTransform
    {
        readonly IScreen screen;


        /// <summary>
        /// Gets the UI-to-pixel scaling factor. 
        /// </summary>
        public float Scale { get; private set; }

        public RectangleF Bounds { get; private set; }

        /// <summary>
        /// Gets the size of the screen in UI units. 
        /// </summary>
        public Vector2 Size => Bounds.Size;


        internal UiTransform(IScreen screen)
        {
            this.screen = screen;
        }

        public void SetScale(float value)
        {
            Scale = value;
            Bounds = new RectangleF(Vector2.Zero, (Vector2)screen.WindowSize / Scale);
        }

        public float ToScreen(float size) => size * Scale;
        public Vector2 ToScreen(Vector2 position) => position * Scale;
        public RectangleF ToScreen(RectangleF bounds) => bounds * Scale;

        public float FromScreen(float size) => size / Scale;
        public Vector2 FromScreen(Vector2 position) => position / Scale;
        public RectangleF FromScreen(RectangleF bounds) => bounds / Scale;
    }
}
