using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleOffice.ExceptionBase
{
    public class ExceptionSheetNotFound : Exception
    {
        public ExceptionSheetNotFound(String msg) : base(msg)
        {

        }
    }
}
