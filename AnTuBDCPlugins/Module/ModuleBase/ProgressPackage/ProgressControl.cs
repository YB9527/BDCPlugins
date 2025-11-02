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
using System.Runtime.InteropServices;
using ModuleBase.Tool;

namespace ModuleBase.ProgressPackage
{
    

    /// <summary>
    /// 进度条控制组件
    /// </summary>
    public partial class ProgressControl : UserControl
    {

        public ProgressControl()
        {
            TimeRecorder = new TimeRecorder();
            InitializeComponent();
            InitializeLogFile();
            btnCancel.Visible = false;
            btnClose.Visible = false;
            this.OnClose += ProgressControl_OnClose;
            tjLabelText = this.lblTj.Text;
            this.TjChange();
        }

        private String tjLabelText = null;
        private TimeRecorder TimeRecorder = null;
        #region 事件定义
        /// <summary>
        /// 取消事件委托
        /// </summary>
        public delegate void CancelEventHandler(object sender, EventArgs e);
        
        /// <summary>
        /// 关闭事件委托
        /// </summary>
        public delegate void CloseEventHandler(object sender, EventArgs e);

        /// <summary>
        /// 取消事件
        /// </summary>
        public event CancelEventHandler OnCancel;

        /// <summary>
        /// 关闭事件
        /// </summary>
        public event CloseEventHandler OnClose;
        #endregion

        #region 私有字段
        private ProgressDisplayMode _displayMode = ProgressDisplayMode.Count;
        private int _maximum = 100;
        private int _minimum = 0;
        private int _value = 0;
        private string _logFilePath;
        private readonly object _logLock = new object();
        private Form _hostForm;
        private bool _isStarted = false;
        #endregion

        #region 公共属性
        /// <summary>
        /// 进度条显示模式
        /// </summary>
        [Category("进度条设置")]
        [Description("进度条显示模式")]
        public ProgressDisplayMode DisplayMode
        {
            get { return _displayMode; }
            set 
            { 
                _displayMode = value;
                UpdateProgressText();
            }
        }

        /// <summary>
        /// 进度条最大值
        /// </summary>
        [Category("进度条设置")]
        [Description("进度条最大值")]
        public int Maximum
        {
            get { return _maximum; }
            set 
            { 
                _maximum = value;
                progressBar.Maximum = value;
                UpdateProgressText();
            }
        }

        /// <summary>
        /// 进度条最小值
        /// </summary>
        [Category("进度条设置")]
        [Description("进度条最小值")]
        public int Minimum
        {
            get { return _minimum; }
            set 
            { 
                _minimum = value;
                progressBar.Minimum = value;
                UpdateProgressText();
            }
        }

        /// <summary>
        /// 进度条当前值
        /// </summary>
        [Category("进度条设置")]
        [Description("进度条当前值")]
        public int Value
        {
            get { return _value; }
            set 
            { 
                _value = value;
                progressBar.Value = value;
                UpdateProgressText();
            }
        }

        /// <summary>
        /// 进度条标题
        /// </summary>
        [Category("进度条设置")]
        [Description("进度条标题")]
        public string ProgressTitle
        {
            get { return lblTitle.Text; }
            set
            {
                lblTitle.Text = value;
                InitializeLogFile();
            }
        }

        /// <summary>
        /// 是否启用日志记录
        /// </summary>
        [Category("日志设置")]
        [Description("是否启用日志记录")]
        public bool EnableLogging { get; set; } = true;
        #endregion

       

        private void ProgressControl_OnClose(object sender, EventArgs e)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // StartProgress(); // 保证控件加载时按钮状态正确 - REMOVED
        }

        #region 初始化方法
        /// <summary>
        /// 初始化日志文件路径
        /// </summary>
        private void InitializeLogFile()
        {
            try
            {
                string appPath = AppTool.GetAdministroDir();
                string tempPath = Path.Combine(appPath, "temp");
                
                // 确保temp目录存在
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                // 创建日志目录
                string logPath = Path.Combine(tempPath, "日志");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                // 生成日志文件名（按日期）
                string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + $"_log_{this.ProgressTitle}.txt";
                _logFilePath = Path.Combine(logPath, fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化日志文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 进度条控制方法
        /// <summary>
        /// 更新进度文本显示
        /// </summary>
        private void UpdateProgressText()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateProgressText));
                return;
            }

            string progressText = "";
            if (_displayMode == ProgressDisplayMode.Count)
            {
                progressText = $"{_value} / {_maximum}";
            }
            else if (_displayMode == ProgressDisplayMode.Percentage)
            {
                if (_maximum > _minimum)
                {
                    double percentage = ((double)(_value - _minimum) / (_maximum - _minimum)) * 100;
                    progressText = $"{percentage:F1}%";
                }
                else
                {
                    progressText = "0%";
                }
            }

            lblProgress.Text = progressText;
        }

        /// <summary>
        /// 重置进度条
        /// </summary>
        public void Reset()
        {
            Value = Minimum;
            ClearLog();
        }

        /// <summary>
        /// 增加进度值
        /// </summary>
        /// <param name="increment">增加的值</param>
        public void Increment(int increment = 1)
        {
            Value = Math.Min(_value + increment, _maximum);


            if (Value == _maximum)
            {
                this.btnCancel.Visible = false;
                this.btnClose.Visible = true;
            }
        }

        /// <summary>
        /// 设置进度值
        /// </summary>
        /// <param name="value">新的进度值</param>
        public void SetValue(int value)
        {
            Value = Math.Max(_minimum, Math.Min(value, _maximum));

           
        }
        #endregion

        #region 日志方法
        /// <summary>
        /// 添加日志信息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="logLevel">日志级别</param>
        public void AddLog(string message, string logLevel = "INFO")
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, string>(AddLog), message, logLevel);
                return;
            }
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"[{timestamp}] [{logLevel}] {message}";

            // 设置颜色
            Color color = Color.Black;
            if (logLevel == "ERROR") color = Color.Red;
            else if (logLevel == "WARN") color = Color.Orange;
            else if (logLevel == "INFO") color = Color.Black;

            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.SelectionLength = 0;
            txtLog.SelectionColor = color;
            txtLog.AppendText(logEntry + Environment.NewLine);
            txtLog.SelectionColor = txtLog.ForeColor; // 恢复默认
            txtLog.ScrollToCaret();

            // 写入文件
            if (EnableLogging)
            {
                WriteLogToFile(logEntry);
            }
        }

        /// <summary>
        /// 添加信息日志
        /// </summary>
        /// <param name="message">消息</param>
        public void AddInfoLog(string message)
        {
            AddLog(message, "INFO");
        }

        /// <summary>
        /// 添加警告日志
        /// </summary>
        /// <param name="message">消息</param>
        public void AddWarningLog(string message)
        {
            AddLog(message, "WARN");
        }
        private int ErrorCount { get; set; }


        public void IncrementError()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(IncrementError));
                return;
            }

            ErrorCount++;
            this.TjChange();
        }

        private int SuccessCount { get; set; }
        public void IncrementSuccess()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(IncrementSuccess));
                return;
            }

            SuccessCount++;
            this.TjChange();

        }

        public String GetTjLabelText()
        {
            if (InvokeRequired)
            {
                return (string)Invoke(new Func<string>(GetTjLabelText));
            }

            return this.lblTj.Text;
        }
        
        /// <summary>
        /// 更新统计信息显示
        /// </summary>
        private void TjChange()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(TjChange));
                return;
            }

            string text = tjLabelText.Replace("{successCount}", SuccessCount.ToString()).Replace("{failCount}", ErrorCount.ToString());
            text = text.Replace("{time}", TimeRecorder.GetRunTime());
           

           
            //求出预计要运行的时间（结果是：时分秒）
            //求出预计完成时间（结果是：日期 YYYY-MM-DD hh:mm:ss）


            //当前运行时间
            TimeSpan runTime = TimeRecorder.GetTime();

            //求出预计要运行的时间（结果是：时分秒）
            if (SuccessCount + ErrorCount > 0)
            {
                // 计算平均每个任务耗时
                double averageTimePerTask = runTime.TotalSeconds / (SuccessCount + ErrorCount);
                // 计算剩余任务数量
                int remainingTasks = _maximum - (SuccessCount + ErrorCount);
                // 计算预计剩余时间（秒）
                double estimatedRemainingSeconds = averageTimePerTask * remainingTasks;
                TimeSpan estimatedRemainingTime = TimeSpan.FromSeconds(estimatedRemainingSeconds);

                // 格式化预计剩余时间为时分秒
                string estimatedTimeString = $"{estimatedRemainingTime.Hours:D2}:{estimatedRemainingTime.Minutes:D2}:{estimatedRemainingTime.Seconds:D2}";

                // 求出预计完成时间（结果是：日期 YYYY-MM-DD hh:mm:ss）
                DateTime estimatedCompletionTime = DateTime.Now.AddSeconds(estimatedRemainingSeconds);
                string completionTimeString = estimatedCompletionTime.ToString("yyyy-MM-dd HH:mm:ss");

                // 可以在这里使用这两个变量，比如更新UI显示
                text = text.Replace("{estimatedTime}", estimatedTimeString);
                text = text.Replace("{completionTime}", completionTimeString);
            }
            else
            {
                text = text.Replace("{estimatedTime}", "-");
                text = text.Replace("{completionTime}", "-");
            }
            lblTj.Text = text;
        }
        /// <summary>
        /// 添加错误日志
        /// </summary>
        /// <param name="message">消息</param>
        public void AddErrorLog(string message)
        {
            AddLog(message, "ERROR");
        }

        /// <summary>
        /// 清除日志显示
        /// </summary>
        public void ClearLog()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ClearLog));
                return;
            }

            txtLog.Clear();
        }

        /// <summary>
        /// 写入日志到文件
        /// </summary>
        /// <param name="logEntry">日志条目</param>
        private void WriteLogToFile(string logEntry)
        {
            try
            {
                lock (_logLock)
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                // 如果写入失败，在界面上显示错误信息
                if (InvokeRequired)
                {
                    Invoke(new Action(() => AddErrorLog($"写入日志文件失败: {ex.Message}")));
                }
                else
                {
                    AddErrorLog($"写入日志文件失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 获取日志文件路径
        /// </summary>
        /// <returns>日志文件路径</returns>
        public string GetLogFilePath()
        {
            return _logFilePath;
        }
        #endregion

        #region 事件处理方法
        /// <summary>
        /// 启动进度条（弹窗显示自身，线程安全，支持多次调用）
        /// </summary>
        public void Start()
        {
            if (_isStarted) return;
            _isStarted = true;
            if (InvokeRequired)
            {
                Invoke(new Action(Start));
                return;
            }
            btnCancel.Visible = true;
            btnCancel.Enabled = true;
            _hostForm = new Form();
            _hostForm.Text = this.ProgressTitle ?? "进度条";
            _hostForm.Size = new System.Drawing.Size(1000, 600);
            _hostForm.StartPosition = FormStartPosition.CenterScreen;
            _hostForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            _hostForm.MaximizeBox = true;
            _hostForm.MinimizeBox = false;
            _hostForm.Controls.Add(this);
            this.Dock = DockStyle.Fill;
            this.OnClose += (s, e) => { _hostForm?.Close(); };
            _hostForm.Show();
            _hostForm.ControlBox = false;  // 隐藏控制框
        }

        /// <summary>
        /// 任务完成或取消后调用，自动显示关闭按钮，隐藏取消按钮
        /// </summary>
        public void FinishProgress()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(FinishProgress));
                return;
            }
            btnCancel.Visible = false;
            btnCancel.Enabled = false;
            btnClose.Visible = true;
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确定要取消当前任务吗？", "确认取消", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            AddInfoLog("用户点击了取消按钮");
            btnCancel.Enabled = false;
            btnCancel.Visible = false; // 取消后隐藏取消按钮
            btnClose.Visible = true;  // 显示关闭按钮
            FinishProgress(); // 取消后自动切换按钮（如有额外逻辑可保留）
            OnCancel?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            AddInfoLog("用户点击了关闭按钮");
            OnClose?.Invoke(this, EventArgs.Empty);
            // 不再关闭窗体
        }

        

        /*private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ProgressControl
            // 
            this.Name = "ProgressControl";
            this.Size = new System.Drawing.Size(1315, 719);
            this.ResumeLayout(false);

        }*/
        #endregion

        /*private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ProgressControl
            // 
            this.Name = "ProgressControl";
            this.Size = new System.Drawing.Size(1638, 651);
            this.ResumeLayout(false);

        }*/
    }
    /// <summary>
    /// 进度条显示模式枚举
    /// </summary>
    public enum ProgressDisplayMode
    {
        /// <summary>
        /// 数量模式
        /// </summary>
        Count,
        /// <summary>
        /// 百分比模式
        /// </summary>
        Percentage
    }

} 