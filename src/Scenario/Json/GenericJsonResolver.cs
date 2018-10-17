using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Client.Common.Json
{
    public class GenericJsonResolver : DefaultContractResolver
    {
        readonly Dictionary<Type, TypedJsonConverter> converters;

        public GenericJsonResolver(params TypedJsonConverter[] converters)
        {
            this.converters = converters.ToDictionary(c => c.FromType, c => c);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (converters.TryGetValue(property.PropertyType, out var converter))
                property.Converter = converter;

            return property;
        }

    }
}
