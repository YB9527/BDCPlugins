using ModuleBase.Tool;
using ModuleOffice.Tool;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleOffice.Page
{
    public enum FileCommand
    {
        COPY,
        MOVE,
        DELTE
    }
    public partial class FileToolPage : UserControl
    {
        public FileToolPage()
        {
            InitializeComponent();
        }

        //导出文件夹所有文件
        private void button48_Click(object sender, EventArgs e)
        {
            ExportFileName();
        }

        private static void ExportFileName()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    String dir = fbd.SelectedPath;
                    //选择保存excel位置
                    using (SaveFileDialog ofd = new SaveFileDialog())
                    {
                        ofd.Title = "保存结果Excel";
                        ofd.Filter = "Excel文件|;*.xls";
                        ofd.FileName = "文件名导出.xls";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {

                            List<String> files = FileTool.FindAllFiles(dir, "*");
                            File.Copy(AppTool.GetTemplateDirectory() + "模板-文件名称.xls", ofd.FileName, true);
                            ISheet sheet = ExcelTool.ReadSheet(ofd.FileName, 0, true);
                            JArray array = new JArray();
                            foreach (var item in files)
                            {
                                JObject obj = new JObject();

                                if (item.Contains("."))
                                {
                                    obj.Add("文件夹路径", Path.GetDirectoryName(item));
                                    obj.Add("文件名", Path.GetFileName(item));
                                }
                                else
                                {
                                    obj.Add("文件夹路径", item);
                                    obj.Add("文件名", "");
                                }

                                array.Add(obj);
                            }
                            ExcelTool.WriteRowsByHeaderRow(sheet, array, 0);
                            ExcelTool.Save(sheet.Workbook, ofd.FileName);
                            if (MessageBox.Show($"保存完成,是否打开文件修改", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Process.Start(ofd.FileName);
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 下载文件更名模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog ofd = new SaveFileDialog())
            {
                ofd.Title = "下载文件复制模板";
                ofd.Filter = "Excel文件|;*.xls";
                ofd.FileName = "文件复制.xls";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(AppTool.GetAppDirectory() + "/template/模板-文件复制.xls", ofd.FileName, true);
                    if (MessageBox.Show($"保存完成,是否打开文件修改", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(ofd.FileName);
                    }
                }
            }


        }
        
        /// <summary>
        /// 导出需要复制的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    String dir = fbd.SelectedPath;
                    //选择保存excel位置
                    using (SaveFileDialog ofd = new SaveFileDialog())
                    {
                        ofd.Title = "选这需要更名文件所在的文件夹";
                        ofd.Filter = "Excel文件|;*.xls";
                        ofd.FileName = "文件复制.xls";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {

                          
                            File.Copy(AppTool.GetAppDirectory() + "/template/模板-文件复制.xls", ofd.FileName, true);
                            ExportFileToExcel(dir, ofd.FileName);
                            
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 导出文件到excel
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="xlsPath"></param>
        public void ExportFileToExcel(String dir, String xlsPath)
        {
            List<String> files = FileTool.FindAllFiles(dir, "*");
            ISheet sheet = ExcelTool.ReadSheet(xlsPath, 0, true);
            JArray array = new JArray();
            foreach (var item in files)
            {
                JObject obj = new JObject();
                obj.Add("是否覆盖已有文件", "是");
                if (item.Contains("."))
                {
                    obj.Add("原始文件夹路径", Path.GetDirectoryName(item));
                    obj.Add("原始文件名", Path.GetFileName(item));
                }
                else
                {
                    obj.Add("原始文件夹路径", item);
                    obj.Add("原始文件名", "");
                }
                obj.Add("目标文件夹路径", "");
                obj.Add("目标文件名", "");
                array.Add(obj);
            }
            ExcelTool.WriteRowsByHeaderRow(sheet, array, 0);
            ExcelTool.Save(sheet.Workbook, xlsPath);
            if (MessageBox.Show($"保存完成,是否打开文件", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Process.Start(xlsPath);
            }
        }

        /// <summary>
        /// 文件复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button50_Click(object sender, EventArgs e)
        {
            ExFileCommand("选择复制的Excel",FileCommand.COPY);
           
        }

        private void ExFileCommand(string title, FileCommand command)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = title;
                ofd.Filter = "Excel文件|*.xlsx;*.xls";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    String path = ofd.FileName;
                    List<Dictionary<string, string>> list = ExcelTool.ReadExcelToJson(path, 0);
                    if (MessageBox.Show($"确定要执行：{list.Count} 条数据 ", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        List<String> errros = new List<string>();
                        foreach (var item in list)
                        {
                            if (StringUtils.IsNullOrTrimEmpty(item["原始文件夹路径"]))
                            {
                                //文件夹复制
                                continue;
                            }

                            String isOvrr = item["是否覆盖已有文件"];
                            bool isOvrride = StringUtils.TrimEq(isOvrr, "是");
                            if (StringUtils.IsNullOrTrimEmpty(item["原始文件名"]))
                            {
                                //文件夹复制
                                String srcFileName = item["原始文件夹路径"];
                                String destFileName = item["目标文件夹路径"];
                                if (!Directory.Exists(srcFileName))
                                {
                                    errros.Add("文件夹不存：" + srcFileName);
                                }
                                if (StringUtils.IsNullOrTrimEmpty(destFileName))
                                {
                                    errros.Add("目标文件没有填写");
                                }
                            }
                            else
                            {
                                //文件复制
                                String srcFileName = item["原始文件夹路径"] + "/" + item["原始文件名"];
                                String destFileName = item["目标文件夹路径"] + "/" + item["目标文件名"];
                                if (!File.Exists(srcFileName))
                                {
                                    errros.Add("文件不存：" + srcFileName);
                                }
                                if (StringUtils.IsNullOrTrimEmpty(destFileName))
                                {
                                    errros.Add("目标文件没有填写");
                                }
                            }
                        }

                        if (errros.Count > 0)
                        {
                            FileTool.WriteTxt(AppTool.GetTimeTxt(), errros, true);
                            if (MessageBox.Show($"数据存在问题：{errros.Count} ，是否继续执行？ ", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                if(command == FileCommand.COPY)
                                {
                                    FileAndDirCopy(list);

                                }
                                else if(command == FileCommand.MOVE)
                                {
                                    FileAndDirMove(list);
                                }
                                MessageBox.Show("执行成功");
                            }
                        }
                        else
                        {
                            if (command == FileCommand.COPY)
                            {
                                FileAndDirCopy(list);
                               
                            }
                            else if (command == FileCommand.MOVE)
                            {
                                FileAndDirMove(list);
                            }
                            MessageBox.Show("执行成功");
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 文件或文件夹移动
        /// </summary>
        /// <param name="list"></param>
        private void FileAndDirMove(List<Dictionary<string, string>> list)
        {
            //先处理文件
            foreach (var item in list)
            {
                String isOvrr = item["是否覆盖已有文件"];
                bool isOvrride = StringUtils.TrimEq(isOvrr, "是");
                if (StringUtils.IsNullOrTrimEmpty(item["原始文件名"]))
                {
                    
                }
                else
                {
                    //文件复制
                    String srcFileName = item["原始文件夹路径"] + "/" + item["原始文件名"];
                    String destFileName = item["目标文件夹路径"] + "/" + item["目标文件名"];
                    if (!File.Exists(srcFileName))
                    {

                    }
                    else if (StringUtils.IsNullOrTrimEmpty(destFileName))
                    {

                    }
                    else
                    {

                        FileTool.MoveFile(srcFileName, destFileName, isOvrride);
                    }
                }
            }
            //再处理文件夹
            foreach (var item in list)
            {
                String isOvrr = item["是否覆盖已有文件"];
                bool isOvrride = StringUtils.TrimEq(isOvrr, "是");
                if (StringUtils.IsNullOrTrimEmpty(item["原始文件名"]))
                {
                    //文件夹复制
                    String srcFileName = item["原始文件夹路径"];
                    String destFileName = item["目标文件夹路径"];
                    FileTool.MoveDir(srcFileName, destFileName, isOvrride);
                }
                else
                {
                   
                }
            }
        }
        /// <summary>
        /// 文件或文件夹复制
        /// </summary>
        /// <param name="list"></param>
        private void FileAndDirCopy(List<Dictionary<string, string>> list)
        {
            foreach (var item in list)
            {
                String isOvrr = item["是否覆盖已有文件"];
                bool isOvrride = StringUtils.TrimEq(isOvrr, "是");
                if (StringUtils.IsNullOrTrimEmpty(item["原始文件名"]))
                {
                    //文件夹复制
                    String srcFileName = item["原始文件夹路径"];
                    String destFileName = item["目标文件夹路径"];
                    if (!Directory.Exists(destFileName))
                    {
                        Directory.CreateDirectory(destFileName);
                    }
                }
                else
                {
                    //文件复制
                    String srcFileName = item["原始文件夹路径"] + "/" + item["原始文件名"];
                    String destFileName = item["目标文件夹路径"] + "/" + item["目标文件名"];
                    if (!File.Exists(srcFileName))
                    {

                    }
                    else if (StringUtils.IsNullOrTrimEmpty(destFileName))
                    {

                    }
                    else
                    {
                       
                        FileTool.CopyFile(srcFileName, destFileName, isOvrride);
                    }
                }
            }
        }

        /// <summary>
        /// 导出文件移动Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    String dir = fbd.SelectedPath;
                    //选择保存excel位置
                    using (SaveFileDialog ofd = new SaveFileDialog())
                    {
                        ofd.Title = "选这需要更名文件所在的文件夹";
                        ofd.Filter = "Excel文件|;*.xls";
                        ofd.FileName = "文件移动.xls";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {


                            File.Copy(AppTool.GetAppDirectory() + "/template/模板-文件复制.xls", ofd.FileName, true);
                            ExportFileToExcel(dir, ofd.FileName);

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行文件移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            ExFileCommand("选择移动的Excel", FileCommand.MOVE);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ExportFileName();
        }
        /// <summary>
        /// 文件删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "选择要删除文件Excel";
                ofd.Filter = "Excel文件|*.xlsx;*.xls";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    String path = ofd.FileName;
                    List<Dictionary<string, string>> list = ExcelTool.ReadExcelToJson(path, 0);
                    if (MessageBox.Show($"确定要执行删除：{list.Count} 条数据 ", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        List<String> errros = new List<string>();
                        foreach (var item in list)
                        {
                            if (StringUtils.IsNullOrTrimEmpty(item["文件夹路径"]))
                            {
                                //文件夹复制
                                continue;
                            }
                            if (StringUtils.IsNullOrTrimEmpty(item["文件名"]))
                            {
                                FileTool.DeleteDir(item["文件夹路径"]);
                            }
                            else
                            {
                                //文件复制
                                String srcFileName = item["文件夹路径"] + "/" + item["文件名"];
                                FileTool.DeleteFile(srcFileName);
                            }
                        }
                        MessageBox.Show("删除成功");

                    }
                }
            }
        }
    }
}
