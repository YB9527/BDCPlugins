using AnTuBDCPlugins.Pages;
using DevExpress.XtraTab;
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

namespace AnTuBDCPlugins
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.LoadPages();
            this.SelectPage();
            this.xtraTabControl1.SelectedPageChanged += XtraTabControl1_SelectedPageChanged;
        }

        private void SelectPage()
        {
            String temp = AppTool.GetCacheTool().GetValue("主页界面tab选择");
            foreach (XtraTabPage item in this.xtraTabControl1.TabPages)
            {
                if (item.Name == temp)
                {
                    this.xtraTabControl1.SelectedTabPage = item;
                    break;
                }
            }
          
        }

        private void LoadPages()
        {
            this.xtraTabPage1.Controls.Add(new CBDBuLuPage() { Dock = DockStyle.Fill });
        }

        private void XtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if (this.xtraTabControl1.SelectedTabPage != null)
            {
                AppTool.GetCacheTool().SetValue("主页界面tab选择", this.xtraTabControl1.SelectedTabPage.Name, true);
            }

        }
    }
}
