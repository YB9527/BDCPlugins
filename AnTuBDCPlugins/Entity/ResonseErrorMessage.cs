using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.Entity
{
    public class ResonseErrorMessage
    {
        public ResonseErrorMessage(JObject json, string descreble, string keyFiled, string keyValue,object data = null)
        {
            if(json != null)
            {
                this.Code = json["code"] != null ? json["code"].ToString() : null;
                this.Message = json["msg"] != null ? json["msg"].ToString() : null;
            }
            this.KeyValue = keyValue;
            this.KeyFiled = keyFiled;
            this.Descreble = descreble;
            this.Data = data;
        }
       /* public ResonseErrorMessage( string descreble, string keyFiled, string keyValue)
        {
            this.KeyValue = keyValue;
            this.KeyFiled = keyFiled;
            this.Descreble = descreble;
        }*/
        public String Code { get; set; }
        public String Message { get; set; }
        public String Descreble { get; set; }
        public String KeyValue { get; set; }
        public String KeyFiled { get; set; }
        public String GroupName { get; set; }
        /// <summary>
        /// 当前数据对象
        /// </summary>
        public object Data { get; set; }

        internal static void AddError(List<ResonseErrorMessage> resonseErrorMessages, JObject result, string v, string dCB_zddmKey, string item)
        {
            
        }
    }
}
