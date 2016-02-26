//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IxSerializer
//{
//    /// <summary>
//    /// Uses the Serializer class to support serialization according to an interface. 
//    /// Should be refactored 
//    /// </summary>
//    public class InterfaceSerializer
//    {

//        /// <summary>
//        /// Deserializes all public properties from the given class or interface type <typeparamref name="TStream"/> to a new instance of the <typeparamref name="TOut"/> type. 
//        /// The output type must be inheriting from the type that is to be read. 
//        /// <para/>
//        /// Can be used to serialize all of an object's properties defined in a public interface. 
//        /// One can then use <see cref="ReadInterfaceData{TInt, TObj}(BinaryReader)"/>
//        /// to deserialize the stream into any object implementing the interface. 
//        /// </summary>
//        /// <typeparam name="TStream">The reference class or interface to serialize public properties from. </typeparam>
//        /// <typeparam name="TOut">The type of the object that is to be created. Must inherit from the <paramref name="TIn"/> type. </typeparam>
//        /// <param name="reader">The BinaryReader used to deserialize the object's properties. </param>
//        /// <param name="skipUnknownFields">Whether to continue with the deserialization process if an un-serializable field is encountered. Throws an exception by default. </param>
//        /// <returns>A new instance of the <paramref name="TOut"/> class. </returns>
//        public static TOut ReadInterfaceData<TStream, TOut>(BinaryReader reader, bool skipUnknownFields = false)
//           where TOut : TStream
//           where TStream : class
//        {
//            var obj = Activator.CreateInstance<TOut>();
//            return (TOut)ReadInterfaceData(reader, typeof(TStream), typeof(TOut), obj, skipUnknownFields);
//        }

//        public static void WriteInterfaceData<TStream>(BinaryWriter writer, TStream oldGuy, bool skipUnknownFields = false)
//            where TStream : class
//        {
//            WriteInterfaceData(writer, typeof(TStream), oldGuy, skipUnknownFields);
//        }


//        /// <summary>
//        /// Serializes all common properties between the provided object and the given class or interface type <typeparamref name="TIn"/>. 
//        /// <para/>
//        /// Can be used to serialize all of an object's properties defined in a public interface. 
//        /// One can then use <see cref="ReadInterfaceData{TInt, TObj}(BinaryReader)"/>
//        /// to deserialize the stream into any object implementing the interface. 
//        /// </summary>
//        /// <typeparam name="TIn"> </typeparam>
//        /// <param name="writer">The BinaryWriter used to serialize the object's properties. </param>
//        /// <param name="obj">The object which is being serialized. </param>
//        /// <param name="streamType">The reference class or interface to serialize public properties from. </param>
//        /// <param name="skipUnknownFields">Whether to continue with the serialization process if an un-serializable field is encountered. Throws an exception by default. </param>
//        public static void WriteInterfaceData(BinaryWriter writer, Type streamType, object obj, bool skipUnknownFields = false)
//        {
//            //get T's public properties and write em
//            foreach (var p in streamType.GetAllProperties())
//            {
//                var val = PropertyCaller.GetValue(obj, p.Name);

//                if (!Serializer.TryWrite(writer, p.PropertyType, val))
//                {
//                    var msg = string.Format("Unable to serialize field {0} of type {1} as part of interface {2}", p.Name, p.PropertyType.FullName, streamType.Name);
//                    if (skipUnknownFields)
//                        Console.WriteLine("Warning: " + msg);
//                    else
//                        throw new Exception(msg);
//                }
//            }
//        }

//        internal static object ReadInterfaceData(BinaryReader reader, Type streamType, Type tyOut, object obj, bool skipUnknownFields = false)
//        {
//            //get T's public properties ordered by name and read them one by one
//            foreach (var p in streamType.GetAllProperties())
//            {
//                object val = null;

//                if (!Serializer.TryRead(reader, p.PropertyType, out val))
//                {
//                    var msg = string.Format("Unable to deserialize field {0} of interface {1}", p.Name, p.PropertyType.FullName);
//                    if (skipUnknownFields)
//                        Console.WriteLine("Warning: " + msg);
//                    else
//                        throw new Exception(msg);
//                }

//                PropertyCaller.SetValue(obj, p.Name, val);
//            }

//            return obj;
//        }
//    }
//}
