using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleBase.Component
{
   

    public class ButtonOpenExcelDialog : DevExpress.XtraEditors.SimpleButton
    {
        public String Self_OpenFileDialogTitle { get; set; }
        public ButtonEditFileSelect Self_ButtonEditFileSelect { get; set; }
        //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonOpenDirDialog));
        public ButtonOpenExcelDialog()
        {
            // this.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.Click += ButtonOpenExcelDialog_Click;
        }

        private void ButtonOpenExcelDialog_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = Self_OpenFileDialogTitle != null ? Self_OpenFileDialogTitle : this.Text;
                ofd.Filter = "Excel文件|*.xls";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Self_ButtonEditFileSelect.Text = ofd.FileName;
                }
            }

        }
    }
}
