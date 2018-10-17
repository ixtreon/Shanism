using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Client.Common.Json
{
    public abstract class TypedJsonConverter : JsonConverter
    {
        protected TypedJsonConverter(Type fromType, Type toType)
        {
            FromType = fromType;
            ToType = toType;
        }

        public Type FromType { get; }

        public Type ToType { get; }
    }

    class JsonSpanConverter<TFrom, TTo> : TypedJsonConverter
    {
        readonly int length;
        readonly Func<TFrom, IEnumerable<TTo>> serialize;
        readonly Func<IList<TTo>, TFrom> deserialize;

        readonly TTo[] buffer;

        public JsonSpanConverter(int length, Func<TFrom, IEnumerable<TTo>> serialize, Func<IList<TTo>, TFrom> deserialize)
            : base(typeof(TFrom), typeof(IEnumerable<TTo>))
        {
            this.length = length;
            this.buffer = new TTo[length];

            this.serialize = serialize;
            this.deserialize = deserialize;
        }

        public override bool CanConvert(Type objectType) => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var typed = (TFrom)value;
            var converted = serialize(typed);
            var jToken = JToken.FromObject(converted);

            jToken.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jToken = JToken.Load(reader);
            try
            {
                var i = 0;
                foreach (var e in jToken.Values<TTo>())
                    buffer[i++] = e;

                var converted = deserialize(buffer);
                return converted;
            }
            catch
            {
                var ans = jToken.ToObject(objectType);

                return ans;
            }
        }
    }

    class GenericJsonConverter<TFrom, TTo> : TypedJsonConverter
    {

        readonly Func<TFrom, TTo> serialize;
        readonly Func<TTo, TFrom> deserialize;

        public GenericJsonConverter(Func<TFrom, TTo> serialize, Func<TTo, TFrom> deserialize)
            : base(typeof(TFrom), typeof(TTo))
        {
            this.serialize = serialize;
            this.deserialize = deserialize;
        }

        public override bool CanConvert(Type objectType) => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var typed = (TFrom)value;
            var converted = serialize(typed);
            var jToken = JToken.FromObject(converted);

            jToken.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jToken = JToken.Load(reader);
            try
            {
                var typed = jToken.ToObject<TTo>();
                var converted = deserialize(typed);

                return converted;
            }
            catch
            {
                var ans = jToken.ToObject(objectType);

                return ans;
            }
        }
    }

}
