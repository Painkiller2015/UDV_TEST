using System.Text.Json;

namespace UDV_TEST.Services
{
    public static class Serialize_Service
    {
        public static string ToJSON(object item)
        {
            return JsonSerializer.Serialize(item);
        }
        public static T FromJSON<T>(string code)
        {
            return JsonSerializer.Deserialize<T>(code);
        }
    }
}
