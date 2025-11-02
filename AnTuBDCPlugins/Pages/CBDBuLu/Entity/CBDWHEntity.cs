using BDCPlugins.Entity;
using ModuleBase.Entity;
using ModuleBase.Tool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnTuBDCPlugins.Pages.CBDBuLu.Entity
{

    /// <summary>
    /// 全量补录宗地
    /// </summary>
    public class CBDWHEntity : RecvieEntity
    {
       
        public String DKID { get; set; }
        public String DBLogID { get;  set; }
        private JObject _db_Dk;
        public JObject Db_Dk
        {
            get { return _db_Dk; }  
            set {
                DBLogID = JsonTool.GetStringValue(value, "ZDID");
                _db_Dk = value;
            } 
        }

        public bool IsCheckShp { get; set; }
        public MapFileType MapFileType { get; set; }
        public String CbdMapPath { get; set; }
        private JObject _jcxx;
        public JObject Jcxx
        {
            get { return _jcxx; }
            set
            {
                DKID = JsonTool.GetStringValue(value, "地块ID");
                DBLogID = JsonTool.GetStringValue(value, "数据库记录ID");
                _jcxx = value;
            }
        }
        /// <summary>
        /// 土地用途
        /// </summary>
        public JObject Tdyt { get; set; }
        /// <summary>
        /// 土地承包信息
        /// </summary>
        public JObject Tdcbxx { get; set; }
        /// <summary>
        /// 土地承包经营权
        /// </summary>
        public JObject Tdcbjyq { get; set; }
        /// <summary>
        /// 土地承包经营权人
        /// </summary>
        public JArray Tdcbjyqrs { get; set; }
        public DataOperationType DataOperationType { get; set; }
        public JObject Common { get; set; }
        public CBDWHEntity():base()
        {
           
            Tdcbjyqrs = new JArray();
            IsCheckShp = true;
            DataOperationType = DataOperationType.ONLY_READ;
            Common = new JObject();
        }
       
    }
}
