using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;

namespace Network.Objects.Serializers
{

    public interface ISerializerModule
    {
        bool CanSerialize(Type ty);

        void Serialize(NetBuffer buf, Type ty, object o);

        object Deserialize(NetBuffer buf, Type ty);
    }

    public static class Serializer
    {

        static List<ISerializerModule> modulesInUse = new List<ISerializerModule>
        {
            new GameObjectSerializer(),
            new PrimitiveSerializer(),
            new EnumSerializer(),
        };

        public static bool TryWrite(NetBuffer buf, Type ty, object obj)
        {
            var module = modulesInUse.FirstOrDefault(m => m.CanSerialize(ty));
            if (module == null)
                return false;

            module.Serialize(buf, ty, obj);
            return true;
        }

        public static bool CanWrite(Type ty)
        {
            return modulesInUse.Any(m => m.CanSerialize(ty));
        }

        public static bool TryRead(NetBuffer buf, Type ty, ref object obj)
        {
            var module = modulesInUse.FirstOrDefault(m => m.CanSerialize(ty));

            if (module == null)
                return false;

            obj = module.Deserialize(buf, ty);
            return true;
        }

        public static bool TryWrite<T>(NetBuffer buf, T obj)
        {
            return TryWrite(buf, typeof(T), obj);
        }

        public static bool TryRead<T>(NetBuffer buf, ref T obj)
        {
            object o = null;
            var retval = TryRead(buf, typeof(T), ref o);
            obj = (T)o;
            return retval;
        }

        public static T Read<T>(NetBuffer buf)
        {
            T val = default(T);
            TryRead(buf, ref val);
            return val;
        }

        public static void WriteInterface<TInt>(this NetBuffer msg, object obj)
        {
            //get T's public properties ordered by name
            var ps = typeof(TInt).GetProperties()
                .OrderBy(pi => pi.Name);

            foreach (var p in ps)
            {
                var ty = p.PropertyType;
                var val = p.GetValue(obj);

                if(!TryWrite(msg, ty, val))
                {
                    Log.Warning("Unable to serialize field {0} of interface {1}", p.Name, typeof(TInt).Name);
                    continue;
                }
            }

        }

        public static TObj ReadInterface<TInt, TObj>(this NetBuffer msg)
            where TObj : TInt
        {
            //get T's public properties ordered by name
            var ps = typeof(TInt).GetProperties()
                .OrderBy(pi => pi.Name);

            var obj = Activator.CreateInstance<TObj>();

            foreach (var p in ps)
            {
                var ty = p.PropertyType;
                object val = null;

                if (!TryRead(msg, ty, ref val))
                {
                    Log.Warning("Unable to deserialize field {0} of interface {1}", p.Name, typeof(TInt).Name);
                    continue;
                }

                p.SetValue(obj, val);
            }

            return obj;
        }
    }
}
