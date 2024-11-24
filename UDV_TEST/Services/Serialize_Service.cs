using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UDV_TEST.Services
{
    public static class Serialize_Service
    {
        static public string ToJSON(object item)
        {
            return JsonSerializer.Serialize(item);            
        }
        static public T FromJSON<T>(string code)
        {
            return JsonSerializer.Deserialize<T>(code);            
        }
    }
}
