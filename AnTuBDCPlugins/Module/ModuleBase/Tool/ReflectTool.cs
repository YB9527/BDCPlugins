using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBase.Tool
{
    public class ReflectTool
    {
        /// <summary>
        /// 将sourceData中字段值设置到destData中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="sourceData">源对象</param>
        /// <param name="destData">目标对象</param>
        /// <param name="copyNullValues">是否复制null值</param>
        /// <param name="propertiesToIgnore">要忽略的属性名称列表</param>
        public static void CopyAttr<T>(T sourceData, T destData,
            bool copyNullValues = false,
            params string[] propertiesToIgnore)
        {
            if (sourceData == null || destData == null)
                return;

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var ignoreSet = new HashSet<string>(propertiesToIgnore ?? Array.Empty<string>());

            foreach (var property in properties)
            {
                if (ignoreSet.Contains(property.Name))
                    continue;

                if (property.CanRead && property.CanWrite)
                {
                    var value = property.GetValue(sourceData);

                    // 如果不复制null值且源值为null，则跳过
                    if (!copyNullValues && value == null)
                        continue;

                    property.SetValue(destData, value);
                }
            }
        }

        public static Dictionary<KEYT, T> GetDict<KEYT,T>(List<T> datas, string key)
        {
            String keyVar = "${" + key + "}";
            Dictionary<KEYT, T> results = new Dictionary<KEYT, T>();
            foreach (var item in datas)
            {
                var val = ValueTool.GetValue(item, keyVar);
                if(val is KEYT )
                {
                    results.Add((KEYT)val, item);
                }
                
            }
            return results;
        }

        public static Dictionary<KEYT, List<T>> GetDicts<KEYT,T>(List<T> datas, string key)
        {
            String keyVar = "${" + key + "}";
            Dictionary<KEYT, List<T>> results = new Dictionary<KEYT, List<T>>();
            foreach (var item in datas)
            {
                var val = ValueTool.GetValue(item, keyVar);
                if (val is KEYT)
                {
                    List<T> temps = null;
                    if (!results.TryGetValue((KEYT)val,out temps))
                    {
                        temps = new List<T>();
                        results.Add((KEYT)val, temps);
                    }
                    temps.Add(item);
                }

            }
            return results;
        }
    }
}
