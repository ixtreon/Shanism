//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using JitIntrinsicAttribute = System.Numerics.JitIntrinsicAttribute;
//using SimdVector = System.Numerics.Vector2;
//using System.Runtime.CompilerServices;

//namespace UnitTests
//{
//    [TestClass]
//    public class VecTests
//    {
//        const long N_ITERATIONS = 100_000_000;

//        [Test]
//        public void Perf_ShanoVector()
//        {
//            for (long i = 0; i < N_ITERATIONS; i++)
//            {
//                var a = new Vector2(5f, 5f);
//                var b = new Vector2(10f, 10f);

//                var c = (a + b) * (b - a) / a;
//            }
//        }
//        [Test]
//        public void Perf_SimdVector()
//        {
//            for (long i = 0; i < N_ITERATIONS; i++)
//            {
//                var a = new SimdVector(5, 5);
//                var b = new SimdVector(10, 10);

//                var c = (a + b) * (b - a) / a;
//            }
//        }


//        struct Vector2
//        {
//            SimdVector val;

//            [JitIntrinsic]
//            public Vector2(Single x, Single y)
//                => val = new SimdVector(x, y);

//            public Vector2(SimdVector v)
//                => val = v;

//            [JitIntrinsic]
//            [MethodImpl(MethodImplOptions.AggressiveInlining)]
//            public static Vector2 operator +(Vector2 left, Vector2 right)
//                => new Vector2(left.val + right.val);

//            [JitIntrinsic]
//            [MethodImpl(MethodImplOptions.AggressiveInlining)]
//            public static Vector2 operator -(Vector2 left, Vector2 right)
//                => new Vector2(left.val - right.val);

//            [JitIntrinsic]
//            [MethodImpl(MethodImplOptions.AggressiveInlining)]
//            public static Vector2 operator *(Vector2 left, Vector2 right)
//                => new Vector2(left.val * right.val);

//            [JitIntrinsic]
//            [MethodImpl(MethodImplOptions.AggressiveInlining)]
//            public static Vector2 operator /(Vector2 left, Vector2 right)
//                => new Vector2(left.val / right.val);
//        }

//    }
//}

//namespace System.Numerics
//{
//    /// <summary>
//    /// An attribute that can be attached to JIT Intrinsic methods/properties
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property)]
//    internal class JitIntrinsicAttribute : Attribute
//    {
//    }
//}
