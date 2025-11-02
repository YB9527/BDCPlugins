using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBase.Tool
{
    /// <summary>
    /// 缓存工具
    /// </summary>
    public class CacheTool
    {
        private Dictionary<string, String> Cache = new Dictionary<string, String>();
        //private static string CacheFile = AppTool.GetAppDirectory() + "/Cache/Cache.txt";
        private static string CacheFile = "";
        public void Read()
        {
            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            CacheFile = userProfilePath + "/房地一体/.cache/Cache.txt";
            if (!Directory.Exists(Path.GetDirectoryName(CacheFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CacheFile));
            }
            if (!File.Exists(CacheFile))
            {
                if (!Directory.Exists(Path.GetDirectoryName(CacheFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(CacheFile));
                }
                File.CreateText(CacheFile).Close();
            }
            try
            {
                // 使用Newtonsoft.Json反序列化JSON字符串为字典
                using (StreamReader reader = new StreamReader(CacheFile))
                {

                    string fileContent = reader.ReadToEnd();


                    Dictionary<string, string> dictionaryFromJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContent);
                    Cache = dictionaryFromJson;
                    if (Cache == null)
                    {
                        Cache = new Dictionary<string, string>();
                    }
                }

            }
            catch (Exception ex)
            {
                Cache = new Dictionary<string, string>();
            }



        }

        public T GetValueTobject<T>(string key)
        {
            try
            {
                String value = GetValue(key);
                T obj = JObject.Parse(value).ToObject<T>();
                return obj;
            }
            catch
            {
                return default(T);
            }

        }

        public String GetValue(string key)
        {
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }
            return null;
        }
        public void SetValue(string key, String value, bool isSave = false)
        {
            if (Cache.ContainsKey(key))
            {
                Cache[key] = value;
            }
            else
            {
                Cache.Add(key, value);
            }
            if (isSave)
            {
                this.Save();
            }
        }
        public void Save()
        {

            string jsonString = JsonConvert.SerializeObject(Cache);
            FileTool.WriteTxt(CacheFile, new List<string>() { jsonString });
        }
    }

}
