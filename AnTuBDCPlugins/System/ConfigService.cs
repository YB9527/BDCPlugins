using BDCPlugins.BDCException;
using BDCPlugins.Config;
using BDCPlugins.Entity;
using BDCPlugins.tool;
using ModuleBase.Tool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.BDCSystem
{
    public class ConfigService
    {
         public List<RequestEntity> requestEntities { get; private set; }
        public ConfigService()
        {
            requestEntities = new List<RequestEntity>();
        }
        public static ConfigService ReadConfig()
        {
            string appPath = AppTool.GetAppDirectory();
            List<String> configPaths = FileTool.FindAllFiles(appPath + @"\Config\RequestConfig", "*.json");
            ConfigService configService = new ConfigService();

            foreach (var configPath in configPaths)
            {
                if (configPath.Contains("原始信息"))
                {
                    continue;
                }

                List<RequestEntity> requestEntities = JsonTool.ReadJsonFile<List<RequestEntity>>(configPath);
                configService.requestEntities.AddRange(requestEntities);
            }


            

            return configService;

            
        }

        public RequestEntity FindRequestEntity(string name)
        {
            name = ProjectConfig.GetRquestName(name);
            if (requestEntities == null || string.IsNullOrWhiteSpace(name))
                return null;
            RequestEntity requestEntity = requestEntities.FirstOrDefault(r => r.Name.Trim()?.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase) == true);
            if (requestEntity == null)
            {
                throw new ExceptionQuanJi("没有再配置文件中，配置请求方法，name = " + name);
            }
            JObject json = JObject.FromObject(requestEntity);
            return JObject.Parse(json.ToString()).ToObject<RequestEntity>();
        }

    }
}
