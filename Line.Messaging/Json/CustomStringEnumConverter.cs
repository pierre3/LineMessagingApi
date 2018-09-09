using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Line.Messaging
{


    public class CustomStringEnumConverter<TEnum> : StringEnumConverter where TEnum : struct, Enum
    {
        private readonly IDictionary<TEnum, string> enumStrPairs;


        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ComponentSize);
        }

        public CustomStringEnumConverter(IDictionary<TEnum, string> enumStrPairs)
        {
            this.enumStrPairs = enumStrPairs ?? new Dictionary<TEnum, string>();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var value = (string)reader.Value;

                if (enumStrPairs.Any(kvp => value == kvp.Value))
                {
                    return enumStrPairs.First(kvp => value == kvp.Value).Key;
                }
            }

            return base.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            if (enumStrPairs.TryGetValue((TEnum)value, out string name))
            {
                writer.WriteValue(name);
            }
            else
            {
                base.WriteJson(writer, value, serializer);
            }
            
        }
    }
}