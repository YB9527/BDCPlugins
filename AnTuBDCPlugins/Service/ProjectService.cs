using BDCPlugins.BDCException;
using BDCPlugins.BDCSystem;
using BDCPlugins.Config;
using BDCPlugins.Entity;
using BDCPlugins.tool;
using ModuleBase.Tool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDCPlugins.Service
{
    /// <summary>
    /// 项目服务
    /// </summary>
    public class ProjectService
    {
        private ConfigService ConfigService;
        public ProjectService()
        {

            ConfigService =  ConfigService.ReadConfig();

        }

        
        
        public void ReplaceParams_old(RequestEntity requestEntity, JObject jobject)
        {
            JObject reqObj = JObject.FromObject(requestEntity);

            bool ReplaceToken(JToken token)
            {
                if (token == null || token.Type == JTokenType.Null)
                {
                    // 处理null值的情况
                    return false;
                }

                if (token.Type == JTokenType.Object)
                {
                    foreach (var prop in ((JObject)token).Properties())
                    {
                        bool bl = ReplaceToken(prop.Value);
                        while (bl)
                        {
                            bl = ReplaceToken(prop.Value);
                        }
                    }
                }
                else if (token.Type == JTokenType.Array)
                {
                    foreach (var item in (JArray)token)
                    {
                        ReplaceToken(item);
                    }
                }
                else if (token.Type == JTokenType.String)
                {
                    string str = token.ToString();
                    JArray array = null;
                    string replaced = Regex.Replace(str, @"\$\{([^}]+)\}", match =>
                    {
                        string expr = match.Groups[1].Value;

                        // 1. 支持多级key查找
                        if (jobject != null)
                        {
                            var parts = expr.Split('.');
                            JToken current = jobject;
                            foreach (var part in parts)
                            {
                                if (current is JObject obj && obj.TryGetValue(part, out var nextToken))
                                {
                                    current = nextToken;
                                }
                                else
                                {
                                    current = null;
                                    break;
                                }
                            }

                            // 处理找到的值为null的情况
                            if (current != null && current.Type != JTokenType.Undefined)
                            {
                                // 如果值是null，返回空字符串而不是null
                                if (current.Type == JTokenType.Null)
                                    return string.Empty;
                                if (requestEntity.IsPostArray && current is JArray)
                                {
                                    array = current as JArray;
                                }
                                else
                                {
                                    array = null;
                                }
                                return current.ToString();

                            }
                        }

                        // 2. 查ProjectConfig静态属性
                        var staticField = typeof(ProjectConfig).GetField(expr, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                        if (staticField != null)
                        {
                            var val = staticField.GetValue(null);
                            return val?.ToString() ?? string.Empty; // 处理null值
                        }

                        var staticProp = typeof(ProjectConfig).GetProperty(expr, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                        if (staticProp != null)
                        {
                            var val = staticProp.GetValue(null);
                            return val?.ToString() ?? string.Empty; // 处理null值
                        }

                        return match.Value; // 未找到则原样返回
                    });

                    if (replaced != str)
                    {
                        if (array != null)
                        {
                            JProperty json = token.Parent as JProperty;
                            json.Value = array;
                        }
                        else
                        {
                            ((JValue)token).Value = replaced;
                        }

                        return true;
                    }
                }
                return false;
            }

            ReplaceToken(reqObj);

            // 替换回requestEntity
            var updated = reqObj.ToObject<RequestEntity>();
            requestEntity.Method = updated.Method;
            requestEntity.Url = updated.Url;
            requestEntity.Body = updated.Body;
            requestEntity.Query = updated.Query;
            requestEntity.Headers = updated.Headers;
            requestEntity.RequestDealy = updated.RequestDealy;
            requestEntity.Still = updated.Still;
        }


        internal String RequestString(string requestName, JObject dataSource)
        {
            RequestEntity requestEntity = ConfigService.FindRequestEntity(requestName);
            ReplaceParams(requestEntity, dataSource);
            var str = JObject.FromObject(requestEntity).ToString();
            var str2 = dataSource.ToString();
            JObject json = RequestTool.SendRequest(requestEntity);
            return HanleResponseString(requestName, json, dataSource);
        }
        public void ReplaceParams(RequestEntity requestEntity, JObject dataSource)
        {
            //ReplaceParams_OLD(requestEntity, dataSource);
            JObject reqObj = JObject.FromObject(requestEntity);

            bool ReplaceToken(JToken token)
            {
                if (token == null || token.Type == JTokenType.Null)
                {
                    return false;
                }

                if (token.Type == JTokenType.Object)
                {
                    foreach (var prop in ((JObject)token).Properties())
                    {
                        bool bl = ReplaceToken(prop.Value);
                        while (bl)
                        {
                            bl = ReplaceToken(prop.Value);
                        }
                    }
                }
                else if (token.Type == JTokenType.Array)
                {
                    foreach (var item in (JArray)token)
                    {
                        ReplaceToken(item);
                    }
                }
                else if (token.Type == JTokenType.String)
                {
                    string str = token.ToString();
                    JArray array = null;
                    String replaced = Regex.Replace(str, @"\$\{([^}]+)\}", match =>
                    {
                        string expr = match.Groups[1].Value;
                        if (expr.Contains("使用权结束时间-日期格式"))
                        {

                        }
                        // 处理 || 语法：${A || B}

                        if (expr.Contains("||"))
                        {
                            var parts = expr.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(p => p.Trim())
                                          .ToList();

                            foreach (var part in parts)
                            {
                                try
                                {
                                    Object value = GetValueFromExpression(part, dataSource, requestEntity);
                                    if (value != null)
                                    {
                                        if (requestEntity.IsPostArray)
                                        {
                                            array = value as JArray;
                                        }
                                        return value.ToString();
                                    }
                                }
                                catch
                                {

                                }

                            }

                            return string.Empty;
                        }
                        else
                        {
                            Object val = GetValueFromExpression(expr, dataSource, requestEntity);
                            if (val != null)
                            {
                                if (requestEntity.IsPostArray)
                                {
                                    array = val as JArray;
                                }
                                return val.ToString();
                            }
                        }

                        return null;

                    });

                    if (replaced != str)
                    {
                        if (array != null)
                        {
                            JProperty json = token.Parent as JProperty;
                            json.Value = array;
                        }
                        else
                        {
                            ((JValue)token).Value = replaced;
                        }
                        return true;
                    }
                }
                return false;
            }

            ReplaceToken(reqObj);

            var updated = reqObj.ToObject<RequestEntity>();
            requestEntity.Method = updated.Method;
            requestEntity.Url = updated.Url;
            requestEntity.Body = updated.Body;
            requestEntity.Query = updated.Query;
            requestEntity.Headers = updated.Headers;
            requestEntity.FilePath = updated.FilePath;
        }

        private Object GetValueFromExpression(string expr, object dataSource, RequestEntity requestEntity)
        {


            // 2. 查ProjectConfig静态属性
            var staticField = typeof(ProjectConfig).GetField(expr, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (staticField != null)
            {
                var val = staticField.GetValue(null);
                return val?.ToString() ?? string.Empty;
            }

            var staticProp = typeof(ProjectConfig).GetProperty(expr, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (staticProp != null)
            {
                var val = staticProp.GetValue(null);
                return val?.ToString() ?? string.Empty;
            }

            if (expr.Contains("ABCD"))
            {

            }
            // 1. 首先尝试从数据源中查找
            if (dataSource != null)
            {
                object current = dataSource;
                var parts = expr.Split('.');

                foreach (var part in parts)
                {
                    try
                    {
                        current = GetValueFromObject(current, part);
                        if (current == null)
                        {
                            break;
                        }
                    }
                    catch (ExceptionQuanJi ex)
                    {
                        throw new ExceptionQuanJi(ex.Describle + "," + expr);
                    }

                }

                if (current != null)
                {
                    if (requestEntity.IsPostArray)
                    {
                        if (current is JArray)
                        {
                            return current;
                        }
                    }

                    return current.ToString();
                }
            }

            return null;
        }

        private object GetValueFromObject(object obj, string key)
        {
            key = key.Trim();
            if (obj == null) return null;

            // 支持 JObject
            if (obj is JObject jObject)
            {
                if (jObject.TryGetValue(key, out var token))
                {
                    return token.Type == JTokenType.Null ? "" : token;
                }
                throw new ExceptionQuanJi("转换失败，未转换的代码：" + key);
            }

            // 支持字典
            if (obj is IDictionary dictionary)
            {
                if (dictionary.Contains(key))
                {
                    return dictionary[key];
                }
                return null;
            }

            // 支持 ExpandoObject
            if (obj is System.Dynamic.ExpandoObject expando)
            {
                var expandoDict = (IDictionary<string, object>)expando;
                if (expandoDict.TryGetValue(key, out var value))
                {
                    return value;
                }
                return null;
            }

            // 支持普通对象的属性
            var prop = obj.GetType().GetProperty(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null)
            {
                return prop.GetValue(obj);
            }

            // 支持普通对象的字段
            var field = obj.GetType().GetField(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (field != null)
            {
                return field.GetValue(obj);
            }

            return null;
        }

        /// <summary>
        /// 写入错误信息
        /// </summary>
        /// <param name="v"></param>
        /// <param name="notFinds"></param>
        internal void WriteErrors(string fileName, List<string> errors)
        {


            string path = AppTool.GetTime(fileName + ".txt");

            FileTool.WriteTxt(path, errors, true);

        }

        /// <summary>
        /// 替换requestEntity参数
        /// </summary>
        /// <param name="requestEntity"></param>
        /// <param name="jobject"></param>
        private void ReplaceParams_OLD(RequestEntity requestEntity, JObject jobject)
        {
            // 递归替换requestEntity对象中的${}，优先用jobject本身的key（支持多级路径）替换，没有则用ProjectConfig静态属性
            JObject reqObj = JObject.FromObject(requestEntity);
            bool ReplaceToken(JToken token)
            {
                if (token.Type == JTokenType.Object)
                {
                    foreach (var prop in ((JObject)token).Properties())
                    {
                        bool bl = ReplaceToken(prop.Value);
                        while (bl)
                        {
                            bl = ReplaceToken(prop.Value);
                        }
                    }
                }
                else if (token.Type == JTokenType.Array)
                {
                    foreach (var item in (JArray)token)
                    {
                        ReplaceToken(item);
                    }
                }
                else if (token.Type == JTokenType.String)
                {
                    string str = token.ToString();
                    string replaced = Regex.Replace(str, @"\$\{([^}]+)\}", match =>
                    {
                        string expr = match.Groups[1].Value;
                        // 1. 支持多级key查找
                        if (jobject != null)
                        {
                            var parts = expr.Split('.');
                            JToken current = jobject;
                            foreach (var part in parts)
                            {
                                JToken jToken = null;
                                if (current is JObject obj && obj.TryGetValue(part, out jToken))
                                {
                                    current = obj[part];
                                }
                                else
                                {
                                    current = null;
                                    break;
                                }
                            }
                            if (current != null && current.Type == JTokenType.String)
                                return current.ToString();
                            if (current != null && current.Type != JTokenType.Null && current.Type != JTokenType.Undefined)
                                return current.ToString();
                        }
                        // 2. 查ProjectConfig静态属性
                        var staticField = typeof(ProjectConfig).GetField(expr, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                        if (staticField != null)
                        {
                            var val = staticField.GetValue(null);
                            if (val != null) return val.ToString();
                        }
                        var staticProp = typeof(ProjectConfig).GetProperty(expr, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                        if (staticProp != null)
                        {
                            var val = staticProp.GetValue(null);
                            if (val != null) return val.ToString();
                        }
                        return match.Value; // 未找到则原样返回
                    });
                    if (replaced != str)
                    {
                        ((JValue)token).Value = replaced;
                        return true;
                    }
                }
                return false;
            }
            ReplaceToken(reqObj);
            // 替换回requestEntity
            var updated = reqObj.ToObject<RequestEntity>();
            requestEntity.Method = updated.Method;
            requestEntity.Url = updated.Url;
            requestEntity.Body = updated.Body;
            requestEntity.Query = updated.Query;
            requestEntity.Headers = updated.Headers;
        }







        public String HanleResponseString(String requestName, JObject json, JObject data = null)
        {
            if (json["code"] != null && json["code"].ToString() != "0")
            {
                throw new ExceptionQuanJi(requestName, json, data);
            }
            String res = JsonTool.GetStringValue(json, "data");
            return res;
        }



        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="jobject"></param>
        /// <returns></returns>
        public JObject Request(String requestName, JObject jobject, bool isLogin = false)
        {

            RequestEntity requestEntity = ConfigService.FindRequestEntity(requestName);
            ReplaceParams(requestEntity, jobject);
            var str = JObject.FromObject(requestEntity).ToString();
            var str2 = jobject.ToString();
            JObject json = RequestTool.SendRequest(requestEntity, null, isLogin);
            return HanleResponse(requestName, json,jobject);
        }

        public JObject CheckToken(String requestName, JObject jobject)
        {

            RequestEntity requestEntity = ConfigService.FindRequestEntity(requestName);
            ReplaceParams(requestEntity, jobject);
            var str = JObject.FromObject(requestEntity).ToString();
            JObject json = RequestTool.SendRequest(requestEntity, null);
            return HanleResponse(requestName, json,jobject);
        }






        internal JArray RequestArray(string requestName, JObject json)
        {
            RequestEntity requestEntity = ConfigService.FindRequestEntity(requestName);
            ReplaceParams(requestEntity, json);
            String str = JObject.FromObject(requestEntity).ToString();
            JObject res = RequestTool.SendRequest(requestEntity);
            return HanleResponseArray(requestName, res, json);
        }

        public JArray HanleResponseArray(string requestName, JObject json, JObject zongdientity)
        {
            if (json["code"] != null && json["code"].ToString() != "0")
            {
                throw new ExceptionQuanJi(requestName, json, zongdientity);
            }
            JArray data = json["data"] as JArray;
            return data;
        }

        public JArray HanleResponseDataRows(string requestName, JObject zongdi)
        {

            List<String> datas = new List<string>();
            try
            {
                RequestEntity requestEntity = ConfigService.FindRequestEntity(requestName);
                ReplaceParams(requestEntity, zongdi);
                String str = JObject.FromObject(requestEntity).ToString();
               
                JObject json = RequestTool.SendRequest(requestEntity);
                return HanleResponseDataRows(requestName, json, zongdi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                String path = AppTool.GetTime("临时测试1.1.txt");
                FileTool.WriteTxt(path, datas, false);
            }

        }
        private JArray HanleResponseDataRows(string requestName, JObject json, JObject zongdientity)
        {
            if (json["code"] != null && json["code"].ToString() != "0")
            {
                throw new ExceptionQuanJi(requestName, json, zongdientity);
            }
            JObject data = json["data"] as JObject;

            JArray array = data.Value<JArray>("rows");
            return array;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="errorMesage">"根据项目名称查找项目，项目存在 {count} 个无法执行操作"</param>
        /// <returns></returns>
        public JObject GetFirstRow(JObject json, String errorMesage)
        {
            String value = JsonTool.GetStringValue(json, "count");
            if (value != "1")
            {
                errorMesage = errorMesage.Replace("{count}", value).Replace("{ count}", value);
                throw new ExceptionQuanJi(errorMesage, json);
            }
            JArray array = json.Value<JArray>("rows");
            JObject firstObj = (JObject)array[0];
            return firstObj;
        }

        public JObject GetFirstRow(JArray json, string errorMesage)
        {
            if (json == null)
            {
                throw new ExceptionQuanJi(errorMesage + ",没有查找到数据");
            }

            if (json.Count != 1)
            {
                errorMesage = errorMesage.Replace("{count}", json.Count.ToString());
                errorMesage = errorMesage.Replace("{ count}", json.Count.ToString());
                throw new ExceptionQuanJi(errorMesage);
            }

            JObject firstObj = (JObject)json[0];
            return firstObj;
        }














        /// <summary>
        /// 查找字典中值（路径）包含指定关键字的文件夹路径
        /// </summary>
        /// <param name="directoryDict">目录字典（Key=目录名, Value=路径）</param>
        /// <param name="searchTerm">要搜索的关键字（不区分大小写）</param>
        /// <returns>所有匹配的文件夹路径</returns>
        public static IEnumerable<string> FindDirectoriesContaining(
            Dictionary<string, string> directoryDict,
            string searchTerm)
        {
            if (directoryDict == null)
                throw new ArgumentNullException(nameof(directoryDict));

            if (string.IsNullOrWhiteSpace(searchTerm))
                return directoryDict.Values; // 如果搜索词为空，返回所有路径

            // 不区分大小写查找包含关键字的路径
            return directoryDict.Values
                .Where(path => path.Contains(searchTerm));
        }

        /// <summary>
        /// 递归获取所有子目录，返回字典（Key=目录名, Value=完整路径）
        /// </summary>
        /// <param name="rootPath">根目录路径</param>
        /// <returns>字典（目录名 → 完整路径）</returns>
        public static Dictionary<string, string> GetAllDirectoriesAsDictionary(string rootPath)
        {
            var directoryDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase); // 不区分大小写

            if (!Directory.Exists(rootPath))
            {
                Console.WriteLine($"目录不存在: {rootPath}");
                return directoryDict;
            }

            try
            {
                // 遍历当前目录的所有子目录
                foreach (string dirPath in Directory.EnumerateDirectories(rootPath, "*", SearchOption.AllDirectories))
                {
                    string dirName = Path.GetFileName(dirPath); // 获取目录名（不含路径）

                    // 确保目录名唯一（如果有重复，可以加后缀或抛异常）
                    if (!directoryDict.ContainsKey(dirName))
                    {
                        if (dirName.Contains("J"))
                        {
                            directoryDict.Add(dirName, dirPath);
                        }

                    }
                    else
                    {
                        // 如果目录名重复，可以修改 Key 或跳过
                        Console.WriteLine($"警告：目录名 '{dirName}' 已存在，路径 '{dirPath}' 将被忽略。");
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"权限不足，无法访问某些目录: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }

            return directoryDict;
        }


        /// <summary>
        /// 根据身份证号码计算性别（"女性":"2", "男性":"1"）
        /// </summary>
        /// <param name="zjhm">18位身份证号码</param>
        /// <returns>"1"表示男性，"2"表示女性</returns>
        private string GetSexByZjhm(string zjhm)
        {
            if (string.IsNullOrEmpty(zjhm) || zjhm.Length != 18)
            {
                // 可根据需求返回默认值或抛出异常
                return "3";
            }

            // 获取第17位字符（索引为16，因为从0开始）
            char genderChar = zjhm[16];

            // 判断奇偶性
            if (int.TryParse(genderChar.ToString(), out int genderDigit))
            {
                return (genderDigit % 2 == 1) ? "1" : "2"; // 奇数=男性，偶数=女性
            }
            else
            {
                return "3";
            }
        }

        /// <summary>
        /// 获取响应结果
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public JObject HanleResponse(String requestName, JObject json,JObject data)
        {
            if (json["code"] != null && json["code"].ToString() != "0")
            {
                throw new ExceptionQuanJi(requestName, json, null);
            }
            if (json["msg"] != null && json["msg"].ToString() == "转出数据错误")
            {
                throw new ExceptionQuanJi(requestName, json, data);
            }
            JObject res = json["data"] as JObject;
            return res;
        }

  

 
      

     

    }
}
