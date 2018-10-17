using Ix.Math;
using Ix.Math.Simd;
using System;

namespace Shanism.Common
{

    public interface IStats
    {
        float[] RawStats { get; }
    }

    public static class StatsExtensions
    {

        public static void Set<T>(this T destinationObject, T sourceObject) where T : IStats
            => Array.Copy(sourceObject.RawStats, destinationObject.RawStats, destinationObject.RawStats.Length);

        public static void Add<T>(this T destinationObject, T addedObject) where T : IStats
            => destinationObject.RawStats.Add(addedObject.RawStats);

        public static void Multiply<T>(this T destinationObject, T addedObject) where T : IStats
            => destinationObject.RawStats.Multiply(addedObject.RawStats);
    }
}
