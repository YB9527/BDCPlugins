using ModuleOffice.Tool;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Office
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // 创建或加载工作簿
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            // 填充数据（示例）
            sheet.CreateRow(0).CreateCell(0).SetCellValue("Hello NPOI!");

            // 保存 Excel 文件
            string savePath = @"d:\2.xls";
            using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
            Console.WriteLine("Excel 保存成功！");

            ExcelTool.Save(ExcelTool.ReadSheet(@"d:\2.xls", 0).Workbook, @"d:\3.xls");
            ExcelTool.Save(ExcelTool.ReadSheet(@"d:\2.xlsx", 0).Workbook, @"d:\3.xlsx");
        }

        private void fileToolPage1_Load(object sender, EventArgs e)
        {

        }
    }
}
