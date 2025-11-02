using AnTuBDCPlugins.Pages.CBDBuLu.Entity;
using AnTuBDCPlugins.Pages.CBDBuLu.Service;
using BDCPlugins.BDCSystem;
using ModuleBase.Component;
using ModuleBase.Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnTuBDCPlugins.Pages
{
    public partial class CBDBuLuPage : UserControl
    {
        private CheckForm CheckForm { get; set; }
        private ChengBaoDiDataCollection dataCollection { get; set; }
        public CBDWHService serviceTemp { get; private set; }
        public CBDBuLuPage()
        {
            InitializeComponent();
            dataCollection = new ChengBaoDiDataCollection();
            serviceTemp = new CBDWHService(dataCollection, ConfigService.ReadConfig());
        }
        /// <summary>
        /// 构建数据
        /// </summary>
        private void ReBuildDataCollection()
        {
            dataCollection.CBDWHTablePath = textBox10.Text;
            dataCollection.CBDWHTuDir = textBox11.Text;
            dataCollection.TablePathError = buttonEditFileSelect1_error.Text;
            serviceTemp.buLuDataColleciton = dataCollection;

        }
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.ReBuildDataCollection();
            CheckForm = new CheckForm();
            CheckForm.AddFileInput(textBox10);
            CheckForm.AddFileInput(buttonEditFileSelect1_error);
            CheckForm.AddDirInput(textBox11);
            //CheckForm.AddInput(textBox2);
            bool bl = CheckForm.Vaild();
            if (!bl)
            {
                return;
            }

            //读取数据
            serviceTemp.ReadDatas();
            if (!serviceTemp.AskExe("承包地维护"))
            {
                return;
            }
            var service = JsonTool.Copy<CBDWHService>(serviceTemp);
            service.Cbdwh();
        }
    }
}
