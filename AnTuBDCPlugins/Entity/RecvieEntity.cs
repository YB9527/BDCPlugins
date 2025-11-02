using ModuleOffice.Tool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class RecvieEntity
    {
        public LiuChengShuVo LiuChengShuVo { get; set; }
        public String ReceiveUser { get; set; }

        public String StartNodeName { get; set; }
        public RecvieEntity()
        {
            LiuChengShuVo = new LiuChengShuVo();
        }

        public static IEnumerable<string> ReplaceDict(List<Dictionary<string, string>> datas, string xlsPath, string sheetName, int headerIndex)
        {
            var dictlist = ExcelTool.ReadExcelCellValues(xlsPath, sheetName, headerIndex);
            var errors = ReplaceDict(datas, dictlist, sheetName);
            return errors;
        }
        public static List<String> ReplaceDict(List<Dictionary<string, string>> datas, Dictionary<string, List<string>> dictlist, String sheetName = null)
        {
            sheetName = sheetName == null ? "" : $"Sheet名称：{sheetName},";
            //data 的值是“1.住宅”
            //data 的值是“1”
            //data 的值是“住宅”
            List<String> errors = new List<string>();
            //先移除不规范的字典
            foreach (var dictKey in dictlist.Keys)
            {
                for (int i = 0; i < dictlist[dictKey].Count; i++)
                {
                    if (dictlist[dictKey][i] == null || dictlist[dictKey][i].Split('.').Length != 2)
                    {
                        dictlist[dictKey].RemoveAt(i);
                        i--;
                    }
                }
            }
            foreach (var dictKey in dictlist.Keys)
            {
                foreach (var data in datas)
                {
                    String val;
                    if (data.TryGetValue(dictKey, out val))
                    {

                        if (val == null || val == "")
                        {
                            continue;
                        }
                        var dicts = dictlist[dictKey];
                        string[] strs = val.Split('.');
                        string stdValue = null;
                        if (strs.Length == 1)
                        {
                            //检查是否是数字
                            double num;
                            bool has = false;
                            if (true)
                            {
                                //data 的值是“1”
                                //检查数字是否规范
                                foreach (var item in dicts)
                                {
                                    if (item.Split('.')[0] == strs[0])
                                    {
                                        stdValue = item.Split('.')[0];
                                        has = true;
                                        break;
                                    }
                                }

                            }
                            if (!has)
                            {
                                //data 的值是“住宅”
                                foreach (var item in dicts)
                                {
                                    if (item.Split('.')[1] == strs[0])
                                    {
                                        stdValue = item.Split('.')[0];
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //data 的值是“1.住宅”
                            foreach (var item in dicts)
                            {
                                if (item.Split('.')[1] == strs[1] && item.Split('.')[0] == strs[0])
                                {
                                    stdValue = item.Split('.')[0];
                                    break;
                                }
                            }
                        }

                        if (stdValue == null)
                        {

                            errors.Add($"{sheetName}{dictKey},字典值不规范，值：{val}");
                        }
                        else
                        {
                            data[dictKey] = stdValue;
                        }

                    }
                }
            }
            return errors;
        }
    }
}
