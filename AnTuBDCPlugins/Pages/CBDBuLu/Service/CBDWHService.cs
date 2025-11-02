
using AnTuBDCPlugins.Pages.CBDBuLu.Entity;
using BDCPlugins.BDCException;
using BDCPlugins.BDCSystem;
using BDCPlugins.Entity;
using BDCPlugins.Service;
using ModuleBase.Entity;
using ModuleBase.ProgressPackage;
using ModuleBase.Tool;
using ModuleOffice.Tool;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ModuleBase.ProgressPackage.ProgressDialog;

namespace AnTuBDCPlugins.Pages.CBDBuLu.Service
{


    public class CBDWHService : AbsBuLuServiceBase<ChengBaoDiDataCollection, CBDWHSLEntity>
    {

        public String PrimaryKey = "受理唯一号";
        public String DBLogID = "数据库记录ID";
        public String DKIDKey = "地块ID";
        private String SheetName_JCXX = "基本信息";
        private String SheetName_JCXX_Dict = "基本信息字典";
        public CBDWHService(ChengBaoDiDataCollection chengBaoDiDataCollection, ConfigService configService) : base(chengBaoDiDataCollection, configService)
        {

        }

        public override void ReadDatas()
        {
            List<String> errors = new List<string>();
            var entities = new List<CBDWHSLEntity>();
            this.Entities = entities;
            List<Dictionary<string, string>> zds = ExcelTool.ReadExcelToJson(buLuDataColleciton.CBDWHTablePath, SheetName_JCXX, 1);
            FilterHeaderSymbol(zds);
            FilterData(zds, "地块ID");
            errors.AddRange(RecvieEntity.ReplaceDict(zds, buLuDataColleciton.CBDWHTablePath, SheetName_JCXX_Dict, 1));
            FormatDateByContains(zds);

            List<Dictionary<string, string>> tdyts = ExcelTool.ReadExcelToJson(buLuDataColleciton.CBDWHTablePath, "土地用途", 1);
            FilterHeaderSymbol(tdyts);
            FilterData(tdyts, "地块ID");
            errors.AddRange(RecvieEntity.ReplaceDict(tdyts, buLuDataColleciton.CBDWHTablePath, "土地用途字典", 1));
            FormatDateByContains(tdyts);

            List<Dictionary<string, string>> tdcbxxs = ExcelTool.ReadExcelToJson(buLuDataColleciton.CBDWHTablePath, "土地承包信息", 1);
            FilterHeaderSymbol(tdcbxxs);
            FilterData(tdcbxxs, "地块ID");
            errors.AddRange(RecvieEntity.ReplaceDict(tdcbxxs, buLuDataColleciton.CBDWHTablePath, "土地承包信息字典", 1));
            FormatDateByContains(tdcbxxs);

            List<Dictionary<string, string>> tdcbjyqs = ExcelTool.ReadExcelToJson(buLuDataColleciton.CBDWHTablePath, "土地承包经营权", 1);
            FilterHeaderSymbol(tdcbjyqs);
            FilterData(tdcbjyqs, "地块ID");
            errors.AddRange(RecvieEntity.ReplaceDict(tdcbjyqs, buLuDataColleciton.CBDWHTablePath, "土地承包经营权字典", 1));
            FormatDateByContains(tdcbjyqs);

            List<Dictionary<string, string>> tdcbjyqrs = ExcelTool.ReadExcelToJson(buLuDataColleciton.CBDWHTablePath, "土地承包经营权人", 1);
            FilterHeaderSymbol(tdcbjyqrs);
            FilterData(tdcbjyqrs, "地块ID");
            errors.AddRange(RecvieEntity.ReplaceDict(tdcbjyqrs, buLuDataColleciton.CBDWHTablePath, "土地承包经营权人字典", 1));
            FormatDateByContains(tdcbjyqrs);

            if (errors != null && errors.Count > 0)
            {
                FileTool.WriteTxt(AppTool.GetTimeTxt(), errors, true,true);
                //throw new ExceptionQuanJi("字典转换错误！");
            }
            
            foreach (var item in zds)
            {
                CBDWHSLEntity entity = this.FindOrCreateEntity(entities, item);
                if (entity == null)
                {
                    continue;
                }
               
                var cBDWHEntity = new CBDWHEntity();
                entity.CBDWHEntities.Add(cBDWHEntity);

                var jcxxObj = JObject.FromObject(item);
                cBDWHEntity.Jcxx = jcxxObj;

                if (item.TryGetValue("是否检查压盖(是，否)", out string res))
                {
                    //设置是否覆盖面积
                    if (res != null && res.Trim() == "否")
                    {
                        cBDWHEntity.IsCheckShp = false;
                    }
                }
                else
                {
                    cBDWHEntity.IsCheckShp = true;
                }
            }
            /*
            //配置土地用途
            foreach (var item in tdyts)
            {
                CBDWHEntity cbd =  this.FindOrCreateCbd(entities, item);
                if(cbd == null)
                {
                    throw new ExceptionQuanJi("土地用途sheet中数据冗余，在‘基础信息’中，没有找到这个zdid=" + item[this.DKIDKey]);
                }
                else
                {
                    cbd.Tdyt = JObject.FromObject(item);
                }
            }
            //配置土地承包信息
            foreach (var item in tdcbxxs)
            {
                CBDWHEntity cbd = this.FindOrCreateCbd(entities, item);
                if (cbd == null)
                {
                    throw new ExceptionQuanJi("土地用途sheet中数据冗余，在‘土地承包信息’中，没有找到这个zdid=" + item[this.DKIDKey]);
                }
                else
                {
                    cbd.Tdcbxx = JObject.FromObject(item);
                }
            }
            //配置土地承包经营权
            foreach (var item in tdcbjyqs)
            {
                CBDWHEntity cbd = this.FindOrCreateCbd(entities, item);
                if (cbd == null)
                {
                    throw new ExceptionQuanJi("土地用途sheet中数据冗余，在‘土地承包经营权’中，没有找到这个zdid=" + item[this.DKIDKey]);
                }
                else
                {
                    cbd.Tdcbjyq = JObject.FromObject(item);
                }
            }
            //配置土地承包经营权人
            foreach (var item in tdcbjyqrs)
            {
                CBDWHEntity cbd = this.FindOrCreateCbd(entities, item);
                if (cbd == null)
                {
                    throw new ExceptionQuanJi("土地用途sheet中数据冗余，在‘土地承包经营权’中，没有找到这个zdid=" + item[this.DKIDKey]);
                }
                else
                {
                    cbd.Tdcbjyqrs.Add(JObject.FromObject(item));
                }
            }*/
           
            base.RebuildDataByErrorExcel(buLuDataColleciton.TablePathError);
            

            foreach (var item in entities)
            {
                item.ReceiveUser = this.buLuDataColleciton.ReceiveUser;
            }


        }

        

        private CBDWHSLEntity FindOrCreateEntity(List<CBDWHSLEntity> entities, Dictionary<string, string> item)
        {
            CBDWHSLEntity entity = new CBDWHSLEntity();
            String slmc = item[this.PrimaryKey];
            String dkid = item[this.DKIDKey];
            if (StringUtils.IsNullOrTrimEmpty(slmc) || StringUtils.IsNullOrTrimEmpty(dkid))
            {
                return null;
            }
            foreach (var data in entities)
            {
                if (StringUtils.TrimEq(data.Slmc, slmc))
                {
                    return data;
                }
            }
            entity.Slmc = slmc;
            entities.Add(entity);
            return entity;
        }
        private CBDWHEntity FindOrCreateCbd(List<CBDWHSLEntity> entities,  Dictionary<string, string> tb)
        {
            String dkid = tb[DKIDKey];
            foreach (var entity in entities)
            {
                foreach (CBDWHEntity item in entity.CBDWHEntities)
                {
                    if(item.DKID == dkid)
                    {
                        return item;
                            
                    }
                }
            }
            return null;
            
           
        }
        protected override string GetPrimaryVal(CBDWHSLEntity zong)
        {
            return zong.Slmc;
        }

        public ListRequestOption GetListRequestOption()
        {
           
            //this.Entities[0].CBDWHEntities[0].DBLogID = "我是数据库id号";
            // WriteZdidToExcel(this.Entities[0].CBDWHEntities[0]);


            ListRequestOption listRequestOption = new ListRequestOption()
            {
                BeforeActionDel = (IProgressControl gp) =>
                {
                    var cbdsl = this.Entities[0];
                    Dictionary<String, String> xzqdmDict = null;
                    this.ExListRequstTask(gp, new ListRequstTask()
                    {
                        TaskName = "前置工作",
                        RequstTasks = new List<RequstTask>()
                        {
                            new RequstTask()
                            {
                                TaskName = "查询行政区代码",
                                Data = cbdsl,
                                customAction = () =>
                                {
                                     //throw new ExceptionQuanJi("测试错误", null, JObject.FromObject(cbdsl), CbdProcessType.承包地维护.ToString());

                                     xzqdmDict = base.SearchQXDM(cbdsl, "土地承包经营权-维护-创建受理");
                                    if (xzqdmDict == null)
                                    {
                                        return false;
                                    }
                                    return true;
                                }
                            },
                            new RequstTask()
                            {
                                TaskName = "配置代码名称",
                                 Data = cbdsl,
                                customAction = () =>
                                {
                                      List<JObject> setDatas = new List<JObject>();
                                        foreach (var data in this.Entities)
                                        {
                                            foreach (var cbd in data.CBDWHEntities)
                                            {
                                                setDatas.Add(cbd.Jcxx);
                                            }

                                        }
                                        ReplaceDM(setDatas, xzqdmDict);
                                        return true;
                                }
                            },
                        }
                    });
                    return true;
                }
            };
            return listRequestOption;
        }
        /// <summary>
        ///  承包地维护
        /// </summary>
        internal void Cbdwh()
        {
            List<String> notFinds = this.FindFj(this.buLuDataColleciton, this.Entities);
            if (notFinds.Count > 0)
            {
                projectService.WriteErrors("图未找到", notFinds);
                MessageBox.Show(notFinds.Count + " 条数据没有找到图！");
                
            }
            ListRequestOption listRequestOption = GetListRequestOption();
            new ProcessService().ListRequest(this.Entities, "承包地维护", (zongdientity, gp) =>
            {

                try
                {
                    //ZongDiReponseEntity zongDiReponseEntity = GetValue(zongdientity, "${ZongDiReponseEntity}") as ZongDiReponseEntity;

                    Cbdwh(zongdientity, gp);
                }
                catch (ExceptionQuanJi ex)
                {
                    return this.HandleExceptionQuanJi(gp, ex, zongdientity);

                }
                finally
                {

                }
                return null;
            }, zongdientity =>
            {
                return GetPrimaryVal(zongdientity);
            }, listRequestOption);
        }

       

        private List<string> FindFj(ChengBaoDiDataCollection dataColleciton, List<CBDWHSLEntity> entities)
        {
            List<string> errors = new List<string>();
            List<String> paths = FileTool.FindAllFiles(dataColleciton.CBDWHTuDir, "*");
            Dictionary<String, String> fileNamePaths = new Dictionary<string, string>();
            foreach (var item in paths)
            {
                String fileName = Path.GetFileName(item);
                if (!fileNamePaths.ContainsKey(fileName))
                {
                    fileNamePaths.Add(fileName, item);
                }
            }
            foreach (var item in entities)
            {

                foreach (var cBDWHEntity in item.CBDWHEntities)
                {
                    if (cBDWHEntity.Jcxx == null)
                    {
                        continue;
                    }
                    String name = JsonTool.GetStringValue(cBDWHEntity.Jcxx, "图名称");
                    if (StringUtils.IsNullOrTrimEmpty(name))
                    {
                        errors.Add("承包地图，文件名没有填写！");
                        continue;
                    }
                    if (fileNamePaths.ContainsKey(name))
                    {
                        cBDWHEntity.CbdMapPath = fileNamePaths[name];
                        if (cBDWHEntity.CbdMapPath.ToLower().EndsWith("dwg"))
                        {
                            cBDWHEntity.MapFileType = MapFileType.DWG;

                        }
                        else if (cBDWHEntity.CbdMapPath.ToLower().EndsWith("shp"))
                        {
                            cBDWHEntity.MapFileType = MapFileType.SHP;
                        }
                        else
                        {
                            errors.Add("不支持承包地图文件类型（当前只支持：dwg,shp），文件名：" + name);
                        }
                    }
                    else
                    {
                        errors.Add("没有找到承包地图，文件名：" + name);
                    }
                } 
            }
            return errors;
        }

        /// <summary>
        /// 承包地维护
        /// </summary>
        /// <param name="zongdientity"></param>
        /// <param name="gp"></param>
        private void Cbdwh(CBDWHSLEntity data, ProgressDialog.IProgressControl gp)
        {

           

            QueryOrCreateSl(gp, data, "通用-查找受理信息", "土地承包经营权-维护-创建受理");
            this.ExListRequstTask(gp, new ListRequstTask()
            {
                Data = data,
                RequstTasks = new List<RequstTask>()
                {
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = "查找项目信息",
                        RequstName = "查找宗地的xxmid",
                        ResultDataType = DataType.String,
                        ResultFiledName = "${ZongDiReponseEntity.Xmxx}"
                    },
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = "查找宗地列表",
                        RequstName = "查找宗地基本信息",
                        ResultDataType = DataType.DATA_ROWS,
                        ResultFiledName = "${ZongDiReponseEntity.Common.Dklist}",
                        debuggAction = () =>
                        {


                        }
                    }
                }
            });
            List<CBDWHEntity> cbds_tb;
            List<JObject> deleteDks_Db;
            HuFenOpreateData(data, out cbds_tb, out deleteDks_Db);

            //执行删除
            foreach (var dk_db in deleteDks_Db)
            {
                //删除图形
                //删除属性
            }

            //如果所有的图都是未上传，那么就一次性上传所有的图
           /* bool isAllCreate = true;
            foreach (var dk_tb in cbds_tb)
            {
                if (dk_tb.DataOperationType != DataOperationType.CREATE)
                {
                    isAllCreate = false;
                    
                }
            }
            if (isAllCreate)
            {
                UploadMapFile(data, gp);
            }
            String zdid = data.CurrentDK.DBLogID;*/
           

            //新增
            foreach (var dk_tb in cbds_tb)
            {
                //每个地块上传
                data.CurrentDK = dk_tb;
                String zdid = data.CurrentDK.DBLogID;
                if (dk_tb.DataOperationType == DataOperationType.CREATE)
                {
                    UploadMapFile(data, gp);
                }

                this.RefreshJcxx(gp,data);
                this.RefreshTdyt(gp, data);
                this.RefreshTdcbxx(gp, data);
                this.RefreshTdcbjyq(gp, data);
                this.RefreshTdcbjyqr(gp, data);
            }

        }

       

        /// <summary>
        /// 土地承包经营权-维护-土地承包经营权人
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="data"></param>
        private void RefreshTdcbjyqr(IProgressControl gp, CBDWHSLEntity data)
        {
            var dk_tb = data.CurrentDK;
            String zdid = data.CurrentDK.DBLogID;

            base.RefreshList(gp, data,new RefreshListEntity()
            {
                Datas = dk_tb.Tdcbjyqrs,
                TaskName = "更新土地承包经营权人,ZDID=" + zdid,
                ListTaskName = "土地承包经营权人-查找列表",
                RequestListName = "土地承包经营权-维护-土地承包经营权人-查找列表",
                ListFieldName = "${ZongDiReponseEntity.Common.TdcbjyqrList}",
                DetailsTaskName = "查找土地承包经营权人-详情",
                RequestDetailsName = "土地承包经营权-维护-土地承包经营权人-详情",
                DbDataFieldName = "${ZongDiReponseEntity.Common.Tdcbjyqr}",
                DbDetailsFieldName = "${ZongDiReponseEntity.Common.TdcbjyqrDetails}",
                NewTaskName = "土地承包经营权人-新增:${ZongDiReponseEntity.Common._CurrentData.权利人名称}",
                RequestNewName = "土地承包经营权-维护-土地承包经营权人-新增",
                DeleteTaskName = "土地承包经营权人-删除:${ZongDiReponseEntity.Common.TdcbjyqrDetails.QLRMC}",
                RequestDeleteName = "土地承包经营权-维护-土地承包经营权人-删除",
                UpdateTaskName = "土地承包经营权人-编辑:${ZongDiReponseEntity.Common.TdcbjyqrDetails.QLRMC}",
                RequestUpdateName = "土地承包经营权-维护-土地承包经营权人-编辑",
                IsEqAction = (data1, tdcbjyqrDetails) =>{

                    if(StringUtils.TrimEq(JsonTool.GetStringValue(data1, "权利人名称"), JsonTool.GetStringValue(tdcbjyqrDetails, "QLRMC"))){
                        return true;
                    }
                    return false;
                }
            });
            

        }

        /// <summary>
        /// 更新土地承包经营权
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="data"></param>
        private void RefreshTdcbjyq(IProgressControl gp, CBDWHSLEntity data)
        {
            var dk_tb = data.CurrentDK;
            String zdid = data.CurrentDK.DBLogID;
            base.RefreshListData(gp, data, new RefreshListEntity()
            {
                Datas = new JArray() { data .CurrentDK.Tdcbjyq},
                TaskName = "更新土地承包经营权,ZDID=" + zdid,
                ListTaskName = "土地承包经营权列表查询",
                RequestListName = "土地承包经营权-维护-土地承包经营权列表查询",
                ListFieldName = "${ZongDiReponseEntity.Common.TdcbjyqList}",
                DetailsTaskName = "土地承包经营权详情查询",
                RequestDetailsName = "土地承包经营权-维护-土地承包经营权详情查询",
                DbDataFieldName = "${ZongDiReponseEntity.Common.Tdcbjyq}",
                DbDetailsFieldName = "${ZongDiReponseEntity.Common.TdcbjyqDetails}",
                NewTaskName = "土地承包经营权新增",
                RequestNewName = "土地承包经营权-维护-土地承包经营权新增",
                UpdateTaskName = "土地承包经营权编辑",
                RequestUpdateName = "土地承包经营权-维护-土地承包经营权编辑",
            }) ;

            //开始更新地块信息
           /* this.ExListRequstTask(gp, new ListRequstTask()
            {
                TaskName = "更新土地承包经营权,ZDID=" + zdid,
                Data = data,
                CheckFiledCanEx = "${Zd}",
                RequstTasks = new List<RequstTask>()
                {
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = "土地承包经营权列表查询",
                        RequstName = "土地承包经营权-维护-土地承包经营权列表查询",
                        ResultDataType = DataType.DATA_ROWS_First,
                        ResultFiledName = "${ZongDiReponseEntity.Common.Tdcbjyq}",
                        ExceptionHandleType = ExceptionHandleType.Catch,
                        debuggAction = () =>
                        {

                        }
                    },
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = "土地承包经营权详情查询",
                        RequstName = "土地承包经营权-维护-土地承包经营权详情查询",
                        CheckFiledCanEx = "${ZongDiReponseEntity.Common.Tdcbjyq}",
                        ResultDataType = DataType.DATA,
                        ResultFiledName = "${ZongDiReponseEntity.Common.TdcbjyqDetails}",
                        ElseAction = () =>
                        {
                            return this.ExRequstTask(gp,new RequstTask()
                            {
                                Data = data,
                                TaskName = "土地承包经营权新增",
                                RequstName = "土地承包经营权-维护-土地承包经营权新增",
                            });
                        }
                    },
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = "土地承包经营权编辑",
                        RequstName = "土地承包经营权-维护-土地承包经营权编辑",
                        CheckFiledCanEx = "${ZongDiReponseEntity.Common.TdcbjyqDetails}",
                    },

                }
            });*/
        }
        /// <summary>
        /// 更新土地承包信息
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="data"></param>
        private void RefreshTdcbxx(IProgressControl gp, CBDWHSLEntity data)
        {
            var dk_tb = data.CurrentDK;
            String zdid = data.CurrentDK.DBLogID;
            if (dk_tb.DataOperationType == DataOperationType.CREATE)
            {
                UploadMapFile(data, gp);
            }
            //开始更新地块信息
            base.RefreshMainData(gp, data, new RefreshMainDataEntity()
            {
                Data = data.CurrentDK.Jcxx,
                TaskName = "更新土地承包信息,ZDID=" + zdid,
                DetailsTaskName = "地块详情查询",
                RequestDetailsName = "土地承包经营权-维护-地块详情查询",
                CheckDbDetailsFieldNameVaild = "${ZongDiReponseEntity.Common.ZDXXDetails}",
                DbDetailsFieldName = "${ZongDiReponseEntity.Common.ZDXXDetails}",

                UpdateTaskName = "土地承包信息编辑",
                RequestUpdateName = "土地承包经营权-维护-土地承包信息编辑",
                UpdateFieldName = "${ZongDiReponseEntity.Common.ZDXXDetails}",
            });
        }
        /// <summary>
        /// 更新土地用途
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="data"></param>
        private void RefreshTdyt(IProgressControl gp, CBDWHSLEntity data)
        {
            var dk_tb = data.CurrentDK;
            String zdid = data.CurrentDK.DBLogID;
            base.RefreshListData(gp, data, new RefreshListEntity()
            {
                Datas = new JArray() { data.CurrentDK.Tdyt },
                TaskName = "更新土地用途,ZDID=" + zdid,
                ListTaskName = "查找土地用途列表查询",
                RequestListName = "土地承包经营权-维护-土地用途列表查询",
                ListFieldName = "${ZongDiReponseEntity.Common.TdytList}",
                DetailsTaskName = "查找土地用途详情查询",
                RequestDetailsName = "土地承包经营权-维护-土地用途详情查询",
                DbDataFieldName = "${ZongDiReponseEntity.Common.Tdyt}",
                DbDetailsFieldName = "${ZongDiReponseEntity.Common.TdytDetails}",
                NewTaskName = "土地用途新增",
                RequestNewName = "土地承包经营权-维护-土地用途新增",
                UpdateTaskName = "土地用途编辑",
                RequestUpdateName = "土地承包经营权-维护-土地用途编辑",
            });
            //开始更新地块信息
            /*this.ExListRequstTask(gp, new ListRequstTask()
            {
                TaskName = "更新土地用途,ZDID=" + zdid,
                Data = data,
                CheckFiledCanEx = "${Zd}",
                RequstTasks = new List<RequstTask>()
                {
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = "查找土地用途列表查询",
                        RequstName = "土地承包经营权-维护-土地用途列表查询",
                        ResultDataType = DataType.DATA_ROWS_First,
                        ResultFiledName = "${ZongDiReponseEntity.Common.Tdyt}",
                        ExceptionHandleType = ExceptionHandleType.Catch,
                        debuggAction = () =>
                        {

                        }
                    },
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = "查找土地用途详情查询",
                        RequstName = "土地承包经营权-维护-土地用途详情查询",
                        CheckFiledCanEx = "${ZongDiReponseEntity.Common.Tdyt}",
                        ResultDataType = DataType.DATA,
                        ResultFiledName = "${ZongDiReponseEntity.Common.TdytDetails}",
                        ElseAction = () =>
                        {
                            return this.ExRequstTask(gp,new RequstTask()
                            {
                                Data = data,
                                TaskName = "土地用途新增",
                                RequstName = "土地承包经营权-维护-土地用途新增",
                            });
                        }
                    },
                    new RequstTask()
                    {
                        Data = data,
                        TaskName = "土地用途编辑",
                        RequstName = "土地承包经营权-维护-土地用途编辑",
                        CheckFiledCanEx = "${ZongDiReponseEntity.Common.TdytDetails}",
                    },
                   
                }
            });*/
        }
        /// <summary>
        /// 更新基本信息
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="data"></param>
        private void RefreshJcxx(IProgressControl gp, CBDWHSLEntity data)
        {
            var dk_tb = data.CurrentDK;
            String zdid = data.CurrentDK.DBLogID;
            /*if (dk_tb.DataOperationType == DataOperationType.CREATE)
            {
                UploadMapFile(data, gp);
            }*/
            //开始更新地块信息
            base.RefreshMainData(gp, data, new RefreshMainDataEntity()
            {
                Data = data.CurrentDK.Jcxx,
                TaskName = "更新地块信息,ZDID=" + zdid,
                DetailsTaskName = "地块详情查询",
                RequestDetailsName = "土地承包经营权-维护-地块详情查询",
                DbDetailsFieldName = "${ZongDiReponseEntity.Common.ZDXXDetails}",
                RequestDetailsFinishAction = () =>
                {
                    return this.ExRequstTask(gp, new RequstTask()
                    {
                        Data = data,
                        TaskName = "生成宗地不动产单元号",
                        RequstName = "土地承包经营权-维护-地块编辑-不动产单元号",
                        ResultDataType = DataType.String,
                        //如果没有不动产单元号就要创建
                        CheckFiledNameVaild = "${ZongDiReponseEntity.ZDXXDetails.BDCDYH}",
                        ResultFiledName = "${ZongDiReponseEntity.ZDXXDetails}",
                        debuggAction = () =>
                        {

                        }
                    });
                },
                UpdateTaskName = "地块编辑",
                RequestUpdateName = "土地承包经营权-维护-地块编辑",
            });
        }
        private void UploadMapsFile(CBDWHSLEntity data, ProgressDialog.IProgressControl gp)
        {
            var RequstTasks = new List<RequstTask>();
            if (data.CurrentDK.MapFileType == MapFileType.SHP)
            {
                this.UploadShpFile(gp, data, "土地承包经营权-维护-图形上传", data.MapsFilePath);
            }
            else if (data.CurrentDK.MapFileType == MapFileType.DWG)
            {
                this.UploadDwgFile(gp, data, "土地承包经营权-维护-图形上传", data.MapsFilePath);
            }
            RequstTasks.AddRange(new List<RequstTask>()
            {
                new RequstTask()
                {
                    Data = data,
                    TaskName = "上传完成回调",
                    RequstName="土地承包经营权-维护-图形上传完成回调",
                    debuggAction = () =>
                    {
                    }
                },
                new RequstTask()
                {
                    Data = data,
                    TaskName = "查找地块列表",
                    RequstName = "土地承包经营权-维护-地块列表查询",
                    ResultDataType = DataType.DATA_ROWS,
                    ResultFiledName = "${ZongDiReponseEntity.Common.Dklist}"
                },
                new RequstTask()
                {
                    Data = data,
                    TaskName = "查找上传地块信息",
                    customAction = () =>
                    {
                        var datas = data.ZongDiReponseEntity.Common["Dklist"] as JArray;
                        //找到每个地块对应的数据库ZDID
                        foreach (var item in datas)
                        {
                            var obj = item as JObject;
                            String ZDDM = JsonTool.GetStringValue(obj,"ZDDM");
                        }
                        data.CurrentDK.Db_Dk = datas[datas.Count-1] as JObject;
                        return true;
                    }
                },
                new RequstTask()
                {
                    Data = data,
                    TaskName = "将ZDID写入表格中",
                    customAction = () =>
                    {
                        this.WriteZdidToExcel(data.CurrentDK);
                        return true;
                    }
                }
            });
            this.ExListRequstTask(gp, new ListRequstTask()
            {
                TaskName = "土地承包经营权-维护-图形上传",
                Data = data,
                CheckFiledCanEx = "${JCXX}",
                RequstTasks = RequstTasks,
            });
        }
        private void UploadMapFile(CBDWHSLEntity data, ProgressDialog.IProgressControl gp)
        {
            var RequstTasks = new List<RequstTask>();
            if(data.CurrentDK.MapFileType == MapFileType.SHP)
            {
                this.UploadShpFile(gp, data, "土地承包经营权-维护-图形上传", data.CurrentDK.CbdMapPath);
            }else if (data.CurrentDK.MapFileType == MapFileType.DWG)
            {
                this.UploadDwgFile(gp, data, "土地承包经营权-维护-图形上传", data.CurrentDK.CbdMapPath);
            }
            RequstTasks.AddRange(new List<RequstTask>()
            {
                new RequstTask()
                {
                    Data = data,
                    TaskName = "上传完成回调",
                    RequstName="土地承包经营权-维护-图形上传完成回调",
                    debuggAction = () =>
                    {
                    }
                },
                new RequstTask()
                {
                    Data = data,
                    TaskName = "查找地块列表",
                    RequstName = "土地承包经营权-维护-地块列表查询",
                    ResultDataType = DataType.DATA_ROWS,
                    ResultFiledName = "${ZongDiReponseEntity.Common.Dklist}"
                },
                new RequstTask()
                {
                    Data = data,
                    TaskName = "查找上传地块信息",
                    customAction = () =>
                    {
                        var datas = data.ZongDiReponseEntity.Common["Dklist"] as JArray;
                        data.CurrentDK.Db_Dk = datas[datas.Count-1] as JObject;
                        return true;
                    }
                },
                new RequstTask()
                {
                    Data = data,
                    TaskName = "将ZDID写入表格中",
                    customAction = () =>
                    {
                        this.WriteZdidToExcel(data.CurrentDK);
                        return true;
                    }
                }
            });
            this.ExListRequstTask(gp, new ListRequstTask()
            {
                TaskName = "土地承包经营权-维护-图形上传",
                Data = data,
                CheckFiledCanEx = "${JCXX}",
                RequstTasks = RequstTasks,
            });
        }

        

        private static void HuFenOpreateData(CBDWHSLEntity data, out List<CBDWHEntity> cbds_tb, out List<JObject> deleteDks_Db)
        {
            JArray dklist_db = data.ZongDiReponseEntity.Common["Dklist"] as JArray;
            if (dklist_db == null)
            {
                dklist_db = new JArray();
                ;
            }
            cbds_tb = data.CBDWHEntities;
            deleteDks_Db = new List<JObject>();
            //编辑
            foreach (var dk_dbTemp in dklist_db)
            {
                JObject dk_db = dk_dbTemp as JObject;
                bool isFind = false;
                String zdid = JsonTool.GetStringValue(dk_db, "ZDID");
                foreach (var dk_tb in cbds_tb)
                {
                    if (dk_tb.DBLogID == zdid)
                    {
                        isFind = true;
                    }
                }
                //没有在维护表格中找到，就是需要删除数据
                if (!isFind)
                {
                    deleteDks_Db.Add(dk_db);
                }
            }
            //区分新增、修改
            foreach (var dk_tb in cbds_tb)
            {
                //数据库中没有找到，新增
                bool isFind = false;
                foreach (var dk_db in dklist_db)
                {
                    if (dk_tb.DBLogID == JsonTool.GetStringValue(dk_db as JObject, "ZDID"))
                    {
                        dk_tb.Db_Dk = dk_db as JObject;
                        isFind = true;
                    }
                }
                if (isFind)
                {
                    dk_tb.DataOperationType = DataOperationType.UPDATE;

                }
                else
                {
                    dk_tb.DataOperationType = DataOperationType.CREATE;
                }
            }
        }

        private void WriteZdidToExcel(CBDWHEntity currentDK)
        {
            List<JObject> datas = new List<JObject>();
            JObject data = new JObject();
            datas.Add(data);
            data.Add("地块ID", currentDK.DKID);
            data.Add("数据库记录ID", currentDK.DBLogID);
            
            base.WriteToExcel("地块ID", datas,this.buLuDataColleciton.CBDWHTablePath, "基本信息", 1);

        }


       

        
    }
}
