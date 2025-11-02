using System;
using System.Threading.Tasks;
using ModuleBase.ExceptionBase;
using ModuleBase.ProgressPackage;


namespace ModuleBase.ProgressPackage
{
    public static class ProgressDialog
    {
        public static void Show(string title, int max, Action<IProgressControl> work,Action finish= null)
        {
            ProgressControl progress = new ProgressControlBuilder()
                .SetTitle($"正在{title}...")
                .SetMaximum(max)
                .SetMinimum(0)
                .SetDisplayMode(ProgressDisplayMode.Count)
                .EnableLogging(true)
                .Build();
            progress.Start();
            progress.AddInfoLog($"正在{title}...");

            Task.Run(() =>
            {
                var safeProgress = new SafeProgressControl(progress, title);
                try
                {
                    work(safeProgress);
                    // 任务正常完成
                    safeProgress.SetTitle($"完成{title}");
                    safeProgress.AddInfoLog($"完成{title}");
                }
                catch (ExceptionCancel)
                {
                    // 任务被取消
                    safeProgress.SetTitle($"取消{title}");
                    safeProgress.AddInfoLog($"取消{title}");
                }
                finally
                {
                    if(finish != null)
                    {
                        finish();
                    }
                }
            });
        }

        // 线程安全包装接口
        public interface IProgressControl
        {
            void Increment(int step = 1);
            void AddInfoLog(string msg);
            void AddErrorLog(string msg);
            void AddWarningLog(string msg);
            void SetValue(int value);
            void SetTitle(string title); // 新增：设置进度条标题
            void CheckCancel(); // 新增：检查是否取消
            void IncrementSuccess();
            void IncrementError();
            string GetTjLabelText();
        }

        private class SafeProgressControl : IProgressControl
        {
            private readonly ProgressControl _inner;
            private volatile bool _isCanceled = false;
            private readonly string _title;
            public SafeProgressControl(ProgressControl inner, string title = null)
            {
                _inner = inner;
                _title = title;
                // 订阅取消事件
                _inner.OnCancel += (s, e) => { _isCanceled = true; };
            }

            private bool IsControlAlive()
            {
                return _inner != null && !_inner.IsDisposed && _inner.IsHandleCreated;
            }

            public void CheckCancel()
            {
                if (_isCanceled)
                    throw new ExceptionCancel();
            }

            public void Increment(int step = 1)
            {
                CheckCancel();
                try
                {
                    if (!IsControlAlive()) return;
                    if (_inner.InvokeRequired)
                        _inner.Invoke(new Action(() => { try { if (IsControlAlive()) { CheckCancel(); _inner.Increment(step); } } catch (ObjectDisposedException) { } }));
                    else
                        _inner.Increment(step);
                   
                }
                catch (ObjectDisposedException) { }
            }
            public void AddInfoLog(string msg)
            {
                CheckCancel();
                try
                {
                    if (!IsControlAlive()) return;
                    if (_inner.InvokeRequired)
                        _inner.Invoke(new Action(() => { try { if (IsControlAlive()) { CheckCancel(); _inner.AddInfoLog(msg); } } catch (ObjectDisposedException) { } }));
                    else
                        _inner.AddInfoLog(msg);
                }
                catch (ObjectDisposedException) { }
            }
            public void AddErrorLog(string msg)
            {

                CheckCancel();
                try
                {
                    if (!IsControlAlive()) return;
                    if (_inner.InvokeRequired)
                        _inner.Invoke(new Action(() => { try { if (IsControlAlive()) { CheckCancel(); _inner.AddErrorLog(msg); } } catch (ObjectDisposedException) { } }));
                    else
                        _inner.AddErrorLog(msg);
                }
                catch (ObjectDisposedException) { }
            }
            public void AddWarningLog(string msg)
            {
                CheckCancel();
                try
                {
                    if (!IsControlAlive()) return;
                    if (_inner.InvokeRequired)
                        _inner.Invoke(new Action(() => { try { if (IsControlAlive()) { CheckCancel(); _inner.AddWarningLog(msg); } } catch (ObjectDisposedException) { } }));
                    else
                        _inner.AddWarningLog(msg);
                }
                catch (ObjectDisposedException) { }
            }
            public void SetValue(int value)
            {
                CheckCancel();
                try
                {
                    if (!IsControlAlive()) return;
                    if (_inner.InvokeRequired)
                        _inner.Invoke(new Action(() => { try { if (IsControlAlive()) { CheckCancel(); _inner.SetValue(value); } } catch (ObjectDisposedException) { } }));
                    else
                        _inner.SetValue(value);
                }
                catch (ObjectDisposedException) { }
            }
            public void SetTitle(string title)
            {
                try
                {
                    if (!IsControlAlive()) return;
                    if (_inner.InvokeRequired)
                        _inner.Invoke(new Action(() =>
                        {
                            if (IsControlAlive())
                            {
                                // _inner.ProgressTitle = title; // 不再设置控件标题
                                var form = _inner.FindForm();
                                if (form != null) form.Text = title;
                            }
                        }));
                    else
                    {
                        // _inner.ProgressTitle = title; // 不再设置控件标题
                        var form = _inner.FindForm();
                        if (form != null) form.Text = title;
                    }
                }
                catch (ObjectDisposedException) { }
            }

            public void IncrementSuccess()
            {
                CheckCancel();
                try
                {
                    if (!IsControlAlive()) return;
                    if (_inner.InvokeRequired)
                        _inner.Invoke(new Action(() => { try { if (IsControlAlive()) { CheckCancel(); _inner.IncrementSuccess(); } } catch (ObjectDisposedException) { } }));
                    else
                        _inner.IncrementSuccess();
                }
                catch (ObjectDisposedException) { }
            }

            public void IncrementError()
            {
                CheckCancel();
                try
                {
                    if (!IsControlAlive()) return;
                    if (_inner.InvokeRequired)
                        _inner.Invoke(new Action(() => { try { if (IsControlAlive()) { CheckCancel(); _inner.IncrementError(); } } catch (ObjectDisposedException) { } }));
                    else
                        _inner.IncrementError();
                }
                catch (ObjectDisposedException) { }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetTjLabelText()
            {
                CheckCancel();
                try
                {
                    if (!IsControlAlive()) return "";
                    if (_inner.InvokeRequired)
                        _inner.Invoke(new Action(() => { try { if (IsControlAlive()) { CheckCancel();  _inner.GetTjLabelText(); } } catch (ObjectDisposedException) { } }));
                    else
                      return  _inner.GetTjLabelText();
                }
                catch (ObjectDisposedException) { }
                return _inner.GetTjLabelText();
            }
        }
    }
} 