using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleBase.Component
{
    
   

    public class ButtonSaveExcelDialog : DevExpress.XtraEditors.SimpleButton
    {
        public String Self_SaveFileDialogTitle { get; set; }
        public String Self_SaveDefaultName { get; set; }
        public ButtonEditFileSelect Self_ButtonEditFileSelect { get; set; }
        //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonOpenDirDialog));
        public ButtonSaveExcelDialog()
        {
            // this.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.Click += ButtonOpenExcelDialog_Click;
        }

        private void ButtonOpenExcelDialog_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog ofd = new SaveFileDialog())
            {
                ofd.Title = Self_SaveFileDialogTitle != null ? Self_SaveFileDialogTitle : this.Text;
                ofd.Filter = "Excel文件|*.xls";
                ofd.FileName = Self_SaveDefaultName;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Self_ButtonEditFileSelect.Text = ofd.FileName;
                }
            }
        }
    }
}
