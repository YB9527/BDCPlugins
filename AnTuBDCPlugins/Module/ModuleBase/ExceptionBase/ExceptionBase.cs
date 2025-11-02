using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBase.ExceptionBase
{
    public class ExceptionBase : Exception
    {
        public ExceptionBase(String msg) : base(msg)
        {

        }
    }
}
