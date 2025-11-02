using ModuleBase.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleBase.Component
{
    public class CheckForm
    {
        public List<ButtonEditDirSelect> ButtonEditDirSelects { get; set; }
        public List<ButtonEditFileSelect> ButtonEditFileSelects { get; set; }
        public List<ButtonEditFileSelect> CheckIsMust { get; set; }
 
        public List<MyTextEdit> MyTextEdits { get; set; }

        public CheckForm()
        {
            ButtonEditDirSelects = new List<ButtonEditDirSelect>();
            ButtonEditFileSelects = new List<ButtonEditFileSelect>();
            MyTextEdits = new List<MyTextEdit>();
            CheckIsMust = new List<ButtonEditFileSelect>();
        }
        public CheckForm AddFileInput(ButtonEditFileSelect buttonEdit1)
        {
            ButtonEditFileSelects.Add(buttonEdit1);
            return this;
        }

        public CheckForm AddDirInput(ButtonEditDirSelect buttonEditDirSelect4)
        {
            ButtonEditDirSelects.Add(buttonEditDirSelect4);
            return this;
        }

        public CheckForm AddInput(MyTextEdit textBox2)
        {
            MyTextEdits.Add(textBox2);
            return this;
        }
        public CheckForm AddCheckIsMust(ButtonEditFileSelect textBox2)
        {
            CheckIsMust.Add(textBox2);
            return this;
        }

        public bool Vaild(bool isShowErrors = true)
        {
            List<String> errros = new List<string>();
            foreach (var item in ButtonEditDirSelects)
            {
                String txt = item.Text;
                if (item.Self_IsMust)
                {
                    if (StringUtils.IsNullOrTrimEmpty(txt))
                    {
                        this.AddError(errros,"必填项没有填写！");
                    }
                }
                if (StringUtils.IsNullOrTrimEmpty(txt))
                {
                    continue;
                }
                if (!Directory.Exists(txt))
                {
                    this.AddError(errros, $"文件夹不存在：{txt}！");
                }
            }
            foreach (var item in CheckIsMust)
            {
                String txt = item.Text;
                if (item.Self_IsMust)
                {
                    if (StringUtils.IsNullOrTrimEmpty(txt))
                    {
                        this.AddError(errros, "必填项没有填写！");
                    }
                }

            }

            foreach (var item in ButtonEditFileSelects)
            {
                String txt = item.Text;
                if (item.Self_IsMust)
                {
                    if (StringUtils.IsNullOrTrimEmpty(txt))
                    {
                        this.AddError(errros, "必填项没有填写！");
                    }
                }
                if (StringUtils.IsNullOrTrimEmpty(txt))
                {
                    continue;
                }
                if (!File.Exists(txt))
                {
                    this.AddError(errros, $"文件不存在：{txt}！");
                }
            }
            foreach (var item in MyTextEdits)
            {
                String txt = item.Text;
                if (item.My_IsMust)
                {
                    if (StringUtils.IsNullOrTrimEmpty(txt))
                    {
                        this.AddError(errros, "必填项没有填写！");
                    }
                }
            }
            if(errros.Count > 0)
            {
                if (isShowErrors)
                {
                    MessageBox.Show(StringUtils.ExpandStrs(errros,",\r\n"));
                }
                return false;
            }
            return true;
        }

        private void AddError(List<string> errros, string str)
        {
            if (!errros.Contains(str))
            {
                errros.Add(str);
            }
        }
    }
}
