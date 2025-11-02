using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModuleBase.ProgressPackage.ProgressDialog;

namespace ModuleBase.ProgressPackage
{
    public class ListRequestOption
    {
        public delegate bool BeforeAction(IProgressControl gp);
        public BeforeAction BeforeActionDel { get; set; }


        public delegate bool FinishAction(IProgressControl gp);
        public FinishAction FinishActionDel { get; set; }

    }
}
