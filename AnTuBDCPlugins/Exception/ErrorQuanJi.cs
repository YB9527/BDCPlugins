
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.BDCException
{


    public class ErrorQuanJi : Exception
    {
        public  Object Data { get; }
        public JObject Response { get; }
        public String Describle { get; }
        public ErrorQuanJi(String describle, JObject response, Object data) :base(describle)
        {
            this.Response = response;
            this.Data = data;
            this.Describle = describle;
        }

        
    }
}
