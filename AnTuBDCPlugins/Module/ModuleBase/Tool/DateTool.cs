using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBase.Tool
{
    /// <summary>
    /// 日期工具类
    /// </summary>
    public static class DateTool
    {
        /// <summary>
        /// 获取当前时间格式化日期为字符串
        /// </summary>
        /// <param name="format">格式字符串，默认为"yyyy-MM-dd HH:mm:ss"</param>
        public static string GetCurrentFormatDateTime(string format = "yyyy-MM-dd HH:mm:ss")
        {
            return DateTime.Now.ToString(format);
        }

        /// <summary>
        /// 获取当前时间的时间戳（秒）
        /// </summary>
        public static long GetCurrentTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        /// <summary>
        /// 获取当前时间的时间戳（毫秒）
        /// </summary>
        public static long GetCurrentTimestampMilliseconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 时间戳转换为DateTime
        /// </summary>
        /// <param name="timestamp">秒级时间戳</param>
        public static DateTime TimestampToDateTime(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
        }

        /// <summary>
        /// 时间戳(毫秒)转换为DateTime
        /// </summary>
        /// <param name="timestamp">毫秒级时间戳</param>
        public static DateTime TimestampMillisecondsToDateTime(long timestamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).LocalDateTime;
        }

        /// <summary>
        /// DateTime转换为时间戳（秒）
        /// </summary>
        public static long DateTimeToTimestamp(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }

        /// <summary>
        /// DateTime转换为时间戳（毫秒）
        /// </summary>
        public static long DateTimeToTimestampMilliseconds(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 获取某天的开始时间（00:00:00）
        /// </summary>
        public static DateTime GetDayStart(DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// 获取某天的结束时间（23:59:59）
        /// </summary>
        public static DateTime GetDayEnd(DateTime date)
        {
            return date.Date.AddDays(1).AddSeconds(-1);
        }

        /// <summary>
        /// 获取两个日期之间的天数差
        /// </summary>
        public static int GetDaysBetween(DateTime startDate, DateTime endDate)
        {
            return (endDate.Date - startDate.Date).Days;
        }

        /// <summary>
        /// 获取两个日期之间的小时差
        /// </summary>
        public static double GetHoursBetween(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).TotalHours;
        }

        /// <summary>
        /// 获取两个日期之间的分钟差
        /// </summary>
        public static double GetMinutesBetween(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).TotalMinutes;
        }

        /// <summary>
        /// 判断是否是闰年
        /// </summary>
        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        /// <summary>
        /// 获取某个月的天数
        /// </summary>
        public static int GetDaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }
        public static string FormatDateTime(DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return dateTime.ToString(format);
        }
        /// <summary>
        /// 将日期转换为中文格式（例如：二〇二五年七月）
        /// </summary>
        /// <param name="date">要转换的日期</param>
        /// <param name="includeDay">是否包含日（例如：二〇二五年七月十一日）</param>
        /// <returns>中文格式日期字符串</returns>
        public static string ToChineseDateString(DateTime date, bool includeDay = false)
        {
            // 中文数字字符数组
            string[] chineseNumbers = { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            string[] chineseMonths = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二" };
            string[] chineseDays = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十",
                            "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十",
                            "二十一", "二十二", "二十三", "二十四", "二十五", "二十六", "二十七", "二十八", "二十九", "三十", "三十一" };

            // 转换年份
            string yearStr = "";
            foreach (char c in date.Year.ToString())
            {
                yearStr += chineseNumbers[int.Parse(c.ToString())];
            }
            yearStr += "年";

            // 转换月份
            string monthStr = chineseMonths[date.Month - 1] + "月";

            // 组合结果
            string result = yearStr + monthStr;

            // 如果需要包含日
            if (includeDay)
            {
                result += chineseDays[date.Day - 1] + "日";
            }

            return result;
        }

        /// <summary>
        /// 将日期转换为中文格式（例如：二〇二五年七月十一日 星期一）
        /// </summary>
        /// <param name="date">要转换的日期</param>
        /// <param name="includeWeekDay">是否包含星期</param>
        /// <returns>中文格式日期字符串</returns>
        public static string ToFullChineseDateString(DateTime date, bool includeWeekDay = true)
        {
            string[] chineseWeekDays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

            string result = ToChineseDateString(date, true);

            if (includeWeekDay)
            {
                result += " " + chineseWeekDays[(int)date.DayOfWeek];
            }

            return result;
        }

        /// <summary>
        /// 计算年龄
        /// </summary>
        /// <param name="birthDate">出生日期</param>
        /// <param name="referenceDate">参考日期（默认为当前日期）</param>
        public static int CalculateAge(DateTime birthDate, DateTime? referenceDate = null)
        {
            var refDate = referenceDate ?? DateTime.Now;
            int age = refDate.Year - birthDate.Year;

            if (refDate.Month < birthDate.Month ||
                (refDate.Month == birthDate.Month && refDate.Day < birthDate.Day))
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// 获取本周的第一天（周一）
        /// </summary>
        public static DateTime GetFirstDayOfWeek(DateTime date)
        {
            int delta = DayOfWeek.Monday - date.DayOfWeek;
            if (delta > 0)
                delta -= 7;
            return date.AddDays(delta).Date;
        }

        /// <summary>
        /// 获取本月的第一天
        /// </summary>
        public static DateTime GetFirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// 获取本季度的第一天
        /// </summary>
        public static DateTime GetFirstDayOfQuarter(DateTime date)
        {
            int quarter = (date.Month - 1) / 3 + 1;
            return new DateTime(date.Year, (quarter - 1) * 3 + 1, 1);
        }

        /// <summary>
        /// 获取本年的第一天
        /// </summary>
        public static DateTime GetFirstDayOfYear(DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        /// <summary>
        /// 判断两个日期是否是同一天
        /// </summary>
        public static bool IsSameDay(DateTime date1, DateTime date2)
        {
            return date1.Date == date2.Date;
        }

        /// <summary>
        /// 添加工作日（跳过周末）
        /// </summary>
        /// <param name="date">起始日期</param>
        /// <param name="businessDays">要添加的工作日天数</param>
        public static DateTime AddBusinessDays(DateTime date, int businessDays)
        {
            if (businessDays == 0) return date;

            int direction = businessDays > 0 ? 1 : -1;
            while (businessDays != 0)
            {
                date = date.AddDays(direction);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDays -= direction;
                }
            }
            return date;
        }

        /// <summary>
        /// 获取两个日期之间的工作日天数
        /// </summary>
        public static int GetBusinessDaysBetween(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return GetBusinessDaysBetween(endDate, startDate);
            }

            int businessDays = 0;
            while (startDate <= endDate)
            {
                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDays++;
                }
                startDate = startDate.AddDays(1);
            }
            return businessDays;
        }


        /// <summary>
        /// 将各种格式的日期字符串统一格式化为 yyyy-MM-dd
        /// </summary>
        /// <param name="dateString">输入的日期字符串（如 "2001/7/1 0:00:00"、"2001年7月1日"、"2001年7月"）</param>
        /// <returns>格式化后的日期字符串（如 "2001-07-01"），解析失败时返回 null</returns>
        public static string FormatDate(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                //return "1900-01-01";
                return "";
            }

            // 支持的日期格式列表
            string[] formats = {
            "yyyy/M/d H:mm:ss",    // 如 "2001/7/1 0:00:00"
            "yyyy年M月d日",         // 如 "2001年7月1日"
            "yyyy年M月",            // 如 "2001年7月"（自动补全为1日）
            "yyyy-M-d",
            "yyyy/M/d",
            "yyyy.M.d"
            };

            DateTime date;
            // 尝试解析日期
            if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date.ToString("yyyy-MM-dd");
            }

            // 尝试用更宽松的解析方式（如 "2001年7月" 可能被解析成当前日）
            if (DateTime.TryParse(dateString, out date))
            {
                return date.ToString("yyyy-MM-dd");
            }

            return null; // 解析失败
        }

        public static string FormatDateTime(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return "1900-01-01 00:00:00";
            }

            // 支持的日期格式列表
            string[] formats = {
            "yyyy/M/d H:mm:ss",    // 如 "2001/7/1 0:00:00"
            "yyyy年M月d日",         // 如 "2001年7月1日"
            "yyyy年M月",            // 如 "2001年7月"（自动补全为1日）
            "yyyy-M-d",
            "yyyy/M/d",
            "yyyy.M.d"
            };

            DateTime date;
            // 尝试解析日期
            if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date.ToString("yyyy-MM-dd H:mm:ss");
            }

            // 尝试用更宽松的解析方式（如 "2001年7月" 可能被解析成当前日）
            if (DateTime.TryParse(dateString, out date))
            {
                return date.ToString("yyyy/M/d H:mm:ss");
            }

            return null; // 解析失败
        }
    }
}