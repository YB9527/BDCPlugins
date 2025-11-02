using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.Entity
{
    public class RefreshListEntity
    {
        /// <summary>
        /// 检查任务是否能进行
        /// </summary>
     //   public String CheckFiledCanTask { get; set; }
        /// <summary>
        /// 操作数据，没有数据时，就不用进行，和CheckFiledCanTask 同理
        /// </summary>
        public JArray Datas { get; internal set; }
        public String Temp_DBDataFieldName { get; internal set; }
        public String Temp_TBDataFieldName { get; internal set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public String TaskName { get; set; }
        /// <summary>
        /// 列表任务名称
        /// </summary>
        public String ListTaskName { get; set; }
        /// <summary>
        /// 请求列表名称
        /// </summary>
        public String RequestListName { get; set; }
        /// <summary>
        /// 列表字段字段
        /// </summary>
       
        public String ListFieldName { get; set; }
        /// <summary>
        /// 详情任务名称
        /// </summary>
        public String DetailsTaskName { get; set; }
        /// <summary>
        /// 请求详情名称
        /// </summary>
        public String RequestDetailsName { get; set; }

        /// <summary>
        /// 数据库数据简单信息
        /// </summary>
        public string DbDataFieldName { get; internal set; }
        /// <summary>
        /// 详情字段名称
        /// </summary>
        public String DbDetailsFieldName { get; set; }
        /// <summary>
        /// 新增任务名称
        /// </summary>
        public String NewTaskName { get; set; }
        /// <summary>
        /// 请求新增名称
        /// </summary>
        public String RequestNewName { get; set; }

        public String DeleteFieldName { get; set; }
        /// <summary>
        /// 删除任务名称
        /// </summary>
        public String DeleteTaskName { get; set; }
        /// <summary>
        /// 请求删除名称
        /// </summary>
        public String RequestDeleteName { get; set; }
        /// <summary>
        /// 修改任务名称
        /// </summary>
        public String UpdateTaskName { get; set; }
        /// <summary>
        /// 请求修改名称
        /// </summary>
        public String RequestUpdateName { get; set; }
        /// <summary>
        /// 判断是否是同一个数据action
        /// </summary>
        public Func<JObject, JObject, bool> IsEqAction { get; internal set; }

        public RefreshListEntity()
        {
            this.DeleteFieldName = "${ZongDiReponseEntity.Common._DeleteDBData}";
            this.Temp_DBDataFieldName = "${ZongDiReponseEntity.Common._Temp_DBData}";
            this.Temp_TBDataFieldName = "${ZongDiReponseEntity.Common._Temp_TBData}";
        }

    }
}
