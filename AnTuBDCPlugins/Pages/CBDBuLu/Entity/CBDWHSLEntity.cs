using BDCPlugins.Entity;
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
    public class CBDWHSLEntity : RecvieEntity
    {
        /// <summary>
        /// 受理名称
        /// </summary>
        public String Slmc { get; set; }
        public String MapsFilePath { get; set; }
        /// <summary>
        /// 所有地块
        /// </summary>
        public List<CBDWHEntity> CBDWHEntities { get; set; }

        /// <summary>
        /// 当前操作的地类
        /// </summary>
        public CBDWHEntity CurrentDK { get; set; }
        public JObject Common { get; set; }

        public ZongDiReponseEntity ZongDiReponseEntity { get; set; }


        public CBDWHSLEntity():base()
        {
            CBDWHEntities = new List<CBDWHEntity>();
            Common = new JObject();
            ZongDiReponseEntity = new ZongDiReponseEntity();
        }
       
    }
}
