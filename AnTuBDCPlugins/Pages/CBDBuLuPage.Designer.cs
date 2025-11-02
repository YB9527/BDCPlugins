
namespace AnTuBDCPlugins.Pages
{
    partial class CBDBuLuPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CBDBuLuPage));
            this.textBox10 = new ModuleBase.Component.ButtonEditFileSelect();
            this.textBox11 = new ModuleBase.Component.ButtonEditDirSelect();
            this.buttonEditFileSelect1_error = new ModuleBase.Component.ButtonEditFileSelect();
            this.label3 = new System.Windows.Forms.Label();
            this.simpleButton19 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton18 = new ModuleBase.Component.ButtonOpenExcelDialog();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new ModuleBase.Component.ButtonDownloadExcelDialog();
            this.simpleButton2 = new ModuleBase.Component.ButtonOpenDirDialog();
            this.simpleButton1 = new ModuleBase.Component.ButtonOpenExcelDialog();
            ((System.ComponentModel.ISupportInitialize)(this.textBox10.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox11.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditFileSelect1_error.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox10
            // 
            this.textBox10.EditValue = "D:\\测试\\承包地\\2试点村（含试点户）\\承包地维护表.xls";
            this.textBox10.Location = new System.Drawing.Point(23, 55);
            this.textBox10.Margin = new System.Windows.Forms.Padding(4);
            this.textBox10.Name = "textBox10";
            this.textBox10.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.textBox10.Self_BindAttr = "";
            this.textBox10.Self_BindData = null;
            this.textBox10.Self_CacheTag = "承包地维护表";
            this.textBox10.Self_IsMust = true;
            this.textBox10.Size = new System.Drawing.Size(794, 28);
            this.textBox10.TabIndex = 156;
            // 
            // textBox11
            // 
            this.textBox11.EditValue = "D:\\测试\\承包地\\2试点村（含试点户）\\分";
            this.textBox11.Location = new System.Drawing.Point(23, 143);
            this.textBox11.Margin = new System.Windows.Forms.Padding(4);
            this.textBox11.Name = "textBox11";
            this.textBox11.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.textBox11.Self_BindData = null;
            this.textBox11.Self_CacheTag = "承包地维护图文件夹";
            this.textBox11.Self_IsMust = true;
            this.textBox11.Size = new System.Drawing.Size(794, 28);
            this.textBox11.Slef_BindAttr = "";
            this.textBox11.TabIndex = 155;
            // 
            // buttonEditFileSelect1_error
            // 
            this.buttonEditFileSelect1_error.EditValue = "";
            this.buttonEditFileSelect1_error.Location = new System.Drawing.Point(23, 232);
            this.buttonEditFileSelect1_error.Margin = new System.Windows.Forms.Padding(4);
            this.buttonEditFileSelect1_error.Name = "buttonEditFileSelect1_error";
            this.buttonEditFileSelect1_error.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditFileSelect1_error.Self_BindAttr = "";
            this.buttonEditFileSelect1_error.Self_BindData = null;
            this.buttonEditFileSelect1_error.Self_CacheTag = "承包地维护错误表";
            this.buttonEditFileSelect1_error.Self_IsMust = false;
            this.buttonEditFileSelect1_error.Size = new System.Drawing.Size(794, 28);
            this.buttonEditFileSelect1_error.TabIndex = 154;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(20, 209);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(512, 18);
            this.label3.TabIndex = 153;
            this.label3.Text = "提示：如果选择导出的错误文件，那么可以选择只执行这些数据";
            // 
            // simpleButton19
            // 
            this.simpleButton19.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton19.Image")));
            this.simpleButton19.Location = new System.Drawing.Point(1083, 209);
            this.simpleButton19.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton19.Name = "simpleButton19";
            this.simpleButton19.Size = new System.Drawing.Size(112, 62);
            this.simpleButton19.TabIndex = 152;
            this.simpleButton19.Text = "移除";
            // 
            // simpleButton18
            // 
            this.simpleButton18.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton18.Image")));
            this.simpleButton18.Location = new System.Drawing.Point(835, 209);
            this.simpleButton18.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton18.Name = "simpleButton18";
            this.simpleButton18.Self_ButtonEditFileSelect = this.buttonEditFileSelect1_error;
            this.simpleButton18.Self_OpenFileDialogTitle = null;
            this.simpleButton18.Size = new System.Drawing.Size(231, 62);
            this.simpleButton18.TabIndex = 151;
            this.simpleButton18.Text = "选择导出的错误表";
            // 
            // simpleButton5
            // 
            this.simpleButton5.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton5.Image")));
            this.simpleButton5.Location = new System.Drawing.Point(23, 308);
            this.simpleButton5.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(320, 62);
            this.simpleButton5.TabIndex = 150;
            this.simpleButton5.Text = "执行承包地维护";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
            this.simpleButton4.Location = new System.Drawing.Point(1083, 21);
            this.simpleButton4.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Self_ButtonEditFileSelect = null;
            this.simpleButton4.Self_IsShowOk = true;
            this.simpleButton4.Self_SaveFileName = "承包地维护表.xls";
            this.simpleButton4.Self_SrcFileName = "/template/模板-承包地维护表.xls";
            this.simpleButton4.Self_Title = "下载承包地维护模板";
            this.simpleButton4.Size = new System.Drawing.Size(236, 62);
            this.simpleButton4.TabIndex = 149;
            this.simpleButton4.Text = "下载承包地维护模板";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.simpleButton2.Location = new System.Drawing.Point(833, 109);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Self_ButtonEditDirSelect = this.textBox11;
            this.simpleButton2.Size = new System.Drawing.Size(231, 62);
            this.simpleButton2.TabIndex = 148;
            this.simpleButton2.Text = "选择承包地图文件夹";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(835, 21);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Self_ButtonEditFileSelect = this.textBox10;
            this.simpleButton1.Self_OpenFileDialogTitle = null;
            this.simpleButton1.Size = new System.Drawing.Size(231, 62);
            this.simpleButton1.TabIndex = 147;
            this.simpleButton1.Text = "选择承包地维护表";
            // 
            // CBDBuLuPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.buttonEditFileSelect1_error);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.simpleButton19);
            this.Controls.Add(this.simpleButton18);
            this.Controls.Add(this.simpleButton5);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Name = "CBDBuLuPage";
            this.Size = new System.Drawing.Size(1403, 884);
            ((System.ComponentModel.ISupportInitialize)(this.textBox10.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox11.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditFileSelect1_error.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ModuleBase.Component.ButtonEditFileSelect textBox10;
        private ModuleBase.Component.ButtonEditDirSelect textBox11;
        private ModuleBase.Component.ButtonEditFileSelect buttonEditFileSelect1_error;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SimpleButton simpleButton19;
        private ModuleBase.Component.ButtonOpenExcelDialog simpleButton18;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private ModuleBase.Component.ButtonDownloadExcelDialog simpleButton4;
        private ModuleBase.Component.ButtonOpenDirDialog simpleButton2;
        private ModuleBase.Component.ButtonOpenExcelDialog simpleButton1;
    }
}
