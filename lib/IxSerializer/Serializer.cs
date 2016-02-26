using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IxSerializer.Modules;

namespace IxSerializer
{

    public static class Serializer
    {

        static readonly List<SerializerModule> modulesInUse = new List<SerializerModule>
        {
            new PrimitiveSerializer(),
            new EnumValueSerializer(),
            new EnumerableSerializer(),
            //new AttributeSerializer(),
            new Array1DSerializer(),
            //new ArrayKDSerializer(),
            new InterfaceSerializer(),
        };

        static readonly Dictionary<Type, SerializerModule> mappedTypes = new Dictionary<Type, SerializerModule>();

        /// <summary>
        /// Gets all the modules that are currently in use. 
        /// </summary>
        public static IEnumerable<SerializerModule> ModulesInUse { get { return modulesInUse; } }

        public static void Initialize()
        {
            foreach (var m in ModulesInUse)
                m.Initialize();
        }

        /// <summary>
        /// Adds a new module to the serializer. 
        /// </summary>
        /// <param name="module">The module to add. </param>
        public static void AddModule(SerializerModule module)
        {
            if (!modulesInUse.Contains(module))
                modulesInUse.Add(module);
        }

        /// <summary>
        /// Returns whether the provided type can be read or written by this serializer. 
        /// </summary>
        public static bool CanParse(Type ty)
        {
            return mappedTypes.ContainsKey(ty) || modulesInUse.Any(m => m.CanSerialize(ty));
        }

        #region Object (de)serialization
        /// <summary>
        /// Tries to deserialize the given type to an new object object of the same type. 
        /// </summary>
        /// <param name="buf">The BinaryReader input stream. </param>
        /// <param name="ty">The type of the object to be deserialized. </param>
        /// <param name="obj">The object that is to be deserialized. </param>
        /// <returns>Whether the object was successfully deserialized. </returns>
        public static bool TryRead(BinaryReader buf, Type ty, out object obj)
        {
            //used cache'd version when finding reader module
            var readerModule = getModule(ty);
            if (readerModule == null)
            {
                obj = null;
                return false;
            }


            obj = readerModule.Deserialize(buf, ty);
            return true;
        }

        /// <summary>
        /// A type-safe wrapper around <see cref="TryRead(BinaryReader, Type, out object)"/>
        /// </summary>
        /// <typeparam name="T">The type of the object to be deserialized. </typeparam>
        /// <param name="buf">The BinaryReader input stream. </param>
        /// <param name="obj">The object that is to be deserialized. </param>
        /// <returns>Whether the object was successfully deserialized. </returns>
        public static bool TryRead<T>(BinaryReader buf, out T obj)
        {
            object o;
            if (!TryRead(buf, typeof(T), out o) || !(o is T))
            {
                obj = default(T);
                return false;
            }

            obj = (T)o;
            return true;
        }

        public static T Read<T>(BinaryReader buf)
        {
            return (T)Read(buf, typeof(T));
        }

        public static object Read(BinaryReader buf, Type ty)
        {
            object obj;
            if (!TryRead(buf, ty, out obj))
                throw new Exception($"Unable to read an object of type `{ty}` from the stream. ");
            return obj;
        }

        /// <summary>
        /// Tries to serialize the given object of a specified type. 
        /// </summary>
        /// <param name="buf">The BinaryWriter used to write the object. </param>
        /// <param name="ty">The type of the object to be serialized. </param>
        /// <param name="obj">The object that is to be serialized. </param>
        /// <returns>Whether the object was successfully serialized. </returns>
        public static bool TryWrite(BinaryWriter buf, Type ty, object obj)
        {
            var module = getModule(ty);
            if (module == null)
                return false;

            module.Serialize(buf, ty, obj);
            return true;
        }

        /// <summary>
        /// A type-safe wrapper around <see cref="TryWrite(BinaryWriter, Type, object)"/>
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized. </typeparam>
        /// <param name="buf">The BinaryWriter used to write the object. </param>
        /// <param name="obj">The object that is to be serialized. </param>
        /// <returns>Whether the object was successfully serialized. </returns>
        public static bool TryWrite<T>(BinaryWriter buf, T obj)
        {
            return TryWrite(buf, typeof(T), obj);
        }

        public static void Write(BinaryWriter buf, Type ty, object obj)
        {
            if (!TryWrite(buf, ty, obj))
                throw new Exception("Unable to write object as type " + ty.Name);
        }
        public static void Write<T>(BinaryWriter buf, T obj)
        {
            if (!TryWrite(buf, obj))
                throw new Exception("Unable to write object as type " + typeof(T).Name);
        }

        public static bool AreEqual(Type ty, object a, object b)
        {
            var module = modulesInUse.FirstOrDefault(m => m.CanSerialize(ty));
            if (module == null)
                throw new Exception();
            return module.AreEqual(ty, a, b);
        }
        #endregion


        #region Helper methods
        /// <summary>
        /// Provides a disposable BinaryWriter in case you are feeling lazy today. 
        /// </summary>
        public static byte[] GetWriter(Action<BinaryWriter> action)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                    action(writer);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Provides a disposable BinaryReader in case you are feeling lazy today. 
        /// </summary>
        public static void GetReader(byte[] bytes, Action<BinaryReader> action)
        {
            using (var ms = new MemoryStream(bytes))
            {
                using (var reader = new BinaryReader(ms))
                    action(reader);
            }
        }

        /// <summary>
        /// Provides a disposable BinaryReader in case you are feeling lazy today. 
        /// </summary>
        public static T GetReader<T>(byte[] bytes, Func<BinaryReader, T> action)
        {
            using (var ms = new MemoryStream(bytes))
            {
                T obj;
                using (var reader = new BinaryReader(ms))
                    obj = action(reader);
                return obj;
            }
        }
        #endregion

        static SerializerModule getModule(Type ty)
        {
            SerializerModule readerModule;
            if (!mappedTypes.TryGetValue(ty, out readerModule))
                mappedTypes[ty] = (readerModule = modulesInUse.FirstOrDefault(m => m.CanSerialize(ty)));
            return readerModule;
        }
    }
}
