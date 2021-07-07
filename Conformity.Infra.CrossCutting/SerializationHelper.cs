using Newtonsoft.Json;

namespace Conformity.Infra.CrossCutting
{
    public class SerializationHelper
    {
        public static string ToJson(object value)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(value, Formatting.Indented, settings);
        }
    }
}
