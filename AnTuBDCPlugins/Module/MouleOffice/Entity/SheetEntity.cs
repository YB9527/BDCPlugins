using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleOffice.Entity
{
    public class SheetEntity
    {

        public String PrimaryKey { get; set; }
        public List<Dictionary<String,Object>> Rows { get; set; }

        public SheetEntity()
        {
            Rows = new List<Dictionary<string, object>>();
        }
    }
}
