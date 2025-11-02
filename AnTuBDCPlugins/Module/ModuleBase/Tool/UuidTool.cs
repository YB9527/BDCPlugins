using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.tool
{
    public class UuidTool
    {
        public static String CreateNoHyphens()
        {

            Guid newGuid = Guid.NewGuid();
            // 无连字符格式
            string noHyphens = newGuid.ToString("N"); // "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            return noHyphens;
        }

        public static String Create()
        {
            Guid newGuid = Guid.NewGuid();
            String id = newGuid.ToString();
            return id;
        }
    }
}
