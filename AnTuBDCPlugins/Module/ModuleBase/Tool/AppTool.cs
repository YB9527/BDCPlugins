using BDCPlugins.tool;
using ModuleBase.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBase.Tool
{
    public class AppTool
    {
        public static string GetAppDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            // Console.WriteLine("Current Directory: " + currentDirectory);
            return currentDirectory;
        }

        public static string GetTempDirectory()
        {
            string currentDirectory = AppTool.GetAdministroDir() + "/temp/";
            if (!Directory.Exists(currentDirectory))
            {
                Directory.CreateDirectory(currentDirectory);
            }
            return currentDirectory;
        }
        public static string GetTemplateDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory() + "/template/";
            if (!Directory.Exists(currentDirectory))
            {
                Directory.CreateDirectory(currentDirectory);
            }
            return currentDirectory;
        }

        public static string GetConfigDir()
        {
            string configDirectory = Directory.GetCurrentDirectory() + "/config/";
            return configDirectory;
        }

        private static CacheTool CacheTool;
        public static CacheTool GetCacheTool()
        {
            if (CacheTool == null)
            {
                CacheTool = new CacheTool();
                CacheTool.Read();
            }
            return CacheTool;
        }

        public static String UserTag { get; set; }
        public static User CurrentUser { private set; get; }
        public static void SetCurrentUser(User user)
        {
            UserTag = UuidTool.CreateNoHyphens();
            CurrentUser = user;

        }
        /// <summary>
        /// 获取用户目录
        /// </summary>
        /// <returns></returns>
        public static string GetAdministroDir()
        {
            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            String AdministroDir = userProfilePath + @"/房地一体/";
            if (!Directory.Exists(AdministroDir))
            {
                Directory.CreateDirectory(AdministroDir);
            }
            return AdministroDir;
        }
        public static string GetTimeTxt()
        {
            string timestamp = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
            string fileName2 = Path.Combine(AppTool.GetTempDirectory(), $"{timestamp}_临时文件.txt");
            return fileName2;
        }
        public static string GetTimeXls(string desc=null)
        {
            string timestamp = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
            String name = desc == null ?  $"{timestamp}_临时文件.xls": $"{timestamp}_临时文件_"+desc +".xls";
            string fileName2 = Path.Combine(AppTool.GetTempDirectory(), name);
            return fileName2;
        }

        public static string GetTime(string name)
        {
            string timestamp = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
            name = name == null ? $"{timestamp}_" : $"{timestamp}_" + name;
            return GetAdministroDir()+ name;
        }




        /// <summary>
        /// 获取临时目录
        /// </summary>
        /// <returns></returns>
        public static String GetTempDir()
        {
             return GetAppDirectory()+@"\temp\";
        }
    }
}
