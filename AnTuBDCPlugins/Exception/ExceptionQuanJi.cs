
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.BDCException
{


    public class ExceptionQuanJi : Exception
    {
     
        public JObject Data { get; }
        public JObject Response { get; }
        public String Describle { get; }
        public String GroupName { get; set; }
        public ExceptionQuanJi(String describle, String gruopName=null) : base(describle)
        {
            this.Describle = describle;

        }
        public ExceptionQuanJi(String describle, JObject response, String gruopName = null) : base(describle)
        {
            this.Response = response;
            this.Describle = describle;
        }
        public ExceptionQuanJi(String describle, JObject response, JObject data, String gruopName = null) : base(describle)
        {
            this.Response = response;
            this.Data = data;
            this.Describle = describle;
            this.GroupName = gruopName;
        }

    }
}
