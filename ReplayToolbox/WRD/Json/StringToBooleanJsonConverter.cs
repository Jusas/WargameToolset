using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReplayToolbox.WRD.Json
{
    public class StringToBooleanJsonConverter : JsonConverter
    {
        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value;

            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return false;
            }

            if (value is bool)
                return value;

            if (string.Equals("1", value.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            return false;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(String) || objectType == typeof(Boolean))
            {
                return true;
            }
            return false;
        }
    }
}
