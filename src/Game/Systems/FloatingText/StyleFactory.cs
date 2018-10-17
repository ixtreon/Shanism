using System;
using System.Numerics;

namespace Shanism.Client.Game.Systems.FloatingText
{
    interface IStyleFactory
    {
        void CalcParams(Vector2 pos, out Vector2 v, out Vector2 a);
    }

    class UpFactory : IStyleFactory
    {
        public void CalcParams(Vector2 pos, out Vector2 v, out Vector2 a)
        {
            v = new Vector2(0, -12);
            a = new Vector2(0, 10);
        }
    }

    class DownFactory : IStyleFactory
    {
        public void CalcParams(Vector2 pos, out Vector2 v, out Vector2 a)
        {
            v = new Vector2(0, 12);
            a = new Vector2(0, -10);
        }
    }

    class RainbowFactory : IStyleFactory
    {
        int xMult = 1;

        public void CalcParams(Vector2 pos, out Vector2 v, out Vector2 a)
        {
            v = new Vector2(xMult * 2.5f, -7);
            a = new Vector2(0, 10);

            xMult = -xMult;
        }
    }

    class SprinkleFactory : IStyleFactory
    {
        const float Velocity = -12;
        const float Gravity = 10;

        const float StartAngle = (float)Math.PI / 6;   // arc from 30° to 150°
        const int StepCount = 7;     // integral angles at: 6, 10

        const float StepSize = ((float)Math.PI - 2 * StartAngle) / (StepCount - 1);

        int atStep;

        public void CalcParams(Vector2 pos, out Vector2 v, out Vector2 a)
        {
            var angle = StartAngle + atStep * StepSize;
            var directionVector = Vector2.Zero.PolarProjection(angle, 1);
            v = Velocity * directionVector;
            a = Gravity * directionVector;

            atStep++;
            if(atStep == StepCount) atStep = 0;
        }
    }
}
