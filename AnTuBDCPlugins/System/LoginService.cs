using DevExpress.XtraEditors;
using BDCPlugins.Config;
using BDCPlugins.tool;
using ModuleBase.Entity;
using ModuleBase.Tool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BDCPlugins.BDCException;
using BDCPlugins.Service;

namespace BDCPlugins.BDCSystem
{
    class LoginService
    {
        
        public bool IsLogin { get; set; }
        /// <summary>
        /// 链接的延迟时间
        /// </summary>
        private int delay = 1000;
        private int loopdelay = 1000*60*10;
        private Dictionary<String, User> userDict ;
        ProjectService projectService;
        public LoginService(string account, string password)
        {
            Account = account;
            Password = password;
            projectService = new ProjectService();
        }

        public string Token { get; set; }
        public string Account { get; }
        public string Password { get; }

        internal  void CheckLoginLoop(bool isTokenNovalid)
        {
            //检查账号密码是否为null
            if(StringUtils.IsNullOrTrimEmpty(Account) || StringUtils.IsNullOrTrimEmpty(Password))
            {
                MessageBox.Show("输入的账号和密码，不能为空");
                throw new ErrorQuanJi("账号和密码，不能为空", null, null);
            }
            Console.WriteLine("1");
            IsLogin = false;
            //缓存读取所有user
            userDict =  AppTool.GetCacheTool().GetValueTobject<Dictionary<String, User>>("users");
            if (userDict == null)
            {
                userDict = new Dictionary<string, User>();
            }
            //找到对应的user
            User user;
            if(userDict.TryGetValue(this.Account,out user))
            {
                user.Password = this.Password;
                this.Token = user.Token;
               
                
            }

            //先试用token尝试连接，如果无效再登录
            bool isConnect = isTokenNovalid ||  this.CheckConnect();
            //token失效不要进去
            if (isConnect && !isTokenNovalid)
            {
                IsLogin = true;
                this.CheckConnectLoop();
                
            }
            else
            {
                //throw new Exception();
                (bool isLogin, String token, String message,User userTemp) = this.Login();
                if (!isLogin)
                {
                    MessageBox.Show(message);
                }
                else
                {
                    this.Token = token;
                    userTemp.Token = token;
                    userDict[this.Account] = userTemp;
                    ProjectConfig.TOKEN = token;
                    AppTool.GetCacheTool().SetValue("users",JObject.FromObject(userDict).ToString(),true);
                    IsLogin = true;


                }
            }
         
        }
        public  void RefresToken()
        {
            userDict = AppTool.GetCacheTool().GetValueTobject<Dictionary<String, User>>("users");
            if (userDict == null)
            {
                userDict = new Dictionary<string, User>();
            }
            //找到对应的user
            User user;
            if (userDict.TryGetValue(this.Account, out user))
            {
                user.Password = this.Password;
                this.Token = user.Token;
            }
        }
        private async void CheckConnectLoop()
        {
            new Task(()=>
            {
                Console.WriteLine("检查链接");
                CheckConnect();
                Thread.Sleep(loopdelay);
                CheckConnectLoop();
            }).Start();
           
        }



        /// <summary>
        /// 检查是否有链接
        /// </summary>
        /// <returns></returns>
        public  bool CheckConnect()
        {
            Console.WriteLine("2");
            if (this.Token == null)
            {
                RefresToken();
            }
            if (this.Token == null)
            {
                return false;
            }
            ProjectConfig.TOKEN = this.Token;
            try {
                if(ProjectConfig.ProjectSystem == ProjectSystem.QuanJi)
                {
                    projectService.CheckToken("权籍系统-持续链接", new JObject());
                }
                else
                {
                    projectService.CheckToken("登记系统-持续链接", new JObject());
                }
               
            }
            catch(Exception ex)
            {
                return false;
            }
            

            return true;

        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        private (bool,string,string,User) Login()
        {

            Console.WriteLine("登录");
            JObject json;
            //获取公钥
            if (ProjectConfig.ProjectSystem == ProjectSystem.QuanJi)
            {
                json = projectService.Request("权籍系统-获取公钥", new JObject(), false);
            }
            else
            {
                json = projectService.Request("登记系统-获取公钥", new JObject(), false);
            }

            
            String pulickKey = JsonTool.GetStringValue(json,"publicKey");
            String keyid = JsonTool.GetStringValue(json, "id");
            string encryptedText = RSAEncryptionWithBouncyCastle.EncryptStringWithBouncyCastle(this.Password, pulickKey);
            JObject jobject = new JObject();
            jobject["username"] = this.Account;
            jobject["password"] = encryptedText;
            jobject["keyid"] = keyid;

            //登录
            try
            {
                JObject user;
                if (ProjectConfig.ProjectSystem == ProjectSystem.QuanJi)
                {
                    user = projectService.Request("权籍系统-登录", jobject, true);
                }
                else
                {
                    user = projectService.Request("登记系统-登录", jobject, true);
                }

                if (JsonTool.GetStringValue(user, "username") == null)
                {
                    User loginUser = new User() { Account = this.Account, Password = this.Password, PasswordEncode = encryptedText, Token = JsonTool.GetStringValue(user, "token"), TenantID = JsonTool.GetStringValue(user, "tenantID") };
                    return (true, loginUser.Token, JsonTool.GetStringValue(user, "message"), loginUser);
                }
                return (false, JsonTool.GetStringValue(user, "message"), null, null);
            }
            catch(ExceptionQuanJi ex)
            {
                string msg = JsonTool.GetStringValue(ex.Response as JObject, "msg");
                MessageBox.Show(msg);
                throw new ErrorQuanJi(msg,null,null);
            }
           
        }

        internal void StopCheckLoginLoop()
        {
            
        }
    }
}
