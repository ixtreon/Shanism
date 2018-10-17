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
    public class DiffMethodCache : MethodCache<FieldWriter, FieldReader>
    {

    }

    public class MethodCache<TWriter, TReader>
    {
        public Dictionary<Type, MethodInfo> Reads { get; } = new Dictionary<Type, MethodInfo>();
        public Dictionary<Type, MethodInfo> Writes { get; } = new Dictionary<Type, MethodInfo>();

        public MethodCache()
        {
            FindWriteMethods();
            FindReadMethods();
        }

        public void FindWriteMethods()
        {
            Debug.WriteLine("Finding write methods...");

            Writes.Clear();
            foreach (var m in typeof(TWriter).GetMethods())
            {
                var ps = m.GetParameters();
                var p0 = ps[0].ParameterType;
                if (ps.Length == 2 && p0 == ps[1].ParameterType)
                    Writes.Add(p0, m);
            }

            Debug.WriteLine($"Added write methods for: {string.Join(", ", Writes.Keys.Select(t => t.Name))}");
        }


        public void FindReadMethods()
        {
            Debug.WriteLine("Finding read methods...");

            Reads.Clear();
            foreach (var m in typeof(TReader).GetMethods())
            {
                var ps = m.GetParameters();
                var p0 = ps[0].ParameterType;
                if (ps.Length == 1 && p0 == m.ReturnType)
                    Reads.Add(p0, m);
            }

            Debug.WriteLine($"Added read methods for: {string.Join(", ", Reads.Keys.Select(t => t.Name))}");
        }
    }
}
