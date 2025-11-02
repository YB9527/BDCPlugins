using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModuleBase.Tool
{
    /// <summary>
    /// 值处理
    /// </summary>
    public class ValueTool
    {
        private static readonly Regex _variableRegex = new Regex(@"\$\{(.+?)\}", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// 解析字段名（支持纯文本、纯变量、文本+变量混合格式）
        /// 规则：
        /// 1. 纯变量路径不存在 → 抛出异常
        /// 2. 变量值为null → 返回null（混合场景中替换为null字符串）
        /// 3. 混合场景中变量路径不存在 → 抛出异常
        /// </summary>
        public static object GetValue(object data, string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return fieldName;
            }
                

            if (data == null)
                return fieldName; // 数据源为空时返回原始字符串（非变量场景）

            var matches = _variableRegex.Matches(fieldName);
            if (matches.Count == 0)
                return fieldName; // 纯文本直接返回

            // 处理变量替换
            string replacedResult = fieldName;
            object pureVariableValue = null;
            bool isPureVariable = IsPureVariable(fieldName);

            foreach (Match match in matches)
            {
                string path = match.Groups[1].Value;
                object value = GetValueFromPath(data, path); // 路径不存在会抛出异常

                if (isPureVariable)
                {
                    pureVariableValue = value; // 纯变量场景记录原始值
                    break;
                }

                // 混合场景：null值替换为"null"字符串，非null值用ToString()
                string replaceStr = value?.ToString() ?? "null";
                replacedResult = replacedResult.Replace(match.Value, replaceStr);
            }

            // 纯变量场景：直接返回原始值（可能为null）
            if (isPureVariable)
                return pureVariableValue;

            // 混合场景：返回替换后的字符串
            return replacedResult;
        }

        /// <summary>
        /// 从数据源获取路径对应的值，路径不存在则抛出异常
        /// </summary>
        private static object GetValueFromPath(object source, string path)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "数据源不能为空");
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("路径不能为空", nameof(path));

            object currentObj = source;
            string[] pathParts = path.Split('.');
            string currentPath = string.Empty; // 记录当前解析的路径（用于异常信息）

            foreach (string part in pathParts)
            {
                currentPath = string.IsNullOrEmpty(currentPath) ? part : $"{currentPath}.{part}";

                if (currentObj == null)
                    throw new KeyNotFoundException($"路径 '{currentPath}' 对应的对象为null，无法继续解析");

                // 处理JObject
                if (currentObj is JObject jObj)
                {
                    if (!jObj.TryGetValue(part, out JToken token))
                        throw new KeyNotFoundException($"JObject中不存在字段: {path}");
                    currentObj = token is JValue jVal ? jVal.Value : token;
                }
                // 处理字典
                else if (currentObj is IDictionary<string, object> objDict)
                {
                    if (!objDict.TryGetValue(part, out object value))
                        throw new KeyNotFoundException($"字典中不存在键: {currentPath}");
                    currentObj = value;
                }
                else if (currentObj is IDictionary<string, string> strDict)
                {
                    if (!strDict.TryGetValue(part, out string value))
                        throw new KeyNotFoundException($"字典中不存在键: {currentPath}");
                    currentObj = value;
                }
                // 处理列表（索引访问）
                else if (currentObj is IList list)
                {
                    if (!int.TryParse(part, out int index))
                        throw new FormatException($"路径 '{currentPath}' 中的索引 '{part}' 不是整数");
                    if (index < 0 || index >= list.Count)
                        throw new IndexOutOfRangeException($"列表索引 '{index}' 越界，路径: {currentPath}");
                    currentObj = list[index];
                }
                // 处理实体类（反射）
                else
                {
                    Type type = currentObj.GetType();
                    PropertyInfo prop = type.GetProperty(part, BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null)
                    {
                        currentObj = prop.GetValue(currentObj);
                    }
                    else
                    {
                        FieldInfo field = type.GetField(part, BindingFlags.Public | BindingFlags.Instance);
                        if (field == null)
                            throw new MissingMemberException(type.Name,  $"{part},实体类中不存在属性或字段: {currentPath}");
                        currentObj = field.GetValue(currentObj);
                    }
                }
            }

            return currentObj; // 允许返回null（表示字段存在但值为null）
        }

        private static bool IsPureVariable(string fieldName)
        {
            return _variableRegex.IsMatch(fieldName) && fieldName.Trim() == _variableRegex.Match(fieldName).Value;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="setKey"></param>
        /// <param name="value"></param>
        public static void SetValue(object data, string fieldName, Object val)
        {
            if (StringUtils.IsNullOrTrimEmpty(fieldName))
            {
                return;
            }
            if (fieldName.Contains("DLRZJH"))
            {

            }

            // 提取 ${} 中的路径
            var match = System.Text.RegularExpressions.Regex.Match(fieldName, @"\$\{(.+?)\}");
            if (!match.Success)
                throw new ArgumentException("fieldName 格式不正确，应为 ${Path.To.Property}");

            string path = match.Groups[1].Value;
            string[] parts = path.Split('.');

            object currentObj = data;

            // 遍历路径，直到最后一个部分
            for (int i = 0; i < parts.Length - 1; i++)
            {
                string part = parts[i];

                // 检查当前对象是否为 JObject
                if (currentObj is JObject jObject)
                {
                    if (!jObject.TryGetValue(part, out JToken token))
                    {
                        // 如果属性不存在，创建一个新的 JObject 作为中间节点
                        token = new JObject();
                        jObject[part] = token;
                    }
                    currentObj = token;
                }
                else
                {
                    // 使用反射获取属性
                    var property = currentObj.GetType().GetProperty(part);
                    if (property == null)
                        throw new ArgumentException($"在类型 {currentObj.GetType().Name} 中找不到属性 {part}");

                    // 获取属性值，如果为null则创建新实例
                    object propValue = property.GetValue(currentObj);
                    if (propValue == null)
                    {
                        // 特殊处理 JObject 类型
                        if (property.PropertyType == typeof(JObject))
                        {
                            propValue = new JObject();
                            property.SetValue(currentObj, propValue);
                        }
                        else
                        {
                            // 尝试创建实例（需要有默认构造函数）
                            propValue = Activator.CreateInstance(property.PropertyType);
                            property.SetValue(currentObj, propValue);
                        }
                    }

                    currentObj = propValue;
                }
            }

            // 处理最后一个属性
            string lastPart = parts[parts.Length - 1];

            // 检查当前对象是否为 JObject
            if (currentObj is JObject parentJObject)
            {
                // 处理 JObject 类型的属性
                JToken jTokenValue;

                if (val is JToken existingToken)
                {
                    jTokenValue = existingToken;
                }
                else if (val is JObject existingJObject)
                {
                    jTokenValue = existingJObject;
                }
                else if (val is string stringValue)
                {
                    // 尝试解析 JSON 字符串
                    try
                    {
                        if (stringValue.TrimStart().StartsWith("{"))
                        {
                            jTokenValue = JToken.Parse(stringValue);
                        }
                        else
                        {
                            jTokenValue = new JValue(stringValue);
                        }
                    }
                    catch
                    {
                        jTokenValue = new JValue(stringValue);
                    }
                }
                else if (val == null)
                {
                    jTokenValue = null;
                }
                else
                {
                    // 将其他类型转换为 JToken
                    jTokenValue = JToken.FromObject(val);
                }

                parentJObject[lastPart] = jTokenValue;
            }
            else
            {
                // 使用反射获取属性
                var property = currentObj.GetType().GetProperty(lastPart);
                if (property == null)
                    throw new ArgumentException($"在类型 {currentObj.GetType().Name} 中找不到属性 {lastPart}");

                // 特殊处理 JObject 类型的属性
                if (property.PropertyType == typeof(JObject))
                {
                    JObject jObjectValue;

                    if (val is JObject existingJObject)
                    {
                        jObjectValue = existingJObject;
                    }
                    else if (val is string jsonString)
                    {
                        try
                        {
                            jObjectValue = JObject.Parse(jsonString);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException($"无法将字符串解析为 JObject: {ex.Message}");
                        }
                    }
                    else
                    {
                        // 尝试将其他类型转换为 JObject
                        jObjectValue = JObject.FromObject(val);
                    }

                    property.SetValue(currentObj, jObjectValue);
                }
                else if (property.PropertyType == typeof(Dictionary<string, string>))
                {
                    // 处理字典类型
                    if (val is Dictionary<string, string> dict)
                    {
                        property.SetValue(currentObj, dict);
                    }
                    else if (val is string str)
                    {
                        // 尝试从字符串解析字典（例如 JSON 格式）
                        try
                        {
                            var parsedDict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(str);
                            property.SetValue(currentObj, parsedDict);
                        }
                        catch
                        {
                            throw new ArgumentException($"无法将字符串解析为 Dictionary<string, string>");
                        }
                    }
                    else
                    {
                        // 尝试转换
                        try
                        {
                            var converted = Convert.ChangeType(val, property.PropertyType);
                            property.SetValue(currentObj, converted);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException($"无法将值转换为目标类型 {property.PropertyType.Name}: {ex.Message}");
                        }
                    }
                }
                else
                {
                    // 处理其他类型
                    try
                    {
                        object convertedVal = Convert.ChangeType(val, property.PropertyType);
                        property.SetValue(currentObj, convertedVal);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException($"无法将值转换为目标类型 {property.PropertyType.Name}: {ex.Message}");
                    }
                }
            }
        }
    }
}
