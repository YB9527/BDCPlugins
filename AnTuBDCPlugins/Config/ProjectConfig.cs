using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.Config
{
    public enum Project
    {
        JingYan,
        ZiGong,
    }
    public enum ProjectSystem
    {
        QuanJi,
        DengJi,
    }
    public  class ProjectConfig
    {
        public static string Empyt = "";

        public static ProjectSystem ProjectSystem { get; private set; }
        public static Project Project { get; private set; }
        public static void SetCurrentProject(Project project)
        {
            ProjectConfig.Project = project;
        }
        public static void SetProjectSystem(ProjectSystem ProjectSystem)
        {
            ProjectConfig.ProjectSystem = ProjectSystem;
        }
        public static string BaseURL = "http://59.213.130.130:4062";
        public static string QujiBaseUrl = "http://172.244.11.105:81";
        public static string BuDongChanBaseUrl = "http://59.213.130.130:4062";
        public static string TOKEN = "";



        public static string ProjectType { get; internal set; }

        public static String GetRquestName(String name)
        {
            String result = name;
            String[] requestNames = new string[]
            {
               
            };
            if (requestNames.Contains(name))
            {
               
            }
            
            return result;
        }

    }
}
