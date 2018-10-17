using System;

namespace Shanism.Client.Systems
{
    /// <summary>
    /// Makes drawing circles with integral co-ordinates fast and easy.
    /// Walks 1/8 of the boundary of a circle with the given radius.
    /// </summary>
    public struct CircleWalker
    {
        public int X, Y;
        int dp;

        /// <summary>
        /// Begins walking the boundary of a circle with the specified radius.
        /// The walk is started at position (0, <paramref name="radius"/>)
        /// and continues as long as Y >= X. 
        /// </summary>
        public CircleWalker(int radius)
        {
            if (radius < 0)
                throw new ArgumentOutOfRangeException($"The radius must be positive but was {radius}. ", nameof(radius));

            X = 0;
            Y = radius;
            dp = (5 - radius * 4) / 4;
        }

        public bool Step()
        {
            if(Y < X)
                return false;

            X++;

            if(dp < 0)
            {
                dp += 2 * X + 1;
            }
            else
            {
                Y--;
                dp += 2 * (X - Y) + 1;
            }

            return Y >= X;
        }
    }
}
