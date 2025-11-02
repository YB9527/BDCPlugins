using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.Entity
{
    public class LiuChengShuVo
    {
        public JArray LiuChengShu { get; set; }


        public String AcceptUserName { get; set; }
        public JObject AcceptUser
        {
            get
            {

                return FindObjectByName(LiuChengShu, AcceptUserName);
            }
        }


        public  String QuanJiHeDingName { get; set; }
        public JObject QuanJiHeDing { get{

                JObject json =  FindObjectByName(LiuChengShu, QuanJiHeDingName);
                return json;
            }
        }

        public JObject treeInfos0Value
        {
            get
            {
                JObject json = this.QuanJiHeDing;

                return FindObjectByName(LiuChengShu, QuanJiHeDingName);
            }
        }

        public static JObject FindObjectByName(JArray tree, string name)
        {
                if (tree == null) return null;

                foreach (JObject node in tree)
                {
                    // Check if current node matches
                    if (node["Name"]?.ToString() == name)
                    {
                        return node;
                    }

                    // Recursively search in children
                    var children = node["children"] as JArray;
                    if (children != null)
                    {
                        var found = FindObjectByName(children, name);
                        if (found != null)
                        {
                            return found;
                        }
                    }
                }

                return null;
            }
        }
}
