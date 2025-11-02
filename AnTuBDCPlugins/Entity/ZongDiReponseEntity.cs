using BDCPlugins.BDCException;
using BDCPlugins.tool;
using ModuleBase.Tool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.Entity
{
    public class ZongDiReponseEntity
    {
        /// <summary>
        /// 通用存储
        /// </summary>
        public JObject Common { get; set; }
        /// <summary>
        /// 受理信息
        /// </summary>
        public JObject SLXX { get; set; }
        /// <summary>
        /// 项目信息
        /// </summary>
        public JObject Xmxx { get; set; }
        
        /// <summary>
        /// 宗地基本信息
        /// </summary>
        public JObject ZDXX { get; set; }



        /// <summary>
        /// 详细宗地信息
        /// </summary>
        public JObject ZDXXDetails { get; set; }


        /// <summary>
        /// 自然幢基本信息
        /// </summary>
        public JObject ZRZXX { get; set; }


        /// <summary>
        /// 自然幢详情
        /// </summary>
        public JObject ZRZXXDetails { get; set; }


        public String XMXXID
        {
            get {
                String XMXXID = JsonTool.GetStringValue(Xmxx, "XMXXID");
                return XMXXID;

            }
        }
        public String ZDID
        {
            get
            {
                String ZDID = JsonTool.GetStringValue(ZDXX, "ZDID");
                return ZDID;

            }
        }
        public String SCZRZID
        {
            get
            {
                String SCZRZID = JsonTool.GetStringValue(ZRZXX, "SCZRZID");
                return SCZRZID;

            }
        }

        public String NewUUID
        {
            get
            {
                String uuid = UuidTool.Create();
                return uuid;

            }
        }
        public String FWJG { get; set; }


        public JObject FwDict { get; set; }
        public void SetFWJGByName(String name)
        {
            if (FwDict == null)
            {
                throw new ExceptionQuanJi("房屋方面字典未查找");

            }

            JArray jArray = FwDict.Value<JArray>("form-FWJG");
            foreach (var item in jArray)
            {
                JObject jobject = (JObject)item;
                String dictname = JsonTool.GetStringValue(jobject, "Name");
                if (dictname == name)
                {
                    FWJG =  JsonTool.GetStringValue(jobject, "Value");

                    return;
                }
            }
            throw new ExceptionQuanJi("房屋结构："+ name+",在数据库中没有找到，查看你的房屋结构是否规范。规范结构："+ jArray.ToString());
        }

        public String SCHID { get; set; }


        
        public String ZhuanChuActInst_ID { get; set; }

       
        /// <summary>
        /// 待办理数据
        /// </summary>
        public JObject DaiBanLi { get; internal set; }

        public ZongDiReponseEntity()
        {
            Common = new JObject();
           
        }


        /// <summary>
        /// 不动产项目信息
        /// </summary>
        public JObject BDCXMXX { get; set; }
        /// <summary>
        /// 不动产信息
        /// </summary>
        public JObject BDCXX { get; internal set; }


        /// <summary>
        /// 不动产登记系统-查询行政区信息
        /// </summary>
        public JObject BDCXZQXX { get; internal set; }

        /// <summary>
        ///  不动产登记系统-查询权利信息
        /// </summary>

        public JObject BDCQLXX { get; internal set; }

        /// <summary>
        /// 不动产登记系统-查询产权信息
        /// </summary>

        public JObject ChanChanQuanXinXi { get; set; }

        /// <summary>
        /// 数据库查到的扫描件目录
        /// </summary>
        public JObject ML { get; set; }

        /// <summary>
        /// 我的目录名称
        /// </summary>
        
        public String MyMLName { get; set; }
    }
}
