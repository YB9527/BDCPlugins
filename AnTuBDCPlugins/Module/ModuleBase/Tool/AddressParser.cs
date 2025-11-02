using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;
namespace ModuleBase.Tool
{


    public class AddressParser
    {
        public static (string County, string Town, string Village, string Group) ParseAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("地址不能为空或空白。");
            }

            // 正则表达式匹配：县、乡镇（街道）、村、组（社）
            var pattern = @"^(?<county>.+?县)(?<town>.+?[镇街道])(?<village>.+村)(?<group>.+[社组])$";
            var match = Regex.Match(address, pattern);

            if (!match.Success)
            {
                throw new FormatException("地址格式不正确，无法解析出完整的县、乡镇（街道）、村、组（社）。");
            }

            return (
                County: match.Groups["county"].Value,
                Town: match.Groups["town"].Value,
                Village: match.Groups["village"].Value,
                Group: match.Groups["group"].Value
            );
        }

        // 示例用法
        public static void Main()
        {
            try
            {
                var address = "巫溪县古路镇大泉村六社";
                var (county, town, village, group) = ParseAddress(address);
                Console.WriteLine($"县: {county}, 乡镇: {town}, 村: {village}, 组: {group}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"解析失败: {ex.Message}");
            }
        }
    }
}
