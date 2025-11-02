using ModuleBase.Tool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleBase.Component
{
    public class ButtonEditDirSelect : DevExpress.XtraEditors.ButtonEdit
    {
        public bool Self_IsMust { get; set; }
        public String Slef_BindAttr { get; set; }
        private Object _My_BindData;
        public Object Self_BindData
        {
            get
            {
                return _My_BindData;
            }
            set
            {
                _My_BindData = value;
                this.BindData();
               
            }
        }

        private string _cacheTag;

        public string Self_CacheTag
        {
            get
            {
                return _cacheTag;
            }
            set
            {
                _cacheTag = value;
                SetText();
            }
        }

        private bool IsInit { get; set; }
        public ButtonEditDirSelect():base()
        {
            this.EditValueChanged += new System.EventHandler(this._EditValueChanged);
            this.ButtonClick += _ButtonClick;
            
            this.SetText();
            IsInit = true;
        }

        private void SetText()
        {
            if (StringUtils.IsNullOrTrimEmpty(Self_CacheTag))
            {
                return;
            }
            this.Text = AppTool.GetCacheTool().GetValue(this.Self_CacheTag);
        }

        private void _ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //打开文件文件夹
            if (Directory.Exists(Text))
            {
                Process.Start("explorer.exe", Text);
            }
        }

        private void _EditValueChanged(object sender, EventArgs e)
        {
            this.BindData();
            if (!IsInit)
            {
                return;
            }
            //记录文件位置
            if (StringUtils.IsNullOrTrimEmpty(Self_CacheTag))
            {
                return;
            }
            AppTool.GetCacheTool().SetValue(Self_CacheTag, Text, true);
            

        }

        private void BindData()
        {
            if (StringUtils.IsNullOrTrimEmpty(Slef_BindAttr) || Self_BindData == null)
            {
                return;
            }
            if (Self_BindData is JObject)
            {
                ((JObject)Self_BindData)[Slef_BindAttr] = this.Text;
            }
            else
            {
                var method = Self_BindData.GetType().GetMethod("set_" + Slef_BindAttr);
                if (method != null)
                {
                    method.Invoke(Self_BindData, new object[] { this.Text });

                }
                else
                {
                    throw new Exception("开发问题，配置！");
                }
            }
        }
    }

   
}
