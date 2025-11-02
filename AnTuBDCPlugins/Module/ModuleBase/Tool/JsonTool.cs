using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModuleBase.Tool
{
    public static class JsonTool
    {
        /// <summary>
        /// 读取json文件并反序列化为对象
        /// </summary>
        public static T ReadJsonFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"未找到文件: {filePath}");
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string GetStringValue(JObject json, string key)
        {
            if(json == null)
            {
                return null;
            }
            Object obj = json.Value<Object>(key);
            if(obj == null)
            {
                return null;
            }
            return obj.ToString();
        }



        public static void SetValue(JObject json, string key, Object value)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json), "JObject 不能为 null");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key 不能为空或 null", nameof(key));
            }

            // 检查 key 是否存在
            JToken obj;
            if (json.TryGetValue(key, out obj))
            {
                // 存在则修改值
                json[key] = JToken.FromObject(value);
            }
            else
            {
                // 不存在则添加新属性
                json.Add(key, JToken.FromObject(value));
            }
        }

        public static List<JObject> GetList<T>(List<T> entities)
        {
            List<JObject> list = new List<JObject>();
            if(entities == null)
            {
                return list;
            }
            foreach (var item in entities)
            {
                list.Add(JObject.FromObject(item));
            }
            return list;


        }

        public static T Copy<T>(T buLuService)
        {
           T t = JObject.FromObject(buLuService).ToObject<T>();
            return t;
        }

        public static String ToString(object obj)
        {
            if(obj == null)
            {
                return null;
            }
            String str = JObject.FromObject(obj).ToString();
            return str;

        }
    }
} 