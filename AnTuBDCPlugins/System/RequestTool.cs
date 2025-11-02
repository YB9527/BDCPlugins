using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

using BDCPlugins.Config;
using ModuleBase.Tool;
using System.Threading;
using BDCPlugins.Entity;
using BDCPlugins.BDCException;

namespace BDCPlugins.BDCSystem
{
   
    public static class RequestTool
    {
        private static LoginService loginService;
        private static String UserTag { get; set; }
        private static bool IsInitToken = false;
        /// <summary>
        /// 同步发送RequestEntity请求，支持POST和GET，返回JObject
        /// </summary>
        public static JObject SendRequest(RequestEntity request,Object body=null,bool isNoLogin = false  )
        {
            if (request.RequestDealy > 0)
            {
                Thread.Sleep(request.RequestDealy);
            }
            if(!IsInitToken && UserTag == null)
            {
                IsInitToken = true;
                new LoginService(AppTool.CurrentUser.Account, AppTool.CurrentUser.Password).CheckConnect();
                foreach (var item in request.Headers.Keys)
                {
                    if (item == "access_token")
                    {
                        request.Headers["access_token"] = ProjectConfig.TOKEN;
                        break;
                    }
                }

            }
            if( AppTool.UserTag != UserTag && UserTag != null)
            {
                //账号切换了
                bool bl =  Login(false);
                if (bl)
                {
                    UserTag = AppTool.UserTag;
                }
                else
                {
                    throw new ErrorQuanJi("账号密码无效，请登录" , null, null);
                }
                
            }
            if(request == null)
            {
                throw new ExceptionQuanJi("RequestEntity ：不能为NULL");
            }
            //检查是否有未转换的参数
            String jsonStr = JObject.FromObject(request).ToString();
            if (jsonStr.Contains("${"))
            {
                throw new ErrorQuanJi("请求参数有未转换的代码，请联系开发人员："+jsonStr,null,null);
            }
            var myRequest = new MyRequest();
            Dictionary<string, object> resultDict = null;
            if (request.Method.ToString().ToUpper() == "POST")
            {
                if(body != null)
                {
                    resultDict = myRequest.PostRequest(request.Url, request.Headers, body);
                }else if (request.ArrayBody != null && request.ArrayBody.Count > 0)
                {
                    resultDict = myRequest.PostRequest(request.Url, request.Headers, request.ArrayBody);
                }
                else
                {
                    //上传文件
                    if (request.Tag == Tag.FILE)
                    {
                        resultDict = myRequest.PostRequestFile(request.Url, request.Headers, request.Body, request.FilePath, request.Still);
                    }
                    else
                    {
                        resultDict = myRequest.PostRequest(request.Url, request.Headers, request.Body, request.Still);
                    }
                }
                
                 
            }else if (request.Method.ToString().ToUpper() == "GET")
            {
                resultDict = myRequest.GetRequest(request.Url, request.Headers );
            }
            else if (request.Method.ToString().ToUpper() == "DELETE")
            {
                resultDict = myRequest.DeleteSync(request.Url,new Dictionary<string, string>(), request.Headers, request.Body);
            }
            else if (request.Method.ToString().ToUpper() == "DOWNLOADFILE")
            {
                resultDict = myRequest.DownloadFile(request.Url, request.FilePath, request.Headers);
            }
            else
            {
                throw new NotSupportedException($"当前SendRequest只支持POST，GET,DELETE: {request.Method}");
            }

           
            if (resultDict != null && resultDict.ContainsKey("code") && resultDict["code"].ToString() == "901")
            {
                if(Login(true))
                {
                    foreach (var item in request.Headers.Keys)
                    {
                        if (item == "access_token")
                        {
                            request.Headers["access_token"] = ProjectConfig.TOKEN;
                            break;
                        }
                    }
                    return SendRequest(request, body);
                }

            }
            var json = MyRequest.GetReposeDataAll(resultDict);
            return json;
        }
        public static   bool Login(bool isTokenNovalid)
        {
            
            if (loginService != null)
            {
                loginService.StopCheckLoginLoop();
            }
            loginService = new LoginService(AppTool.CurrentUser.Account, AppTool.CurrentUser.Password);
            loginService.CheckLoginLoop(isTokenNovalid);
            if (loginService.IsLogin)
            {
                return true;
            }
            return false;
        }
    }
} 