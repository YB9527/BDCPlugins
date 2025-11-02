using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModuleBase.Tool
{
    public static class StringUtils
    {
        public static bool IsNullOrEmpyt(String str)
        {
            if(str == null)
            {
                return true;
            }
            if(str == "")
            {
                return true;
            }
            return false;
        }

        public static bool IsNullOrTrimEmpty(String str)
        {
            if (IsNullOrEmpyt(str))
            {
                return true;
            }
            
            if (str.Trim() == "")
            {
                return true;
            }
            return false;
        }

        public static bool TrimEq(string a, string b)
        {
            if(a == b)
            {
                return true;
            }
            if(a != null && b != null)
            {
                return a.Trim() == b.Trim();
            }
            if((a == null || a.Trim() == "") && (b == null || b.Trim() == ""))
            {
                return true;
            }
            return false;
        }

        public static bool IsNullOrTrimEmptyOr0(string str)
        {
            bool res = IsNullOrTrimEmpty(str);
            if (res)
            {
                return true;
            }else if(str == "0")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从字符串中提取第一个匹配的整数
        /// </summary>
        /// <param name="input">输入字符串（如"鲁柏成等50户"）</param>
        /// <param name="pattern">可选的正则表达式模式（默认匹配连续数字）</param>
        /// <returns>提取的整数</returns>
        /// <exception cref="ArgumentException">输入为空或未找到数字时抛出</exception>
        public static int ExtractFirstNumber(this string input, string pattern = @"\d+")
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("输入字符串不能为空或空白。");
            }

            Match match = Regex.Match(input, pattern);
            if (!match.Success)
            {
                throw new ArgumentException("未找到匹配的数字。");
            }

            return int.Parse(match.Value);
        }

        /// <summary>
        /// 展开字符串
        /// </summary>
        /// <param name="errros"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        internal static string ExpandStrs(List<string> errros, string symbol)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < errros.Count; i++)
            {
                stringBuilder.Append(errros[i]);
                if(i+1 != errros.Count)
                {
                    stringBuilder.Append(symbol);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
