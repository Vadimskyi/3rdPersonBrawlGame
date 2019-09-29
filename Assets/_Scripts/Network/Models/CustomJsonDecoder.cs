using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SocketIO.JsonEncoders;
using Newtonsoft.Json;
using UnityEngine;

namespace Vadimskyi.Utils
{
    public class CustomJsonDecoder : IJsonEncoder
    {
        public List<object> Decode(string json)
        {
            var index = json.IndexOf(",", StringComparison.CurrentCulture) + 1;
            string method = string.Empty, data = string.Empty;
            if (index > 0)
            {
                method = json.Substring(1, index - 2).Replace("\\", "").Replace("\"", "");
                data = json.Substring(index, json.Length - (index + 1));
            }
            else
            {
                method = json.Replace("\\", "").Replace("\"", "").Replace("[", "").Replace("]", "");
            }
            
            return new List<object> { method, data };
        }

        public string Encode(List<object> obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings{ ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        }
    }
}
