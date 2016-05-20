﻿using Shanism.Common.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Shanism.Common
{
    /// <summary>
    /// </summary>
    public static class GeneralExt
    {
        // Extension methods for the conversion between strings and byte arrays. 

        /// <summary>
        /// Encodes all the characters in the specified string as bytes. 
        /// </summary>
        /// <param name="str">The string containing the characters to encode. </param>
        /// <returns>A byte array containing the results of encoding the specified set of characters.</returns>
        public static byte[] GetBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string GetString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }





        public static IEnumerable<Type> GetTypesDescending<T>(this Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(ty => typeof(T).IsAssignableFrom(ty));
        }

        /// <summary>
        /// Gets all public properties, including those defined in implemented interfaces. 
        /// If a property is defined in more than one place, the top-most definition is returned. 
        /// </summary>
        /// <param name="ty"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type ty)
        {
            var propNames = new HashSet<string>();

            //use a fifo container for a bfs walk
            var types = new Queue<Type>();
            types.Enqueue(ty);

            do
            {
                var curTy = types.Dequeue();

                foreach (var prop in curTy.GetProperties())
                    if (propNames.Add(prop.Name))
                        yield return prop;

                foreach (var subTy in curTy.GetInterfaces())
                    types.Enqueue(subTy);
            }
            while (types.Any());
        }
    }
}