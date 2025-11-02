using ModuleBase.Tool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleBase.Component
{
   

    public class ButtonDownloadExcelDialog : DevExpress.XtraEditors.SimpleButton
    {
        public String Self_SrcFileName { get; set; }
        public String Self_Title { get; set; }
        public String Self_SaveFileName { get; set; }

        public ButtonEditFileSelect Self_ButtonEditFileSelect { get; set; }

        public bool Self_IsShowOk { get; set; }
        //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonOpenDirDialog));
        public ButtonDownloadExcelDialog()
        {
            // this.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.Click += ButtonOpenExcelDialog_Click;
            this.Self_IsShowOk = true;
        }

        private void ButtonOpenExcelDialog_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog ofd = new SaveFileDialog())
            {
                ofd.Title = Self_Title != null ? Self_Title: this.Text;
                ofd.Filter = "Excel文件|;*.xls";
                ofd.FileName = Self_SaveFileName;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    
                    FileTool.CopyFile(AppTool.GetAppDirectory() + Self_SrcFileName, ofd.FileName);
                    if(Self_ButtonEditFileSelect != null)
                    {
                        Self_ButtonEditFileSelect.Text = ofd.FileName;
                    }
                    if (Self_IsShowOk)
                    {
                        

                        if (MessageBox.Show("保存完成！,是否打开文件？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Process.Start(ofd.FileName);
                        }
                    }
                }
            }

        }
    }
}
