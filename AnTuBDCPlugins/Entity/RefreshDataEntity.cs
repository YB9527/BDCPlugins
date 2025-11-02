using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.Entity
{
    public class RefreshMainDataEntity
    {

        /// <summary>
        /// 要修改的数据
        /// </summary>
        public JObject Data { get; internal set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public String TaskName { get; set; }

        /// <summary>
        /// 详情任务名称
        /// </summary>
        public String DetailsTaskName { get; set; }
        /// <summary>
        /// 请求详情名称
        /// </summary>
        public String RequestDetailsName { get; set; }

        /// <summary>
        /// 如果有这个字段，就不用查询详情了
        /// </summary>
        public string CheckDbDetailsFieldNameVaild { get; internal set; }
        /// <summary>
        /// 详情字段名称
        /// </summary>
        public String DbDetailsFieldName { get; set; }

        public String DeleteFieldName { get; set; }
        /// <summary>
        /// 修改任务名称
        /// </summary>
        public String UpdateTaskName { get; set; }
        /// <summary>
        /// 请求修改名称
        /// </summary>
        public String RequestUpdateName { get; set; }

       
        public Func<bool> RequestDetailsFinishAction { get; internal set; }
        
        public string UpdateFieldName { get; internal set; }

        public RefreshMainDataEntity()
        {
            this.DeleteFieldName = "${ZongDiReponseEntity.Common._DeleteDBData}";
        }

    }
}
