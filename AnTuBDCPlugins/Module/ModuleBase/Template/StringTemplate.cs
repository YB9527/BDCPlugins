using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCPlugins.template
{
    public class StringTemplate
    {
        public static StringTemplate Instance = new StringTemplate();
        public  String CheckExcelAndContinue { get; set; }

       
        public StringTemplate()
        {
            CheckExcelAndContinue = "当前文件不可写，请先关闭Excel,再点击‘确定’，程序继续完成任务";
        }
    }
}
