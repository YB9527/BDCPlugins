
namespace ModuleOffice.Page
{
    partial class FileToolPage
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileToolPage));
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton8 = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // simpleButton4
            // 
            this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
            this.simpleButton4.Location = new System.Drawing.Point(570, 154);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(157, 41);
            this.simpleButton4.TabIndex = 120;
            this.simpleButton4.Text = "下载文件复制模板";
            this.simpleButton4.Click += new System.EventHandler(this.button1_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(223, 104);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(157, 41);
            this.simpleButton1.TabIndex = 121;
            this.simpleButton1.Text = "导出文件夹所有文件";
            this.simpleButton1.Click += new System.EventHandler(this.button48_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(223, 154);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(157, 41);
            this.simpleButton2.TabIndex = 122;
            this.simpleButton2.Text = "导出需要复制文件";
            this.simpleButton2.Click += new System.EventHandler(this.button2_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.simpleButton3.Location = new System.Drawing.Point(397, 154);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(157, 41);
            this.simpleButton3.TabIndex = 123;
            this.simpleButton3.Text = "文件复制";
            this.simpleButton3.Click += new System.EventHandler(this.button50_Click);
            // 
            // simpleButton5
            // 
            this.simpleButton5.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton5.Image")));
            this.simpleButton5.Location = new System.Drawing.Point(225, 207);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(157, 41);
            this.simpleButton5.TabIndex = 124;
            this.simpleButton5.Text = "导出需要移动文件";
            this.simpleButton5.Click += new System.EventHandler(this.button3_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton6.Image")));
            this.simpleButton6.Location = new System.Drawing.Point(397, 207);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(157, 41);
            this.simpleButton6.TabIndex = 125;
            this.simpleButton6.Text = "文件移动";
            this.simpleButton6.Click += new System.EventHandler(this.button4_Click);
            // 
            // simpleButton7
            // 
            this.simpleButton7.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton7.Image")));
            this.simpleButton7.Location = new System.Drawing.Point(225, 264);
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.Size = new System.Drawing.Size(157, 41);
            this.simpleButton7.TabIndex = 126;
            this.simpleButton7.Text = "导出需要删除文件";
            this.simpleButton7.Click += new System.EventHandler(this.button5_Click);
            // 
            // simpleButton8
            // 
            this.simpleButton8.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton8.Image")));
            this.simpleButton8.Location = new System.Drawing.Point(397, 264);
            this.simpleButton8.Name = "simpleButton8";
            this.simpleButton8.Size = new System.Drawing.Size(157, 41);
            this.simpleButton8.TabIndex = 127;
            this.simpleButton8.Text = "文件删除";
            this.simpleButton8.Click += new System.EventHandler(this.button6_Click);
            // 
            // FileToolPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.simpleButton8);
            this.Controls.Add(this.simpleButton7);
            this.Controls.Add(this.simpleButton6);
            this.Controls.Add(this.simpleButton5);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton4);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "FileToolPage";
            this.Padding = new System.Windows.Forms.Padding(500);
            this.Size = new System.Drawing.Size(908, 549);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
        private DevExpress.XtraEditors.SimpleButton simpleButton8;
    }
}
