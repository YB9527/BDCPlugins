using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleBase.ProgressPackage
{
    /// <summary>
    /// ProgressControl 用例示例
    /// </summary>
    public class ProgressControlExample
    {
        private ProgressControl progressControl;
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// 演示进度条的典型用法
        /// </summary>
        public void ShowProgressExample()
        {
            // 创建窗体
            Form progressForm = new Form();
            progressForm.Text = "进度条演示";
            progressForm.Size = new System.Drawing.Size(800, 800);
            progressForm.StartPosition = FormStartPosition.CenterScreen;
            progressForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            progressForm.MaximizeBox = false;
            progressForm.MinimizeBox = false;

            // 使用Builder模式创建进度条控件
            progressControl = new ProgressControlBuilder()
                .SetTitle("正在导入数据...")
                .SetMaximum(100)
                .SetMinimum(0)
                .SetDisplayMode(ProgressDisplayMode.Percentage)
                .EnableLogging(true)
                .OnCancel((s, e) => OnCancel())
                .Build();
            progressControl.Dock = DockStyle.Fill;
            progressControl.OnClose += (s, e) => progressForm.Close();

            // 添加到窗体
            progressForm.Controls.Add(progressControl);
            progressForm.Show();

            // 启动任务
            StartProcessing();
        }

        private async void StartProcessing()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            try
            {
                await Task.Run(() => SimulateProcessing(token), token);
            }
            catch (OperationCanceledException)
            {
                progressControl.AddWarningLog("用户取消了操作");
            }
            catch (Exception ex)
            {
                progressControl.AddErrorLog($"处理过程中发生错误: {ex.Message}");
            }
        }

        private void SimulateProcessing(CancellationToken token)
        {
            progressControl.AddInfoLog("开始处理数据...");
            for (int i = 0; i <= 100; i++)
            {
                token.ThrowIfCancellationRequested();
                progressControl.Invoke(new Action(() =>
                {
                    progressControl.SetValue(i);
                    progressControl.AddInfoLog($"正在处理第 {i} 项数据...");
                }));
                Thread.Sleep(50);
                if (i == 25)
                {
                    progressControl.Invoke(new Action(() =>
                    {
                        progressControl.AddWarningLog("发现数据异常，正在处理...");
                    }));
                    Thread.Sleep(200);
                }
                else if (i == 50)
                {
                    progressControl.Invoke(new Action(() =>
                    {
                        progressControl.AddInfoLog("已完成50%，正在继续处理...");
                    }));
                }
                else if (i == 75)
                {
                    progressControl.Invoke(new Action(() =>
                    {
                        progressControl.AddWarningLog("网络连接不稳定，正在重试...");
                    }));
                    Thread.Sleep(100);
                }
            }
            progressControl.Invoke(new Action(() =>
            {
                progressControl.AddInfoLog("数据处理完成！");
                progressControl.AddInfoLog($"日志文件保存在: {progressControl.GetLogFilePath()}");
                progressControl.FinishProgress();
            }));
        }

        private void OnCancel()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        }
    }
} 