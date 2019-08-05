using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class JsonParser
    {
        private dynamic jdata;

        public JsonParser(JObject json)
        {
            jdata = json;
        }
        public T Parse<T>(string key)
        {
            return jdata.key;
        }
        public T ParseObj<T>(string key) {
            return jdata.key.ToObject<T>();
        }
    }
}