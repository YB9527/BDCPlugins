using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBase.ProgressPackage
{
    public class ProgressControlBuilder
    {
        private readonly ProgressControl _progressControl = new ProgressControl();

        public ProgressControlBuilder SetTitle(string title)
        {
            _progressControl.ProgressTitle = title;
            
            return this;
        }
        public ProgressControlBuilder SetMaximum(int max)
        {
            _progressControl.Maximum = max;
            return this;
        }
        public ProgressControlBuilder SetMinimum(int min)
        {
            _progressControl.Minimum = min;
            return this;
        }
        public ProgressControlBuilder SetDisplayMode(ProgressDisplayMode mode)
        {
            _progressControl.DisplayMode = mode;
            return this;
        }
        public ProgressControlBuilder EnableLogging(bool enable)
        {
            _progressControl.EnableLogging = enable;
            return this;
        }
        public ProgressControlBuilder OnCancel(EventHandler handler)
        {
            _progressControl.OnCancel += (s, e) => handler(s, e);
            return this;
        }
        // 新增：设置进度值
        public ProgressControlBuilder SetValue(int value)
        {
            _progressControl.SetValue(value);
            return this;
        }
        // 新增：进度递增
        public ProgressControlBuilder Increment(int step = 1)
        {
            _progressControl.Increment(step);
            return this;
        }
        // 新增：添加日志
        public ProgressControlBuilder AddLog(string message, string level = "INFO")
        {
            _progressControl.AddLog(message, level);
            return this;
        }
        public ProgressControlBuilder AddInfoLog(string message)
        {
            _progressControl.AddInfoLog(message);
            return this;
        }
        public ProgressControlBuilder AddWarningLog(string message)
        {
            _progressControl.AddWarningLog(message);
            return this;
        }
        public ProgressControlBuilder AddErrorLog(string message)
        {
            _progressControl.AddErrorLog(message);
            return this;
        }
        public ProgressControl Build()
        {
            return _progressControl;
        }
    }
}
