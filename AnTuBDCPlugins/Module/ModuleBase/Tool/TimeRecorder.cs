using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBase.Tool
{
    public class TimeRecorder 
    {
        private readonly Stopwatch _stopwatch;
      

        public TimeRecorder()
        {
           
            _stopwatch = Stopwatch.StartNew();
           
        }

       

        public String Dispose()
        {
            _stopwatch.Stop();
            string formattedTime = GetFormattedTime(_stopwatch.Elapsed);
          
            return formattedTime;
        }

        public static string GetFormattedTime(TimeSpan timeSpan)
        {
            double totalSeconds = timeSpan.TotalSeconds;

            if (totalSeconds < 1)
            {
                return $"{timeSpan.TotalMilliseconds:F0} 毫秒";
            }
            else if (totalSeconds < 60)
            {
                return $"{totalSeconds:F2} 秒";
            }
            else if (totalSeconds < 3600)
            {
                double minutes = Math.Floor(totalSeconds / 60);
                double seconds = totalSeconds % 60;
                return $"{minutes:F0} 分 {seconds:F2} 秒";
            }
            else
            {
                double hours = Math.Floor(totalSeconds / 3600);
                double minutes = Math.Floor((totalSeconds % 3600) / 60);
                double seconds = totalSeconds % 60;
                return $"{hours:F0} 时 {minutes:F0} 分 {seconds:F2} 秒";
            }
        }
        /// <summary>
        /// 获取运行时间
        /// </summary>
        /// <returns></returns>
        public String GetRunTime()
        {
            string formattedTime = GetFormattedTime(_stopwatch.Elapsed);

            return formattedTime;
        }
        public TimeSpan GetTime()
        {
            return _stopwatch.Elapsed;
        }

    }
}
