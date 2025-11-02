
using BDCPlugins.BDCException;
using BDCPlugins.BDCSystem;
using BDCPlugins.Entity;
using BDCPlugins.template;
using ModuleBase.ProgressPackage;
using ModuleBase.Tool;
using ModuleOffice.Tool;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using static ModuleBase.ProgressPackage.ProgressDialog;

namespace BDCPlugins.Service
{

    public enum ExceptionHandleType
    {
        //不管
        NONE,
        //捕获处理，然后跳过
        Catch,
        CatchWrite,
    }
    public enum DataType
    {

        NONE,
        DATA,
        DATAS,
        DATAS_First,
        DATA_ROWS,
        DATA_ROWS_First,
        String,
        DATA_ROWS_First_ByLatTime,
    }

   

    public abstract class AbsBuLuServiceBase<T1,T> where T1:RecvieEntity 
    {
        public AbsBuLuServiceBase(T1 buLuDataColleciton, ConfigService configService)
        {

            this.buLuDataColleciton = buLuDataColleciton;
            this.configService = configService;
            this.projectService = new ProjectService();

        }

        public virtual void SetSlmc(T zrz)
        {

        }

        public bool AskExe(string taskName)
        {
            if (Entities.Count == 0)
            {
                MessageBox.Show("没有可以执行的数据");
                return false;
            }

            //提示用户是否继续
            if (MessageBox.Show($"确定要执行任务：总共{Entities.Count} 条数据 ，{taskName}", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                return true;
            }


            return false;
        }

        public class RequstTask
        {
            internal Action debuggAction;

            public T Data { get; set; }
            public String TaskName { get; set; }
            public String RequstName { get; set; }
            public DataType ResultDataType { get; set; }


            public String ResultFiledName { get; set; }
            public String CheckFiledNameVaild { get; set; }
            public ExceptionHandleType ExceptionHandleType { get; set; }
            public RequstTask()
            {
                ExceptionHandleType = ExceptionHandleType.NONE;
                ResultDataType = DataType.NONE;
            }

            public delegate bool CustomAction();
            public delegate bool CatchAction(Exception ex);
            public CustomAction customAction { get; set; }


            public delegate bool CatchEvent();

            public CatchAction catchEvent { get; internal set; }
            public bool Debug { get; internal set; }
            public string TimeFiled { get; internal set; }
            public Action RequestAction { get; internal set; }
            public Func<bool> ElseAction { get; internal set; }

            /// <summary>
            /// 有数据才执行
            /// </summary>
            public string CheckFiledCanEx { get; internal set; }
            public string GroupName { get; internal set; }
            public Action StartAction { get; internal set; }
            public Action FinishAction { get; internal set; }
        }

        public class ListRequstTask
        {
            public T Data { get; set; }
            public String TaskName { get; set; }
            public List<RequstTask> RequstTasks { get; set; }
            /// <summary>
            /// 检查对象是否能执行
            /// </summary>
            public string CheckFiledCanEx { get; internal set; }

            public ListRequstTask()
            {
                RequstTasks = new List<RequstTask>();
            }
        }

        public T1 buLuDataColleciton { get; set; }
        public ConfigService configService { get; set; }
        public ProjectService projectService { get; set; }
        protected String DCB_Key = "不动产权证号";
        public List<T> Entities { get;  set; }

        
        protected ResonseErrorMessage HandleExceptionQuanJi(ProgressDialog.IProgressControl gp, ExceptionQuanJi ex, T zong)
        {
            ResonseErrorMessage resonseErrorMessage = new ResonseErrorMessage(ex.Response, ex.Describle, DCB_Key, GetPrimaryVal(zong));
            resonseErrorMessage.GroupName = ex.GroupName;
            return resonseErrorMessage;
        }

        protected abstract string GetPrimaryVal(T zong);

        

        protected bool ExListRequstTask(ProgressDialog.IProgressControl gp, ListRequstTask list)
        {
            if (!StringUtils.IsNullOrTrimEmpty(list.CheckFiledCanEx))
            {
                Object val = GetValue(list.Data, list.CheckFiledCanEx);
                if (val == null)
                {
                    return true;
                }
                if (val is JArray && ((JArray)val).Count == 0)
                {
                    return true;
                }
            }
            String primaryVal = list.Data == null ? "" : GetPrimaryVal(list.Data);
            if (list.RequstTasks.Count > 1)
            {
                if(gp != null)
                {
                    gp.AddInfoLog($"{primaryVal},开始{list.TaskName}");
                }
               
            }

            foreach (var item in list.RequstTasks)
            {
                
                bool bl = ExRequstTask(gp, item);
                if (!bl)
                {
                    if (gp != null)
                    {
                        gp.AddErrorLog($"{primaryVal},失败{list.TaskName}");
                        return false;
                    }
                  
                }
            }
            if (list.RequstTasks.Count > 1)
            {
                if (gp != null)
                {
                    gp.AddInfoLog($"{primaryVal},完成{list.TaskName}");
                }

            }
            else
            {
                if (gp != null && !StringUtils.IsNullOrTrimEmpty(list.TaskName))
                {
                    gp.AddInfoLog($"{primaryVal},无可执行任务，{list.TaskName}");
                }
            }

            return true;
        }

        public void GetProcessCountMsg<T3>( ref bool isStartNode, ref string askMsg, Dictionary<T3, int> dict) where T3 : struct, Enum
        {
            MethodInfo m = null;
            if (this.Entities.Count != 0)
            {
                m = this.Entities[0].GetType().GetMethod("get_StartNodeName");
                if (m == null)
                {
                    throw new ErrorQuanJi("程序问题，没有这个熟悉：StartNodeName",null,null);
                }

            }
            foreach (var item in Entities)
            {
                String StartNodeName = m.Invoke(item,null) as String;
                if (StringUtils.IsNullOrTrimEmpty(StartNodeName))
                {
                    continue;
                }
                isStartNode = true;
                if (Enum.TryParse<T3>(StartNodeName, out T3 enumValue))
                {
                    // 转换成功，使用 enumValue
                    if (dict.ContainsKey(enumValue))
                    {
                        dict[enumValue] = ++dict[enumValue];
                    }
                }
                else
                {
                    // 转换失败
                    Console.WriteLine("字符串无法转换为枚举值");
                }
            }

            if (isStartNode)
            {
                askMsg += $"。\r\n";
                foreach (var key in dict.Keys)
                {
                    if (dict[key] > 0)
                    {
                        askMsg += $"{key.ToString()}，执行{dict[key]}个。\r\n";
                    }

                }

            }
    }
    internal void RebuildDataByErrorExcel(string tablePathError)
        {
            if (File.Exists(tablePathError))
            {
                String primarykey = "主键KeyFiled";
                String valuekey = "代码KeyValue";
                List<Dictionary<string, string>> errordatas = ExcelTool.ReadExcelToJson(tablePathError, "错误记录", 0);
                for (int i = 0; i < errordatas.Count; i++)
                {
                    if (StringUtils.IsNullOrTrimEmpty(errordatas[i][valuekey]))
                    {
                        errordatas.RemoveAt(i);
                        i--;
                    }
                }

                //提示用户是否继续
                //不需要询问
                if (true || MessageBox.Show($"当前选择了错误表：是否只执行错误表中的数据，共计 ，{errordatas.Count} 条", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if(this.Entities.Count != 0)
                    {
                        var m = this.Entities[0].GetType().GetMethod("set_StartNodeName");
                        if (m != null)
                        {
                            for (int i = 0; i < this.Entities.Count; i++)
                            {
                                var entity = this.Entities[i];
                                String val = this.GetPrimaryVal(entity);
                                bool flag = false;
                                foreach (var item in errordatas)
                                {
                                    if (val == item[valuekey])
                                    {
                                        flag = true;
                                        //获取当前流程节点
                                        try
                                        {
                                            String nodeName = item["分类"];
                                            if (nodeName != null)
                                            {
                                                nodeName = nodeName.Replace("、", "");
                                            }

                                            m.Invoke(entity, new object[] { nodeName });

                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                                if (!flag)
                                {
                                    this.Entities.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                    }
                    
                    
                }

            }
        }

       

        protected bool ExListRequstTaskJObject(ProgressDialog.IProgressControl gp, ListRequstTask list)
        {
            String primaryVal = list.Data == null ? "" : GetPrimaryVal(list.Data);
            if (list.RequstTasks.Count > 1)
            {
                if (gp != null)
                {
                    gp.AddInfoLog($"{primaryVal},开始{list.TaskName}");
                }

            }

            foreach (var item in list.RequstTasks)
            {
                bool bl = ExRequstTaskJObject(gp, item);
                if (!bl)
                {
                    if (gp != null)
                    {
                        gp.AddErrorLog($"{primaryVal},失败{list.TaskName}");
                        return false;
                    }

                }
            }
            if (list.RequstTasks.Count > 1)
            {
                if (gp != null)
                {
                    gp.AddInfoLog($"{primaryVal},完成{list.TaskName}");
                }

            }
            else
            {
                if (gp != null && !StringUtils.IsNullOrTrimEmpty(list.TaskName))
                {
                    gp.AddInfoLog($"{primaryVal},无可执行任务，{list.TaskName}");
                }
            }

            return true;
        }
        protected bool ExRequstTask(ProgressDialog.IProgressControl gp, RequstTask requstTask)
        {
           
            var temp2  = requstTask.Data;
            var zongdientity = JObject.FromObject(temp2);
            try
            {
                if (requstTask.StartAction != null)
                {
                    requstTask.StartAction();
                }
                if (requstTask.debuggAction != null)
                {
                    requstTask.debuggAction();
                }
                
                String primaryVal = GetPrimaryVal(temp2);
                String requestName = requstTask.RequstName;
                String resultFiledName = requstTask.ResultFiledName;
                //先检查是否有数据
                if (!StringUtils.IsNullOrTrimEmpty(requstTask.CheckFiledNameVaild))
                {
                    Object val = GetValue(temp2, requstTask.CheckFiledNameVaild);
                    if (val != null)
                    {
                        if(gp != null)
                        {
                            gp.AddInfoLog($"{primaryVal},已经存在数据，跳过该步骤：{requstTask.TaskName}");
                            if (requstTask.ElseAction != null)
                            {
                                return requstTask.ElseAction();
                            }
                        }
                        
                        return true;
                    }

                }
                //检查是否有数据可以执行
                if(requstTask.CheckFiledCanEx != null)
                {
                    Object val = GetValue(temp2, requstTask.CheckFiledCanEx);
                    if (val == null)
                    {
                        if (gp != null)
                        {
                            gp.AddInfoLog($"{primaryVal},不存在数据，跳过该步骤：{requstTask.TaskName}");
                            if (requstTask.ElseAction != null)
                            {
                                return requstTask.ElseAction();
                            }
                        }
                        return true;
                    }
                }
                bool resultFlag = true;
                if (gp != null)
                {
                    gp.AddInfoLog($"{primaryVal},开始{requstTask.TaskName}");
                }
               

                if (requstTask.customAction != null)
                {
                    resultFlag = requstTask.customAction();

                }
                else if (requstTask.ResultDataType == DataType.NONE)
                {
                    var result = projectService.Request(requestName, zongdientity);
                }
                else if (requstTask.ResultDataType == DataType.String)
                {
                    var result = projectService.RequestString(requestName, zongdientity);
                    this.SetValue(zongdientity, resultFiledName, result);
                }
                else if (requstTask.ResultDataType == DataType.DATA)
                {
                    var result = projectService.Request(requestName, zongdientity);
                    this.SetValue(zongdientity, resultFiledName, result);
                }
                else if (requstTask.ResultDataType == DataType.DATAS)
                {
                    var results = projectService.RequestArray(requestName, zongdientity);
                    this.SetValue(zongdientity, resultFiledName, results);
                    //zongdientity.ZongDiReponseEntity.SLXX = results;
                }
                else if (requstTask.ResultDataType == DataType.DATAS_First)
                {
                    var result = projectService.RequestArray(requestName, zongdientity);
                    JObject temp = projectService.GetFirstRow(result, requestName + "，信息存在 { count} 个无法执行操作");
                    this.SetValue(zongdientity, resultFiledName, temp);

                }
                else if (requstTask.ResultDataType == DataType.DATA_ROWS)
                {
                    JArray rows = projectService.HanleResponseDataRows(requestName, zongdientity);
                    this.SetValue(zongdientity, resultFiledName, rows);
                }
                else if (requstTask.ResultDataType == DataType.DATA_ROWS_First)
                {
                    JArray rows = projectService.HanleResponseDataRows(requestName, zongdientity);
                    JObject row = projectService.GetFirstRow(rows, requestName + "，信息存在 { count} 个无法执行操作");
                    this.SetValue(zongdientity, resultFiledName, row);
                }
                if (requstTask.ResultDataType == DataType.DATA_ROWS_First_ByLatTime)
                {
                    if(StringUtils.IsNullOrTrimEmpty(requstTask.TimeFiled))
                    {
                        throw new ExceptionQuanJi("软件不管，时间排序时，时间字段没有填写");
                    }
                    JArray rows = projectService.HanleResponseDataRows(requestName, zongdientity);
                    if(rows == null || rows.Count == 0)
                    {
                        throw new ExceptionQuanJi(requestName + "，信息存在 0 个无法执行操作", null, zongdientity);
                    }
                    JObject latTimeData = null;
                    foreach (var row in rows)
                    {
                        //移除无效数据，这里关联业务了很不好
                        if(JsonTool.GetStringValue(row as JObject, "ProInst_State") == "1010")
                        {
                            continue;
                        }
                        if(latTimeData == null)
                        {
                            latTimeData = row as JObject;
                        }else if(!CompareTime(latTimeData, row as JObject, requstTask.TimeFiled)){
                            latTimeData = row as JObject;
                        }
                        
                    }
                    
                    this.SetValue(zongdientity, resultFiledName, latTimeData);
                }
                if (gp != null)
                {
                    gp.AddInfoLog($"{primaryVal},完成{requstTask.TaskName}");
                }

                if (requstTask.FinishAction != null)
                {
                    requstTask.FinishAction();
                }
                return resultFlag;
            }
            catch (Exception ex)
            {   
                if(ex is ExceptionQuanJi)
                {
                    var exceptionQuanJi = ex as ExceptionQuanJi;
                    exceptionQuanJi.GroupName = requstTask.GroupName;
                    if (exceptionQuanJi.Response != null)
                    {
                        string msg  = JsonTool.GetStringValue(exceptionQuanJi.Response, "msg");
                        if (msg != null && msg.Contains("No route to host"))
                        {
                            //系统挂了，要一直等待运行
                            gp.AddWarningLog($"{GetPrimaryVal(temp2)},等待服务器重启，错误信息，{msg}");
                            Thread.Sleep(21005);
                            return ExRequstTask(gp, requstTask);
                        }
                        else if (!StringUtils.IsNullOrTrimEmpty(msg))
                        {
                            gp.AddErrorLog($"{GetPrimaryVal(temp2)},{msg},失败,{requstTask.TaskName}");
                        }
                    }
                }
                
                if (ex is ErrorQuanJi)
                {
                    gp.AddErrorLog($"{GetPrimaryVal(temp2)},{ex.Message},失败,{requstTask.TaskName}");
                    throw ex;
                }
                else if (requstTask.ExceptionHandleType == ExceptionHandleType.Catch)
                {
                    if (requstTask.catchEvent != null)
                    {
                        return requstTask.catchEvent(ex);
                    }
                    return true;
                }
                else if (requstTask.ExceptionHandleType == ExceptionHandleType.CatchWrite)
                {
                    if (requstTask.catchEvent != null)
                    {
                        return requstTask.catchEvent(ex);
                    }
                    return true;
                }
                else
                {
                    if(gp != null)
                    {
                        gp.AddErrorLog($"{GetPrimaryVal(temp2)},{ex.Message},失败{requstTask.TaskName}");
                    }
                  
                    throw ex;
                }
            }
            finally
            {

            }
           
        }
        

       

        
        protected bool ExRequstTaskJObject(ProgressDialog.IProgressControl gp, RequstTask requstTask)
        {
           

            var temp2 = requstTask.Data;
            var data =  temp2 as JObject;
            try
            {
               
                
               
                if(data == null && temp2 != null)
                {
                    data = JObject.FromObject(temp2);
                }
                if (requstTask.StartAction != null)
                {
                    requstTask.StartAction();
                }
                if (requstTask.debuggAction != null)
                {
                    requstTask.debuggAction();
                }
              
                String str = data.ToString();
                T str2 = requstTask.Data;
                String primaryVal = temp2 == null ? "" :GetPrimaryVal(temp2);
                String requestName = requstTask.RequstName;
                String resultFiledName = requstTask.ResultFiledName;
               
                //先检查是否有数据
                if (!StringUtils.IsNullOrTrimEmpty(requstTask.CheckFiledNameVaild))
                {
                    Object val = GetValue(temp2, requstTask.CheckFiledNameVaild);
                    if (val != null && val.ToString() != "[]"  )
                    {
                        if (gp != null)
                        {
                            gp.AddInfoLog($"{primaryVal},已经存在数据，跳过该步骤：{requstTask.TaskName}");
                            
                        }
                        if (requstTask.ElseAction != null)
                        {
                            return requstTask.ElseAction();
                        }
                        return true;
                    }
                }
               
                if (requstTask.CheckFiledCanEx != null)
                {
                    Object val = GetValue(temp2, requstTask.CheckFiledCanEx);
                    if (val == null)
                    {
                        if (gp != null)
                        {
                            gp.AddInfoLog($"{primaryVal},不存在数据，跳过该步骤：{requstTask.TaskName}");
                            if (requstTask.ElseAction != null)
                            {
                                return requstTask.ElseAction();
                            }
                        }
                        return true;
                    }
                }
                
                bool resultFlag = true;
                if (gp != null)
                {
                    gp.AddInfoLog($"{primaryVal},开始{requstTask.TaskName}");
                }
                
                if (requstTask.customAction != null)
                {
                    resultFlag = requstTask.customAction();

                }

                else if (requstTask.ResultDataType == DataType.NONE)
                {
                   
                    var result = projectService.Request(requestName, data);
                }
                else if (requstTask.ResultDataType == DataType.String)
                {
                    var result = projectService.RequestString(requestName, data);
                    this.SetValue(data, resultFiledName, result);
                }
                else if (requstTask.ResultDataType == DataType.DATA)
                {
                    var result = projectService.Request(requestName, data);
                    this.SetValue(data, resultFiledName, result);
                }
                else if (requstTask.ResultDataType == DataType.DATAS)
                {
                    var results = projectService.RequestArray(requestName, data);
                    this.SetValue(data, resultFiledName, results);
                    //zongdientity.ZongDiReponseEntity.SLXX = results;
                }
                else if (requstTask.ResultDataType == DataType.DATAS_First)
                {
                    var result = projectService.RequestArray(requestName, data);
                    JObject temp = projectService.GetFirstRow(result, requestName + "，信息存在 { count} 个无法执行操作");
                    this.SetValue(data, resultFiledName, temp);

                }
                else if (requstTask.ResultDataType == DataType.DATA_ROWS)
                {
                    JArray rows = projectService.HanleResponseDataRows(requestName, data);
                    this.SetValue(data, resultFiledName, rows);
                }
                else if (requstTask.ResultDataType == DataType.DATA_ROWS_First)
                {
                   
                    JArray rows = projectService.HanleResponseDataRows(requestName, data);
                    JObject row = projectService.GetFirstRow(rows, requestName + "，信息存在 { count} 个无法执行操作");
                    this.SetValue(data, resultFiledName, row);
                   
                }
                if (requstTask.ResultDataType == DataType.DATA_ROWS_First_ByLatTime)
                {
                    if (StringUtils.IsNullOrTrimEmpty(requstTask.TimeFiled))
                    {
                        throw new ExceptionQuanJi("软件不管，时间排序时，时间字段没有填写");
                    }
                    JArray rows = projectService.HanleResponseDataRows(requestName, data);
                    if (rows == null || rows.Count == 0)
                    {
                        throw new ExceptionQuanJi(requestName + "，信息存在 0 个无法执行操作", null, data);
                    }
                    JObject latTimeData = null;
                    foreach (var row in rows)
                    {
                        if (latTimeData == null)
                        {
                            latTimeData = row as JObject;
                        }
                        else if (!CompareTime(latTimeData, row as JObject, requstTask.TimeFiled))
                        {
                            latTimeData = row as JObject;
                        }

                    }

                    this.SetValue(data, resultFiledName, latTimeData);
                }
              
                if (gp != null)
                {
                    gp.AddInfoLog($"{primaryVal},完成{requstTask.TaskName}");
                }
           
                if (requstTask.customAction == null && JObjectTransToT(data) != null)
                {
                   
                   ReflectTool.CopyAttr(JObjectTransToT(data),requstTask.Data);
                }

                if (requstTask.FinishAction != null)
                {
                    requstTask.FinishAction();
                }
                return resultFlag;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionQuanJi)
                {
                    var exceptionQuanJi = ex as ExceptionQuanJi;
                    if (!StringUtils.IsNullOrTrimEmpty(requstTask.GroupName))
                    {
                        exceptionQuanJi.GroupName = requstTask.GroupName;
                    }
                   
                    if (exceptionQuanJi.Response != null)
                    {
                        string msg = JsonTool.GetStringValue(exceptionQuanJi.Response, "msg");
                        if (msg != null && msg.Contains("No route to host"))
                        {
                            //系统挂了，要一直等待运行
                            gp.AddWarningLog($"{GetPrimaryVal(temp2)},等待服务器重启，错误信息，{msg}");
                            Thread.Sleep(21005);
                            return ExRequstTaskJObject(gp, requstTask);
                        }
                        else if (!StringUtils.IsNullOrTrimEmpty(msg))
                        {
                            gp.AddErrorLog($"{GetPrimaryVal(temp2)},{msg},失败,{requstTask.TaskName}");
                        }
                    }
                }

                if (ex is ErrorQuanJi)
                {
                    gp.AddErrorLog($"{GetPrimaryVal(temp2)},{ex.Message},失败,{requstTask.TaskName}");
                    throw ex;
                }
                else if (requstTask.ExceptionHandleType == ExceptionHandleType.Catch)
                {
                    if (requstTask.catchEvent != null)
                    {
                        return requstTask.catchEvent(ex);
                    }
                    return true;
                }
                else if (requstTask.ExceptionHandleType == ExceptionHandleType.CatchWrite)
                {
                    if (requstTask.catchEvent != null)
                    {
                        return requstTask.catchEvent(ex);
                    }
                    return true;
                }
                else
                {
                    if(gp != null)
                    {
                        gp.AddErrorLog($"{GetPrimaryVal(temp2)},{ex.Message},失败,{requstTask.TaskName},{ex.Message}");
                    }
                    
                    throw ex;
                }
            }
            finally
            {

            }
            
           

        }

        /// <summary>
        /// 数据更新到Excel中
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="xlsPath"></param>
        /// <param name="sheetName"></param>
        /// <param name="headerRowIndex"></param>
        public void WriteToExcel(string primaryKey, List<JObject> datas,string xlsPath,String sheetName,int headerRowIndex )
        {

            if (!ExcelTool.CheckExcelWrite(xlsPath))
            {
                MessageBox.Show(StringTemplate.Instance.CheckExcelAndContinue);
            }

            ISheet sheet = ExcelTool.ReadSheet(xlsPath, sheetName);
            WriteRowsByHeaderRow(sheet, primaryKey,datas, headerRowIndex);
            ExcelTool.Save(sheet.Workbook, xlsPath);
        }

        private void WriteRowsByHeaderRow(ISheet sheet, string primaryKey, List<JObject> datas, int headerRowIndex)
        {
           IRow row = sheet.GetRow(headerRowIndex);
            foreach (ICell cell in row.Cells)
            {
                if(cell == null)
                {
                    continue;
                }
                String value = ExcelTool.GetCellValue(cell);
                String orgVlaue = value;
                if (StringUtils.IsNullOrTrimEmpty(value))
                {
                    continue;
                   
                }
                if(value.Contains("（") && value.Contains("）")){
                    foreach (JObject data in datas)
                    {
                        value = value.Replace("（", "").Replace("）", "");
                        if (data.ContainsKey(value))
                        {
                            data[orgVlaue] = data[value];
                        }
                        
                    }
                }
                if (value.Contains("(") && value.Contains(")"))
                {
                    foreach (JObject data in datas)
                    {
                        value = value.Replace("(", "").Replace(")", "");
                        if (data.ContainsKey(value))
                        {
                            data[orgVlaue] = data[value];
                        }
                    }
                }
                if (value.Contains("*"))
                {
                    foreach (JObject data in datas)
                    {
                        value = value.Replace("*", "");
                        if (data.ContainsKey(value))
                        {
                            data[orgVlaue] = data[value];
                        }
                    }
                }
            }
           ExcelTool.ReplaceCellValueByHeaderRow(sheet, primaryKey, datas, headerRowIndex);
        }

        /// <summary>
        /// 去除Excel标题头不需要的符号
        /// </summary>
        /// <param name="datas"></param>
        public void FilterHeaderSymbol(List<Dictionary<string, string>> datas)
        {

            foreach (var item in datas)
            {
                var keys = item.Keys.ToList<String>();
                foreach (var key in keys)
                {
                    var updateKey = key;
                    if (updateKey.Contains("(") && updateKey.Contains(")"))
                    {

                        updateKey = updateKey.Substring(0, updateKey.IndexOf("("));
                    }
                    else if (updateKey.Contains("（") && updateKey.Contains("）"))
                    {
                        updateKey = updateKey.Substring(0, updateKey.IndexOf("（"));
                    }
                    //去除带星号的必填文字
                    if (updateKey.Contains("*"))
                    {
                        updateKey = updateKey.Replace("*", "");
                        
                    }
                    item[updateKey] = item[key];
                }
            }

           

        }

        protected virtual T JObjectTransToT(JObject data)
        {
            return default(T);
        }

        /// <summary>
        /// 比较两个JObject中指定时间字段的值，判断data1的时间是否更晚（更大）
        /// </summary>
        /// <param name="data1">包含时间字段的JObject（latTimeData）</param>
        /// <param name="data2">对比的JObject</param>
        /// <param name="timeField">时间字段名</param>
        /// <returns>data1时间更晚返回true，否则返回false</returns>
        private bool CompareTime(JObject data1, JObject data2, string timeField)
        {
            string dateStr1 = JsonTool.GetStringValue(data1, timeField);
            string dateStr2 = JsonTool.GetStringValue(data2, timeField);

            // 任一时间为空时直接返回true（保持原逻辑）
            if (StringUtils.IsNullOrEmpyt(dateStr1) || StringUtils.IsNullOrEmpyt(dateStr2))
            {
                return true;
            }

            // 定义时间格式
            string format = "yyyy-MM-dd HH:mm:ss";

            // 解析时间字符串
            if (DateTime.TryParseExact(dateStr1, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt1) &&
                DateTime.TryParseExact(dateStr2, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt2))
            {
                // 直接比较DateTime对象
                return dt1 > dt2;
            }

            // 解析失败时保持原逻辑返回true
            return true;
        }










        /// <summary>
        /// 获取zongdientity中checkFiledNameVaild的值
        /// </summary>
        /// <param name="zongdientity"></param>
        /// <param name="checkFiledNameVaild">${ZongDiReponseEntity.SLXX}</param>
        /// <returns></returns>
        protected object GetValue(T zongdientity, string checkFiledNameVaild)
        {
            if(checkFiledNameVaild == "true")
            {
                return true;
            }
            if (checkFiledNameVaild == "false")
            {
                return null;
            }
            // 提取 ${} 中的路径
            var match = System.Text.RegularExpressions.Regex.Match(checkFiledNameVaild, @"\$\{(.+?)\}");
            if (!match.Success)
                throw new ArgumentException("checkFiledNameVaild 格式不正确，应为 ${Path.To.Property}");

            string path = match.Groups[1].Value;
            string[] parts = path.Split('.');

            object currentObj = zongdientity;

            // 遍历路径的所有部分
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];

                // 检查当前对象是否为 JObject
                if (currentObj is JObject jObject)
                {
                    // 尝试从 JObject 中获取属性
                    if (jObject.TryGetValue(part, out JToken token))
                    {
                        currentObj = token;

                        // 如果是 JValue，提取实际值
                        if (currentObj is JValue jValue)
                        {
                            currentObj = jValue.Value;
                        }
                    }
                    else
                    {
                        return null; // 属性不存在
                    }
                }
                // 检查当前对象是否为字典
                else if (currentObj is IDictionary<string, string> dictionary)
                {
                    if (dictionary.TryGetValue(part, out string value))
                    {
                        currentObj = value;
                    }
                    else
                    {
                        return null; // 键不存在
                    }
                }
                // 检查当前对象是否为列表
                else if (currentObj is System.Collections.IList list && int.TryParse(part, out int index))
                {
                    if (index >= 0 && index < list.Count)
                    {
                        currentObj = list[index];
                    }
                    else
                    {
                        return null; // 索引超出范围
                    }
                }
                else
                {
                    // 使用反射获取属性
                    var property = currentObj.GetType().GetProperty(part);
                    if (property == null)
                    {
                        // 尝试查找字段
                        var field = currentObj.GetType().GetField(part);
                        if (field == null)
                        {
                            return null; // 属性或字段不存在
                        }

                        currentObj = field.GetValue(currentObj);
                    }
                    else
                    {
                        currentObj = property.GetValue(currentObj);
                    }

                    if (currentObj == null)
                    {
                        return null; // 属性值为 null
                    }
                }
            }

            return currentObj;
        }




        /// <summary>
        /// 根据fieldName的变量部分字段名称，替换zongdientity中的属性值，fieldName存在多级，例如${ZongDiReponseEntity.SLXX}，${}中内容就是变量字段名称，ZongDiReponseEntity.SLXX 代表替换zongdientity中ZongDiReponseEntity中的SLXX
        /// </summary>
        /// <param name="zongdientity"></param>
        /// <param name="fieldName">${ZongDiReponseEntity.SLXX}</param>
        /// <param name="val">属性值</param>
        protected void SetValue(Object zongdientity, string fieldName, object val)
        {
            ValueTool.SetValue(zongdientity,fieldName,val);
        }
       
        

       

        

        public void FormatDateByContains(List<Dictionary<string, string>> dicts)
        {

            if (dicts == null)
            {
                return;
            }
            foreach (var item in dicts)
            {

                List<String> keys = item.Keys.ToList();
                foreach (var dateKey in keys)
                {
                    if (dateKey.EndsWith("-日期格式"))
                    {
                        item[dateKey] = DateTool.FormatDate(item[dateKey]);
                    }
                    else if (dateKey.EndsWith("-时间格式"))
                    {
                        item[dateKey] = DateTool.FormatDateTime(item[dateKey]);
                    }

                }
            }

        }


        protected void QueryOrCreateSl(ProgressDialog.IProgressControl gp, T zongdientity, String questNmaeCreateSlmc)
        {
            String primaryVal = GetPrimaryVal(zongdientity);
            this.ExListRequstTask(gp, new ListRequstTask()
            {
                TaskName = "查询受理",
                Data = zongdientity,
                RequstTasks = new List<RequstTask>()
                        {
                            new RequstTask()
                            {
                                Data = zongdientity,
                                TaskName = "查询受理",
                                RequstName="不动产数据运维-全量宗地-宗地数据补录-受理查询",
                                ResultDataType = DataType.DATA_ROWS_First,
                                CheckFiledNameVaild = "${ZongDiReponseEntity.SLXX}",
                                ResultFiledName = "${ZongDiReponseEntity.SLXX}",
                                ExceptionHandleType=ExceptionHandleType.Catch,
                                debuggAction = () =>
                                {

                                }
                            },

                        }
            });
            JObject temp = JObject.FromObject(zongdientity);
            if ((temp["ZongDiReponseEntity"] as JObject)["SLXX"].ToString() == "")
            {
                this.ExListRequstTask(gp, new ListRequstTask()
                {
                    TaskName = "受理",
                    Data = zongdientity,
                    RequstTasks = new List<RequstTask>()
                            {
                                new RequstTask()
                                {
                                    Data = zongdientity,
                                    TaskName = "创建受理",
                                    RequstName = questNmaeCreateSlmc,
                                    debuggAction = () =>
                                    {

                                    }
                                },
                                new RequstTask()
                                {
                                    Data = zongdientity,
                                    TaskName = "查询受理",
                                    RequstName="不动产数据运维-全量宗地-宗地数据补录-受理查询",
                                    ResultDataType = DataType.DATA_ROWS_First,
                                    CheckFiledNameVaild = "${ZongDiReponseEntity.SLXX}",
                                    ResultFiledName = "${ZongDiReponseEntity.SLXX}",
                                    ExceptionHandleType=ExceptionHandleType.Catch
                                },
                            }
                });
            }

          /* */





        }
        /// <summary>
        /// 查询补录项目
        /// </summary>

        public void QueryBuLuXm(ProgressDialog.IProgressControl gp, T zongdientity)
        {
            this.ExRequstTask(gp, new RequstTask()
            {
                Data = zongdientity,
                TaskName = "查询项目信息",
                RequstName = "不动产数据运维-宗地-宗地数据补录-项目信息查询",
                ResultDataType = DataType.DATA,
                ResultFiledName = "${ZongDiReponseEntity.Xmxx}",
            });

            this.ExRequstTask(gp, new RequstTask()
            {
                Data = zongdientity,
                TaskName = "检查是否有项目信息",
                RequstName = "不动产数据运维-宗地-宗地数据补录-项目信息简单查询",
                ResultDataType = DataType.DATA_ROWS,
                ResultFiledName = "${ZongDiReponseEntity.Common.Xmxxs}"
            });
        }
        protected void QueryOrCreateSl(ProgressDialog.IProgressControl gp, T zongdientity, String querySlmc, String createSlmc)
        {
            String primaryVal = GetPrimaryVal(zongdientity);
            this.ExListRequstTask(gp, new ListRequstTask()
            {
                TaskName = "查询受理",
                Data = zongdientity,
                RequstTasks = new List<RequstTask>()
                        {
                            new RequstTask()
                            {
                                Data = zongdientity,
                                TaskName = "查询受理",
                                RequstName = querySlmc,
                                ResultDataType = DataType.DATA_ROWS_First,
                                CheckFiledNameVaild = "${ZongDiReponseEntity.SLXX}",
                                ResultFiledName = "${ZongDiReponseEntity.SLXX}",
                                ExceptionHandleType=ExceptionHandleType.Catch,
                                debuggAction = () =>
                                {

                                }
                            },

                        }
            });
            JObject temp = JObject.FromObject(zongdientity);
            if ((temp["ZongDiReponseEntity"] as JObject)["SLXX"].ToString() == "")
            {
                this.ExListRequstTask(gp, new ListRequstTask()
                {
                    TaskName = "受理",
                    Data = zongdientity,
                    RequstTasks = new List<RequstTask>()
                            {
                                new RequstTask()
                                {
                                    Data = zongdientity,
                                    TaskName = "创建受理",
                                    RequstName = createSlmc,
                                    debuggAction = () =>
                                    {

                                    }
                                },
                                new RequstTask()
                                {
                                    Data = zongdientity,
                                    TaskName = "查询受理",
                                    RequstName=querySlmc,
                                    ResultDataType = DataType.DATA_ROWS_First,
                                    CheckFiledNameVaild = "${ZongDiReponseEntity.SLXX}",
                                    ResultFiledName = "${ZongDiReponseEntity.SLXX}",
                                    ExceptionHandleType=ExceptionHandleType.Catch
                                },
                            }
                });
            }

            this.ExRequstTask(gp, new RequstTask()
            {
                Data = zongdientity,
                TaskName = "查询项目信息",
                RequstName = "不动产数据运维-宗地-宗地数据补录-项目信息查询",
                ResultDataType = DataType.DATA,
                ResultFiledName = "${ZongDiReponseEntity.Xmxx}",
            });

            this.ExRequstTask(gp, new RequstTask()
            {
                Data = zongdientity,
                TaskName = "检查是否有项目信息",
                RequstName = "不动产数据运维-宗地-宗地数据补录-项目信息简单查询",
                ResultDataType = DataType.DATA_ROWS,
                ResultFiledName = "${ZongDiReponseEntity.Common.Xmxxs}"
            });

        }

        /// <summary>
        /// 读取数据
        /// </summary>
        public abstract void ReadDatas();

        internal List<String> UploadShpFile(IProgressControl gp ,T data,string requestName, string shpFilePath)
        {
            String fileName = Path.GetFileNameWithoutExtension(shpFilePath);
            
            var dbf = fileName + ".dbf";
            var shx = fileName + ".shx";
            var shp = fileName + ".shp";
            //检查文件是否存在
            if (!File.Exists(dbf))
            {
                throw new ExceptionQuanJi("文件不存在:" + dbf, null, JObject.FromObject(data));
            }
            if (!File.Exists(shx))
            {
                throw new ExceptionQuanJi("文件不存在:" + shx, null, JObject.FromObject(data));
            }
            if (!File.Exists(shp))
            {
                throw new ExceptionQuanJi("文件不存在:" + shp, null, JObject.FromObject(data));
            }
            ZongDiReponseEntity zongDiReponseEntity = JObject.FromObject(data)["ZongDiReponseEntity"].ToObject<ZongDiReponseEntity>();
            this.ExListRequstTask(gp, new ListRequstTask()
            {
                RequstTasks = new List<RequstTask>()
                {
                    new RequstTask()
                    {
                        TaskName = "上传dbf文件",
                        Data = data,
                        RequstName = requestName,
                        ResultDataType = DataType.String,
                        ResultFiledName = "${ZongDiReponseEntity.Common._dbfid}",
                        StartAction = () =>
                        {
                            zongDiReponseEntity.Common["_filePathTemp"] = dbf;
                        }
                    },
                    new RequstTask()
                    {
                        TaskName = "上传shx文件",
                        Data = data,
                        RequstName = requestName,
                        ResultDataType = DataType.String,
                        ResultFiledName = "${ZongDiReponseEntity.Common._shxid}",
                        StartAction = () =>
                        {
                            zongDiReponseEntity.Common["_filePathTemp"] = shx;
                        }
                    },
                    new RequstTask()
                    {
                        TaskName = "上传shp文件",
                        Data = data,
                        RequstName = requestName,
                        ResultDataType = DataType.String,
                        ResultFiledName = "${ZongDiReponseEntity.Common._shpid}",
                        StartAction = () =>
                        {
                            zongDiReponseEntity.Common["_filePathTemp"] = shp;
                        }
                    }
                }
            }) ;
            JArray ids = new JArray()
            {
                JsonTool.GetStringValue(zongDiReponseEntity.Common,"_dbfid"),
                JsonTool.GetStringValue(zongDiReponseEntity.Common,"_shxid"),
                JsonTool.GetStringValue(zongDiReponseEntity.Common,"_shpid"),
            };
            zongDiReponseEntity.Common["ListFileID"] = ids;
            return ids.ToObject<List<String>>();
        }

        protected String UploadDwgFile(ProgressDialog.IProgressControl gp, T data, string requestName, string dwgPath)
        {
            if (!dwgPath.EndsWith(".dwg", StringComparison.OrdinalIgnoreCase)){
                dwgPath += ".dwg";
            }
           
            //检查文件是否存在
            if (!File.Exists(dwgPath))
            {
                throw new ExceptionQuanJi("文件不存在:" + dwgPath, null, JObject.FromObject(data));
            }
           
            ZongDiReponseEntity zongDiReponseEntity = JObject.FromObject(data)["ZongDiReponseEntity"].ToObject<ZongDiReponseEntity>();
            this.ExListRequstTask(gp, new ListRequstTask()
            {
                RequstTasks = new List<RequstTask>()
                {
                    new RequstTask()
                    {
                        TaskName = "上传dwg文件",
                        Data = data,
                        RequstName = requestName,
                        ResultDataType = DataType.String,
                        ResultFiledName = "${ZongDiReponseEntity.Common._dwgid}",
                        StartAction = () =>
                        {
                            zongDiReponseEntity.Common["_filePathTemp"] = dwgPath;
                        }
                    },
                }
            });
            JArray ids = new JArray()
            {
                JsonTool.GetStringValue(zongDiReponseEntity.Common,"_dwgid"),
            };
            zongDiReponseEntity.Common["ListFileID"] = ids;
            return JsonTool.GetStringValue(zongDiReponseEntity.Common, "_dwgid");
        }
       

        /// <summary>
        /// 修改主体数据
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="data"></param>
        /// <param name="refreshDataEntity"></param>
        internal void RefreshMainData(IProgressControl gp, T data, RefreshMainDataEntity refreshDataEntity)
        {
            //开始更新地块信息
            this.ExListRequstTask(gp, new ListRequstTask()
            {
                TaskName = refreshDataEntity.TaskName,
                Data = data,
                CheckFiledCanEx = refreshDataEntity.Data != null ? "true" : "false",
                RequstTasks = new List<RequstTask>()
                {
                    //查询详情
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = refreshDataEntity.DetailsTaskName,
                        RequstName = refreshDataEntity.RequestDetailsName,
                        ResultDataType = DataType.DATA,
                        ResultFiledName = refreshDataEntity.DbDetailsFieldName,
                        CheckFiledNameVaild = refreshDataEntity.CheckDbDetailsFieldNameVaild,
                        FinishAction = () =>
                        {
                            //查询详情之后回调事件
                            refreshDataEntity.RequestDetailsFinishAction();
                        }
                    },
                    //修改数据
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = refreshDataEntity.UpdateTaskName,
                        RequstName = refreshDataEntity.RequestUpdateName,
                        ResultDataType = DataType.DATA,
                        ResultFiledName = refreshDataEntity.UpdateFieldName,
                        debuggAction = () =>
                        {

                        }
                    },
                }
            });
        }


        /// <summary>
        /// 列表第一个数据更新
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="data"></param>
        /// <param name="refreshListEntity"></param>
        internal void RefreshListData(ProgressDialog.IProgressControl gp, T data, RefreshListEntity refreshListEntity)
        {
            this.ExListRequstTask(gp, new ListRequstTask()
            {
                TaskName = refreshListEntity.TaskName,
                Data = data,
                CheckFiledCanEx = refreshListEntity.Datas != null && refreshListEntity.Datas.Count > 0 ? "true" : "false",
                RequstTasks = new List<RequstTask>()
                {
                    //查找列表
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = refreshListEntity.ListTaskName,
                        RequstName = refreshListEntity.DbDataFieldName,
                        ResultDataType = DataType.DATA_ROWS_First,
                        ResultFiledName =refreshListEntity.ListFieldName,
                        ExceptionHandleType = ExceptionHandleType.Catch,
                        debuggAction = () =>
                        {

                        }
                    },
                    //查找详情
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = refreshListEntity.DetailsTaskName,
                        RequstName = refreshListEntity.RequestDetailsName,
                        CheckFiledCanEx = refreshListEntity.ListFieldName,
                        ResultDataType = DataType.DATA,
                        ResultFiledName = refreshListEntity.DbDetailsFieldName,
                        ElseAction = () =>
                        {
                            //新增数据
                            return this.ExRequstTask(gp,new RequstTask()
                            {
                                Data = data,
                                TaskName = refreshListEntity.NewTaskName,
                                RequstName = refreshListEntity.RequestNewName,
                            });
                        }
                    },
                    //修改数据
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = refreshListEntity.UpdateTaskName,
                        RequstName = refreshListEntity.RequestUpdateName,
                        CheckFiledCanEx = refreshListEntity.DbDetailsFieldName,
                    },

                }
            });


        }

        /// <summary>
        /// 列表更新
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="data"></param>
        /// <param name="refreshListEntity"></param>
        internal void RefreshList(ProgressDialog.IProgressControl gp, T data, RefreshListEntity refreshListEntity)
        {
            //开始更新地块信息
            this.ExListRequstTask(gp, new ListRequstTask()
            {
                TaskName = refreshListEntity.TaskName,
                Data = data,
                CheckFiledCanEx = refreshListEntity.Datas != null && refreshListEntity.Datas.Count > 0 ? "true" : "false",
                RequstTasks = new List<RequstTask>()
                {
                    new RequstTask()
                    {
                        Data = data,
                        TaskName =refreshListEntity.ListTaskName,
                        RequstName = refreshListEntity.RequestListName,
                        ResultDataType = DataType.DATA_ROWS,
                        ResultFiledName = refreshListEntity.ListFieldName,
                        debuggAction = () =>
                        {

                        }
                    }
                }
            });
            JArray db_list = GetValue(data, refreshListEntity.ListFieldName) as JArray;
            //删除
            foreach (var db in db_list)
            {
                bool has = false;
                foreach (var item in refreshListEntity.Datas)
                {
                    if (refreshListEntity.IsEqAction(item as JObject, db as JObject))
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    SetValue(data, refreshListEntity.DeleteFieldName, db);
                    //在表中没有找到数据就删除
                    this.ExRequstTask(gp, new RequstTask()
                    {
                        Data = data,
                        TaskName = refreshListEntity.DeleteTaskName,
                        RequstName = refreshListEntity.RequestListName,
                    });
                }
            }
            //新增、//修改
            foreach (var item in refreshListEntity.Datas)
            {
                SetValue(data, refreshListEntity.Temp_TBDataFieldName, item);
                bool has = false;
                foreach (var db in db_list)
                {
                    if (refreshListEntity.IsEqAction(item as JObject, db as JObject))
                    {
                        SetValue(data, refreshListEntity.Temp_DBDataFieldName, item);
                        has = true;
                        break;
                    }
                }
                if (has)
                {
                    //有数据就是修改
                    this.ExListRequstTask(gp, new ListRequstTask()
                    {
                        RequstTasks = new List<RequstTask>()
                        {
                            //查询详情
                            new RequstTask()
                            {
                                Data = data,
                                TaskName = refreshListEntity.DetailsTaskName,
                                RequstName = refreshListEntity.RequestDetailsName,
                                ResultDataType = DataType.DATA,
                                ResultFiledName = refreshListEntity.DbDetailsFieldName,

                            },
                            //修改
                            new RequstTask()
                            {
                                Data = data,
                                TaskName = refreshListEntity.UpdateTaskName,
                                RequstName = refreshListEntity.RequestUpdateName,
                            }
                        }
                    });
                }
                else
                {
                    //没有有数据就是新增
                    this.ExListRequstTask(gp, new ListRequstTask()
                    {
                        RequstTasks = new List<RequstTask>()
                        {
                            //新增
                            new RequstTask()
                            {
                                Data = data,
                                TaskName = refreshListEntity.NewTaskName,
                                RequstName = refreshListEntity.RequestNewName,
                            },
                        }
                    });
                }
            }


        }


        public Dictionary<String, String> SearchQXDM(T temp,String requestNameCreateSlmc )
        {
            Dictionary<String, String> xzqdmDict = new Dictionary<string, String>();
            try
            {
                //找到代码
                this.ExListRequstTask(null, new ListRequstTask()
                {

                    TaskName = "查询区县代码",
                    Data = temp,
                    RequstTasks = new List<RequstTask>()
                    {
                        new RequstTask()
                        {
                            Data = temp,
                            customAction = () =>
                            {
                                QueryOrCreateSl(null, temp, requestNameCreateSlmc);
                                return true;
                            }
                        },
                        new RequstTask()
                        {
                            Data = temp,
                            RequstName = "查找行政区代码",
                            ResultDataType = DataType.DATA,
                            ResultFiledName = "${ZongDiReponseEntity.Common.XZQ}",
                            debuggAction = () =>
                            {

                            }
                        },
                        new RequstTask()
                        {
                            Data = temp,
                            RequstName = "查询地籍区、区县数据",
                            ResultDataType = DataType.DATA,
                            ResultFiledName = "${ZongDiReponseEntity.Common.DJQOfQX}",
                            debuggAction = () =>
                            {

                            }
                        },
                        new RequstTask()
                        {
                            Data = temp,
                            RequstName = "查询地籍子区、地籍区数据",
                            ResultDataType = DataType.DATA,
                            ResultFiledName = "${ZongDiReponseEntity.Common.DJZQOfDJQ}",
                        },
                       
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务繁忙:" + ex.Message);
                return null;
            }
            return xzqdmDict;
        }

        public void ReplaceDM(List<JObject> setDatas, Dictionary<string, string> xzqdmDict)
        {
            List<String> errors = new List<string>();
            String[] strs = new String[] { "行政区", "地籍区", "地籍子区" };
            foreach (var setData in setDatas)
            {
                foreach (var str in strs)
                {
                    String name = JsonTool.GetStringValue(setData, str);
                    setData[str + "名称"] = name;
                    if (xzqdmDict.TryGetValue(name, out string val))
                    {
                        setData[str + "代码"] = val;
                    }
                    else
                    {
                        errors.Add($"【{str}】,代码未找到，查看是否填写错误:" + name);

                    }
                }
            }
           
            if(errors.Count > 0)
            {

                FileTool.WriteTxt(AppTool.GetTime("代码替换错误.txt"), errors, true);
                
                throw new ExceptionQuanJi("行政区、地籍区、地籍子区代码替换错误，共计{errors.Count}个。");
            }
        }

        public void FilterData(List<Dictionary<string, string>> zds, string primaryKey)
        {
            for (int i = 0; i < zds.Count; i++)
            {
                if (!zds[i].ContainsKey(primaryKey) || StringUtils.IsNullOrTrimEmpty(zds[i][primaryKey]))
                {
                    zds.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
