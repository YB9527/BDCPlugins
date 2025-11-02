using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleOffice.ExceptionBase
{
    public class ExceptionExcel : Exception
    {
        public ExceptionExcel(String msg) : base(msg)
        {

        }
    }
}
