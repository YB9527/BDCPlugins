using BDCPlugins.Config;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.Entity
{
    public enum Tag
    {
        FILE,
        NONE,
    } 
    public class RequestEntity
    {

        public RequestEntity()
        {
            Tag = Tag.NONE;
        }
        public Tag Tag { get; set; }
        public string Name { get; set; }
        public RequestMethod Method { get; set; }
        public string Url { get; set; }
        public Dictionary<String, Object> Body { get; set; }
        public JArray ArrayBody { get; set; }
        public Dictionary<String,String> Query { get; set; }
        public Dictionary<String,String> Headers { get; set; }
        public String  FilePath { get; set; }

        public bool IsPostArray { get; set; }

        public int RequestDealy { get;  set; }
        public bool Still { get;  set; }

    }
}
