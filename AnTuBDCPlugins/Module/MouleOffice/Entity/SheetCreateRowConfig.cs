using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleOffice.Entity
{
    public class SheetCreateRowConfig
    {
        public IWorkbook Workbook { get; set; }
        public int HeaderIndex { get; set; }
        public String SheetName { get; set; }


    }
}
