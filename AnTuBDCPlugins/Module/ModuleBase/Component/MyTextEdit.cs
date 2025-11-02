using ModuleBase.Tool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBase.Component
{
    public class MyTextEdit:DevExpress.XtraEditors.TextEdit
    {
        public bool My_IsMust { get; set; }
        public String My_BindAttr { get; set; }
        private Object _My_BindData;
        public Object My_BindData
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

        private bool IsInit { get; set; }
      


        public MyTextEdit() : base()
        {
            this.EditValueChanged += new System.EventHandler(this._EditValueChanged);
            this.SetText();
            IsInit = true;

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
            if (StringUtils.IsNullOrTrimEmpty(My_BindAttr) || My_BindData == null)
            {
                return;
            }
            if (My_BindData is JObject)
            {
                ((JObject)My_BindData)[My_BindAttr] = this.Text;
            }
            else
            {
                var method = My_BindData.GetType().GetMethod("set_" + My_BindAttr);
                if (method != null)
                {
                    method.Invoke(My_BindData, new object[] { this.Text });

                }
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


        private void SetText()
        {
            if (StringUtils.IsNullOrTrimEmpty(Self_CacheTag))
            {
                return;
            }
            this.Text = AppTool.GetCacheTool().GetValue(this.Self_CacheTag);
        }

    }
}
