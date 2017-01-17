using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shanism.Network.Serialization;

namespace Shanism.Network.Common
{
    public class DiffMethodCache
    {
        public static readonly DiffMethodCache Default = new DiffMethodCache();

        public Dictionary<Type, MethodInfo> Reads { get; } = new Dictionary<Type, MethodInfo>();
        public Dictionary<Type, MethodInfo> Writes { get; } = new Dictionary<Type, MethodInfo>();

        public DiffMethodCache()
        {
            FindWriteMethods();
            FindReadMethods();
        }

        public void FindWriteMethods()
        {
            Debug.WriteLine("Finding write methods...");
            Writes.Clear();

            var mis = typeof(FieldWriter).GetMethods();
            foreach (var m in mis)
            {
                var mParams = m.GetParameters();
                if (mParams.Length != 2)
                    continue;

                var paramType = mParams[0].ParameterType;
                if (paramType == mParams[1].ParameterType)
                {
                    Writes.Add(paramType, m);
                }
            }
            Debug.WriteLine($"Added write methods for: {string.Join(", ", Writes.Keys.Select(t => t.Name))}");
        }


        public void FindReadMethods()
        {
            Debug.WriteLine("Finding read methods...");
            Reads.Clear();

            var mis = typeof(FieldReader).GetMethods();
            foreach (var m in mis)
            {
                var mParams = m.GetParameters();
                if (mParams.Length != 1)
                    continue;

                var paramType = mParams[0].ParameterType;
                if (paramType == m.ReturnType)
                {
                    Reads.Add(paramType, m);
                }
            }
            Debug.WriteLine($"Added write methods for: {string.Join(", ", Reads.Keys.Select(t => t.Name))}");
        }
    }
}
