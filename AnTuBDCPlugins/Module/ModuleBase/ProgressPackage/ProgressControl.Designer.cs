namespace ModuleBase.ProgressPackage
{
    partial class ProgressControl
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTj = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            // 新增btnClose
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Height = 60;
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1600, 60);
            this.panel1.TabIndex = 6;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 18);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(650, 24);
            this.progressBar.TabIndex = 0;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = false;
            this.lblProgress.Location = new System.Drawing.Point(670, 18);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(80, 24);
            this.lblProgress.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "0 / 100";

            ///

            this.lblTj.AutoSize = true;
            this.lblTj.Location = new System.Drawing.Point(10, 45);
            this.lblTj.Name = "lblTj";
           // this.lblTj.Size = new System.Drawing.Size(80, 24);
           // this.lblTj.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Italic);
            this.lblTj.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTj.TabIndex = 3;
            this.lblTj.Text = "当前运行时间: {time} ，成功数量: {successCount} ，失败数量: {failCount}。   预计运行时间：{estimatedTime}，预计完成时间：{completionTime} ";

            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(750, 18);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(750, 18);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 24);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1.Controls
            // 
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(this.progressBar);
            this.panel1.Controls.Add(this.lblProgress);
            this.panel1.Controls.Add(this.lblTj);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnClose);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLog.ForeColor = System.Drawing.Color.Black;
            this.txtLog.Location = new System.Drawing.Point(12, 80);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(700, 400);
            this.txtLog.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Controls.Add(this.txtLog);
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1000, 530);
            this.panel2.TabIndex = 7;
            // 
            // ProgressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ProgressControl";
            this.Size = new System.Drawing.Size(1000, 600);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.btnCancel.BringToFront();
            this.btnClose.BringToFront();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTj;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        // Designer字段
        private System.Windows.Forms.Button btnClose;
    }
} 