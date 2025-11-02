using BDCPlugins.BDCException;
using BDCPlugins.Entity;
using BDCPlugins.Service;
using ModuleBase.ExceptionBase;
using ModuleBase.ProgressPackage;
using ModuleBase.Tool;
using ModuleOffice.Tool;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BDCPlugins.BDCSystem
{
    public class ProcessService
    {

        public delegate ResonseErrorMessage ZongDiRequestDel<T>(T t, ProgressDialog.IProgressControl work);
        public delegate String GetPriamryValue<T>(T t);

        public List<ResonseErrorMessage> ListRequest<T>(List<T> zongDiEntities, String message, ZongDiRequestDel<T> zongDiRequestDel,GetPriamryValue<T> getPriamryValue,ListRequestOption listRequestOption = null)
        {
            List<ResonseErrorMessage> resonseErrorMessages = new List<ResonseErrorMessage>();
            int successcount = 0;
            
            ProgressDialog.Show(message, zongDiEntities.Count, progress =>
            {
                var recorder2 = new TimeRecorder();
                try
                {
                    ResonseErrorMessage errorMessagedata = null;
                    if (listRequestOption != null)
                    {
                        errorMessagedata = StartBeforeAction(listRequestOption, progress);
                        if (errorMessagedata == null)
                        {
                            progress.AddInfoLog($"前置执行成功");
                        }
                        else
                        {
                            resonseErrorMessages.Add(errorMessagedata);
                            progress.AddErrorLog($"前置执行失败");
                            return;
                        }
                    }
                    
                    
                    int isWriteErrorCount = 0;
                    int index = 0;
                    foreach (var item in zongDiEntities)
                    {
                        index++;
                        String primaryVal = getPriamryValue(item) ;
                        try
                        {
                            var recorder3 = new TimeRecorder();

                            progress.AddInfoLog($"{getPriamryValue(item)},开始执行");
                            errorMessagedata = zongDiRequestDel(item, progress);
                            if (errorMessagedata == null)
                            {
                                successcount++;
                                progress.AddInfoLog($"{getPriamryValue(item)},执行成功");
                                progress.IncrementSuccess();


                            }
                            else
                            {
                                progress.IncrementError();
                                errorMessagedata.Data = item;
                                resonseErrorMessages.Add(errorMessagedata);
                                progress.AddErrorLog($"{getPriamryValue(item)},执行失败");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            throw ex;
                        }
                        finally
                        {
                            String time = recorder2.Dispose();
                            //每10个错误保存一次结果
                            if (resonseErrorMessages.Count > 0 && resonseErrorMessages.Count % 10 == 0 && isWriteErrorCount != resonseErrorMessages.Count)
                            {
                                this.WriteResonseErrorMessage(message + $",总共{zongDiEntities.Count},执行成功{successcount}条,执行失败{resonseErrorMessages.Count}条。过程错误，", resonseErrorMessages);
                                isWriteErrorCount = resonseErrorMessages.Count;
                            }
                            
                            // progress.AddInfoLog($"本宗运行时间：" + time+$"执行成功{successcount}条,执行失败{resonseErrorMessages.Count}条。");
                        }

                        progress.Increment();
                    }

                    if (listRequestOption != null)
                    {
                        if (listRequestOption.FinishActionDel != null)
                        {
                            if (!listRequestOption.FinishActionDel(progress))
                            {
                                return;
                            }
                        }
                    }
                }
                catch (ExceptionCancel ex)
                {
                    progress.AddInfoLog("用户点击了取消");
                    MessageBox.Show("用户点击了取消");
                }
                finally
                {

                    try
                    {
                        progress.AddInfoLog(progress.GetTjLabelText());
                    }
                    catch
                    {

                    }
                   
                    if (resonseErrorMessages.Count == 0)
                    {
                        MessageBox.Show(message +  ",执行成功");
                    }
                    else
                    {
                        this.WriteResonseErrorMessage(message + $",总共{zongDiEntities.Count},执行成功{successcount}条,执行失败{resonseErrorMessages.Count}条。", resonseErrorMessages);
                    }
                    
                }
            });
            return resonseErrorMessages;

        }

        internal void ListRequest<T1,T>(AbsBuLuServiceBase<T1, T>.ListRequstTask list, string title, Func<T, String> getPriamry) where T1 : RecvieEntity  // 添加泛型约束
        {
            List<ResonseErrorMessage> resonseErrorMessages = new List<ResonseErrorMessage>();
            int successcount = 0;

            ProgressDialog.Show(title, list.RequstTasks.Count, progress =>
            {
                var recorder2 = new TimeRecorder();
                try
                {
                    

                    int isWriteErrorCount = 0;
                    int index = 0;
                    foreach (var item in list.RequstTasks)
                    {
                        index++;
                  
                        try
                        {
                            var recorder3 = new TimeRecorder();
                            progress.AddInfoLog($"{item.TaskName},开始执行");
                            bool bl = item.customAction();
                            if (bl)
                            {
                                successcount++;
                                progress.AddInfoLog($"{item.TaskName},执行成功");
                                progress.IncrementSuccess();
                            }
                            else
                            {
                                progress.IncrementError();
                               
                                progress.AddErrorLog($"{item.TaskName},***************************执行失败");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            throw ex;
                        }
                        finally
                        {
                           
                        }

                        progress.Increment();
                    }
                }
                catch (ExceptionCancel ex)
                {
                    progress.AddInfoLog("用户点击了取消");
                    MessageBox.Show("用户点击了取消");
                }
                finally
                {

                    try
                    {
                        progress.AddInfoLog(progress.GetTjLabelText());
                    }
                    catch
                    {

                    }

                }
            });
        }

        private ResonseErrorMessage StartBeforeAction(ListRequestOption listRequestOption, ProgressDialog.IProgressControl progress)
        {
            if (listRequestOption != null)
            {
                if (listRequestOption.BeforeActionDel != null)
                {
                    try
                    {
                        if (!listRequestOption.BeforeActionDel(progress))
                        {
                            return null;
                        }
                    }
                    catch(ExceptionQuanJi ex)
                    {
                        ResonseErrorMessage resonseErrorMessage = new ResonseErrorMessage(ex.Response, ex.Describle,null,null,ex.Data);
                        resonseErrorMessage.GroupName = ex.GroupName;
                        return resonseErrorMessage;
                       
                    }
                }
            }
            return null;
        }

       



        /// <summary>
        /// 写入错误记录到excel中
        /// </summary>
        /// <param name="name"></param>
        /// <param name="resonseErrorMessages"></param>
        private void WriteResonseErrorMessage(string name, List<ResonseErrorMessage> resonseErrorMessages)
        {
            WriteResonseErrorMessage(name, resonseErrorMessages, true);
            WriteResonseErrorMessage(name, resonseErrorMessages,false);
           
        }
        private void WriteResonseErrorMessage(string name, List<ResonseErrorMessage> resonseErrorMessages, bool isWriteData)
        {
            if (resonseErrorMessages == null || resonseErrorMessages.Count == 0)
            {
                return;
            }

            ISheet sheet = null;
            // 1. 生成文件名
            string fileName = "";
            if (isWriteData)
            {
                fileName = AppTool.GetTimeXls(name + "_包含更多信息");
                sheet = ExcelTool.ReadSheet(AppTool.GetTemplateDirectory() + "/模板-错误情况-数据.xls", "错误记录");

            }
            else
            {
                fileName = AppTool.GetTimeXls(name);
                sheet = ExcelTool.ReadSheet(AppTool.GetTemplateDirectory() + "/模板-错误情况.xls", "错误记录");
            }
            // 3. 写表头
            List<JObject> datas = new List<JObject>();

            for (int i = 0; i < resonseErrorMessages.Count; i++)
            {
                var err = resonseErrorMessages[i];
                JObject data = new JObject();
                datas.Add(data);
                data.Add("序号",i+1);
                data.Add("响应编码Code", err.Code ?? "");
                data.Add("操作Descreble", err.Descreble ?? "");
                data.Add("消息Message", err.Message ?? "");
                data.Add("主键KeyFiled", err.KeyFiled ?? "");
                data.Add("代码KeyValue", err.KeyValue ?? "");
                data.Add("当前对象","" );
                data.Add("分类", err.GroupName ?? "");

                if (isWriteData)
                {
                    if (err.Data != null)
                    {
                        try
                        {
                            data.Add("当前对象", JObject.FromObject(err.Data).ToString());
                        }
                        catch
                        {

                        }
                    }
                }
            }
            ExcelTool.WriteRowsByHeaderRow(sheet, datas, 0);


            ExcelTool.Save(sheet.Workbook, fileName);
            if (!isWriteData)
            {
                // 6. 询问用户是否打开文件
                if (MessageBox.Show($"错误记录已导出到：\n{fileName}\n是否现在打开？", "导出完成", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Process.Start(fileName);
                }

            }
            
        }

    }
}
