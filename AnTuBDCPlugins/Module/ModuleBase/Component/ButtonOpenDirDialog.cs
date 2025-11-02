using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleBase.Component
{
    public class ButtonOpenDirDialog : DevExpress.XtraEditors.SimpleButton
    {
        public String Self_DirPath { get; private set; }
        public ButtonEditDirSelect Self_ButtonEditDirSelect { get; set; }
        //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonOpenDirDialog));
        public ButtonOpenDirDialog()
        {
           // this.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.Click += ButtonOpenExcelDialog_Click;
        }

        private void ButtonOpenExcelDialog_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    Self_DirPath = fbd.SelectedPath;
                    if (Self_ButtonEditDirSelect != null)
                    {
                        Self_ButtonEditDirSelect.Text = Self_DirPath;
                    }
                }
                else
                {
                    Self_DirPath = null;
                }
            }
        }
    }
}
