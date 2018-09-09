using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Line.Messaging
{
    internal class CamelCaseJsonSerializerSettings : JsonSerializerSettings
    {
        public CamelCaseJsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            Converters.Add(new StringEnumConverter { CamelCaseText = true });
            NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
