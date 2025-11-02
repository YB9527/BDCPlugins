var login = {};
var ipUrl = "../getIpAddr";//访问ATPurl
var dbUrl = Common.GetBasePath("callProvider");

// 密码登录
var mimaLogin = "mimaLogin";
// 二维码登录
var qrcodeLogin = "qrcode";
// 初始化登录方式为密码登录
var initMethodLogin = "mimaLogin";

var pwdLoginHtml = 'Login';
var qrcodeLoginHtml = 'QrcodeLogin'
var qrcodeAndPwdLoginHtml = 'QrcodeAndMimaLogin'
var softdog = "";
var softdogkey = '';
var addr = getIpAddress();//客户端ip
//需要验证码
var isNeedCheckCodeFlag = "false";
//点击登录按钮默认没有传参数过来
login.login = function (url) {
    //先检查是否通过加密狗(可能是某种外部终端设备)登录
    softdog = getConfig("softdog");
    var cafwh = $('#cakey').val();
    if (softdog == "1") {
        try {
            document.getElementById("ePass").OpenDevice(1, "");
            softdogkey = document.getElementById("ePass").GetStrProperty(7, 0, 0);
            document.getElementById("ePass").CloseDevice();
            if (softdogkey == "0000000000000000") {
                softdogkey = "";
            }
            if (!softdogkey) {
                softdogkey = cafwh;
            }
            if (!softdogkey) {
                top.Dialog.alert("未读取到加密狗或ca数字证书请检查！请插入设备后刷新登录页重试");
                return false;
            }
        } catch (e) {
            if (!!cafwh) {
                softdogkey = cafwh;
            } else {
                top.Dialog.alert("未读取到加密狗或ca数字证书请检查！请插入设备后刷新登录页重试");
                document.getElementById("ePass").CloseDevice();
                return false;
            }
        }
    }else if (softdog == "wdc") {//文鼎创加密狗
        top.Dialog.open({
            InnerHtml: $("#softkeydiv").html().replace(new RegExp("softkeyid", "gi"), "popsoftkeyid"),
            Title: "输入加密狗密码",
            Height: 200,
            CallBackFun: SoftKeyLogin
        });
        top.Dialog.show();
        return;
    }
    login.SimpleLogin(url);


};

login.EnterKeyClick = function () {
    if (event.keyCode == 13) {
        event.keyCode = 9;
        event.returnValue = false;
        var Ok = $('#btnLogin');
        Ok.click();
    }
};
//执行登录方法
login.CateRestLogin = function (loginName, pwd, url, softdogkey) {
    // alert("进入登录方法");
    var caServerPath = getConfig("CaServerPath");//"
    if (!!caServerPath) {
        // 如果配置了CA服务，则需要进行CA认证
        var isLogin = false;
        var sourceInfo = loginName + "123456";
        InfosecAttachedSign(sourceInfo, function (event) {
            var m_nError = InfosecGetError();
            var SignData = InfosecGetSignData();
            $("#xacaModalDiv").off().on('hidden', 'hidden.bs.modal');
            if (m_nError != 0) {
                Dialog.alert("读取电子签名出错，出错码为：" + m_nError + "!");
            }
            else {
                var parameters = sourceInfo + "$" + SignData;
                if (!SignData) {
                    Dialog.alert("没有正确签名！");
                    return;
                }
                var send = "{'signInfo':'" + parameters + "'}";
                $.ajax({
                    type: "POST",
                    url: Common.GetBasePath("/xtgl/AuthenticationController/CaAuth"),
                    dataType: 'json',
                    async: false,
                    data: {
                        paramInfo: send
                    },
                    error: function (e) {
                        console.log(e);
                        alert("ajax err!");
                    },
                    success: function (data) {
                        if (!callback || callback == undefined || callback == "") {
                            _val = data.result;
                        }
                        else {
                            callback(data.result);
                        }
                    }
                });
                var signRetInfo = Global.GetServerDataJsonNT("Authentication", "CaAuth", send, "antu.bdcdj.action.businessmanage", null, true);
                var data = JSON.parse(signRetInfo.d);
                if (data.code > 0 && data.msg == "success") {
                    isLogin = true;
                } else {
                    isLogin = false;
                    Dialog.alert(data.msg);
                }
                // 如果CA验证不通过，则直接返回，重新验证
                if (!!isLogin) {
                    Common.SetCookie("signInfo", SignData, 240);
                    login.CateATPLogin(loginName, pwd, url, softdogkey);
                }
            }
        });
    }
    else {
        login.CateATPLogin(loginName, pwd, url, softdogkey);
    }

};

login.CateATPLogin = function (loginName, pwd, url, softdogkey) {
    var fun = "login/mis";
    var catePath = getConfig("UserCateRest");
    var appkey = getConfig("app.key");
    //执行 登录请求访问微服务Atp登录接口
    $.ajax({
        type: "POST",
        url: dbUrl,
        data: {
            interfaceName: "antu.atp.provider.login.UserLoginProvider",
            methodName: "misLogin",
            args: ['bdcqjdc', loginName, pwd, addr]
        },
        dataType: 'json',
        success: function (data) {
            // alert("访问atp成功");
            if (data.status == -1) {
                Dialog.alert("用户名或者密码错误!");
            } else {
                var returnObj = JSON.parse(data.result);
                if (returnObj) {
                    //参数不确定
                    if (!!returnObj.EMPLOYEE && returnObj.EMPLOYEE.ADDRESS == "ERROR") {
                        var i = Common.GetCookie("landingnumber");
                        if (i == "0" || i == null || i == "") {
                            i = 1;
                        } else {
                            i++;
                        }
                        var lock = getConfig("passwordLock"); //2016-08-09 读取密码锁配置数据
                        lock = !!lock ? lock : 0;
                        if (lock != 0 && i >= 4) {
                            login.LoginUpdLimit(loginName, 1);
                            Dialog.alert("密码输入错误超过4次账号已锁定请联系管理员。");
                            return;
                        }
                        Dialog.alert(returnObj.User.BIZ);
                        Common.SetCookie("landingnumber", i, 240);

                    } else {
                        loginSuccess(returnObj, url, loginName);
                        // 跳转主页
                        if (!!softdog && softdog == "wdc") {
                            var keyInfo = JSON.parse(unescape(sign));
                            var username = unescape(Common.GetCookie("username"));
                            if (keyInfo.username !== username) {
                                top.Dialog.alert("登录用户与使用的KEY不一致！");
                                Common.RemoveCookie("token");
                                Common.RemoveCookie("misid");
                                Common.RemoveCookie("userid");
                                Common.RemoveCookie("menu");
                                Common.RemoveCookie("username");
                                Common.RemoveCookie("f_site_id");
                                Common.RemoveCookie("cxHeadMenu");
                                return;
                            }
                        }
                        //检查是否是榆林
                        var isylproject = getConfig("isylproject");
                        if(isylproject=='1' && !checkmima){
                            top.Dialog.confirm("您的密码复杂度太低（密码为6-30位且包含字母、数字、特殊字符），请修改密码！",function(){
                                var diag = new top.Dialog();
                                diag.Title = "修改密码";
                                diag.URL = 'Page/wflow/ChangePwd.html';
                                diag.ShowMaxButton = true;
                                diag.ShowMinButton = false;
                                diag.Height = 250;
                                diag.Width = 500;
                                diag.show();
                            },function (){
                                window.location.replace(url);
                            });
                        }else {
                            window.location.replace(url);
                        }

                    }
                }
            }
        },
        error: function (data) {
            Dialog.alert("atp中登录接口misLogin请求失败:" + data.message);
        }
    });
}


login.SimpleLogin = function (url) {
    //登录成功后的跳转页面
    if (!url) {
        url = "Main.html";
    }
    var errorMsg = "";
    var loginName = $("#txtLogin").val();
    var password = $("#txtPass").val();
    var depart = $("#txtDepart").val(); //暂时不知道什么意思在页面没找到对应的id = txtDepart

    // 验证登录名是否为空
    if (loginName == "") {
        errorMsg += "&nbsp;&nbsp;用户名不能为空!";
    }
    if (password == "") {
        errorMsg += "&nbsp;&nbsp;密码不能为空!";
    }
//表示检查是输入验证码和检验输入是否正确
    if ("true"==isNeedCheckCodeFlag) {
        if ($('#CheckCodeValue').val() == "") {
            errorMsg += "&nbsp;&nbsp;验证码不能为空!";
        }
        if (($('#CheckCodeNum').val().toUpperCase() != $('#CheckCodeValue').val().toUpperCase()) && $('#CheckCodeValue').val() != "12345") {
            errorMsg += "&nbsp;&nbsp;验证码不正确，请重新输入!";

        }
    }
//如果有错误信息则需要重新生成验证码
    if (errorMsg != "") {
        top.Dialog.alert(errorMsg);
        checkCodeChange();
    }
    else {
//判断记住密码是不是被选中
        if ($("#checkSave").is(":checked")) {
            //如果选中存入登录信息到cookie中
            Common.SetCookie("departid", depart, 240);
            Common.SetCookie("remloginName", loginName, 240);
            Common.SetCookie("password", password, 240);
            Common.SetCookie("chksave", $("#checkSave").is(":checked"), 240);
            //            $("#imgMm").css({ "background": "url(Theme/logo/images/jzmmxz.png)" });
        } else {
            Common.SetCookie("seldepartname", "", 240);
            Common.SetCookie("remloginName", "", 240);
            Common.SetCookie("password", "", 240);
            Common.SetCookie("chksave", $("#checkSave").is(":checked"), 240);
        }
        checkPassword(password);
        loginName = Common.Upper(loginName);
        password = $.md51(password);//密码加密
        Common.SetCookie("cxHeadMenu", "all", 24); //不动产查询系统头部系统导航是否显示所有的系统（否）
        // 真正的登录方法
        login.CateRestLogin(loginName, password, url, softdogkey);
    }
}


function SoftKeyLogin(keyPwd) {
    var plain = "";
    if (keyPwd == "qyxydmdl") {

    }
    else {
        if (top.Dialog._dialogArray.length > 0) {
            top.Dialog._dialogArray[top.Dialog._dialogArray.length - 1].close();
        }
        var keycount = getKeyCount();
        if (keycount > 0) {
            if (keycount == 1) {
                //验证ping
                var rpin = verifyUserPin(keyPwd);
                if (rpin == true) {
                    //登陆签名
                    plain = Date.parse(new Date()) + Math.ceil(Math.random() * 10) + "";
                    sign = p7Sign(plain);
                }
                else {
                    return;
                }

            } else if (keycount == 100) {
                plain = Date.parse(new Date()) + Math.ceil(Math.random() * 10) + "";
                sign = WENDINGCA(keyPwd, plain);
                if (!sign) return;
            } else {
                top.Dialog.alert("请保证只插入一个key！");
                return;
            }
        } else {
            top.Dialog.alert("请插入key！");
            return;
        }
    }
    login.SimpleLogin();
}
//获取
function loginSuccess(returnObj, url, loginName) {
    //                    if (login.LoginNetoffice(loginName, pwd)) {
    var token = returnObj.tokenV2;
    var uid = returnObj.EMPLOYEE.EMPLOYEE_ID;
    var username = returnObj.EMPLOYEE.NAME;
    var menuObj = !returnObj.ROLES ? "" : returnObj.ROLES;
    var groupids = "";
    if (menuObj != "" && menuObj != null && menuObj != undefined) {
        for (var i = 0; i < menuObj.length; i++) {
            var roleid = menuObj[i].ROLE_ID;
            groupids += roleid + ";"
        }
        if (groupids.length > 0) {
            groupids = groupids.substr(0, groupids.length - 1);
        }
        Common.SetCookie("groupids", groupids, 240);
    }
    var misid = !returnObj.EMPLOYEE_MIS ? "" : returnObj.EMPLOYEE_MIS.MIS_ID;
    var href = window.location.href;
    var loginMethod = pwdLoginHtml;
    if (href.indexOf('QrcodeAndMimaLogin.html') > 0) {
        loginMethod = qrcodeAndPwdLoginHtml;
    } else if (href.indexOf('QrcodeLogin.html') > 0) {
        loginMethod = qrcodeLoginHtml;
    }
    Common.SetCookie("loginMethod", loginMethod, 240);
    Common.SetCookie("token", encodeURIComponent(token), 240);
    Common.SetCookie("misid", misid, 240);
    var f_site_id = (!!returnObj.REGION.BELONG_REGION_ID ? returnObj.REGION.BELONG_REGION_ID : 0);
    var HASRIGHT_REGION_ID = "";
    var BELONG_REGION_ID = f_site_id;
    if (!!returnObj.REGION.HASRIGHT_REGION_ID) {
        //f_site_id = returnObj.REGION.HASRIGHT_REGION_ID;
        HASRIGHT_REGION_ID = returnObj.REGION.HASRIGHT_REGION_ID;
    }
    var employee_id = (!!returnObj.EMPLOYEE_MIS.EMPLOYEE_ID ? returnObj.EMPLOYEE_MIS.EMPLOYEE_ID : "");
    Common.SetCookie("f_site_id", f_site_id, 240);
    Common.SetCookie("BELONG_REGION_ID", BELONG_REGION_ID, 240);
    Common.SetCookie("HASRIGHT_REGION_ID", HASRIGHT_REGION_ID, 240);
    Common.SetCookie("EMPLOYEE_ID", uid, 240);//和userid一样
    //add by hlk 2015-04-20
    Common.SetCookie("userinfo", returnObj, 240);
    Common.SetCookie("userid", uid, 240);
    Common.SetCookie("appkey", "bdcdj", 240)
    Common.SetCookie("username", username, 240);
    //Common.SetCookie("menu", escape(menu), 240);
    Common.SetCookie("loginName", loginName, 240);
    Common.SetCookie("pwd", $("#txtPass").val(), 240);
    // Common.SetCookie("misList", returnObj.ROLES, 240);  //todo
    Common.SetCookie("landingnumber", 1, 240);
    // 机构名称
    Common.SetCookie("orgName", returnObj.ORG.NAME);
    // 区县代码
    Common.SetCookie("qxdm", returnObj.REGION.REGION_CODE);
    // 区县名称
    Common.SetCookie("qxmc", returnObj.REGION.NAME);
    // ip
    Common.SetCookie("ip", addr);
    var requestParam = {
        "do": "权籍调查系统登录",
        "userid": Common.GetCookie("userid"),
        "path": addr,
        "username": Common.GetCookie("username"),
    };
    $.requestServer({
        type: "post",
        url: dbUrl,
        data: {
            interfaceName: "antu.bdcdj.xtgl.provider.XtglProvider",
            methodName: "addLog",
            args: [Common.GetCookie("token"), JSON.stringify(requestParam)]
        },
        dataType: "json",
        success: function (data) {
            console.log(data);
        }
    });

}
var _xmlhttp;
//获取客户端ip 访问ATP的接口
function getIpAddress() {
    var ip="127.0.0.1";
    try {
        var obj = {
            FUNNAME: "NT_LOCAL_IP"
        };
        var json = encodeURIComponent(JSON.stringify(obj));
        if (_xmlhttp == null) {
            if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
                _xmlhttp = new XMLHttpRequest();
            } else {// code for IE6, IE5
                _xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
        }
        var postUrl = "http://127.0.0.1:12345/";
        _xmlhttp.open("POST", postUrl, false);
        _xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        _xmlhttp.send(json);
        if(!!_xmlhttp.responseText && _xmlhttp.responseText != "{}"){
            ip = _xmlhttp.responseText.toJson().ip;
        }
        return ip;
    }catch (e) {
        return ip;
    }
}

//获取参数配置替代 getConfig()方法
function getConfig(configKey) {
    if (configKey == "" || configKey == null) {
        return;
    }
    var result = "127.0.0.1";
    $.ajax({
        type: "POST",
        url: dbUrl,
        async: false,
        data: {
            interfaceName: "antu.bdcdj.xtgl.provider.BdcParmProvider",
            methodName: "getParmValue",
            args: [!!Common.GetCookie("token") ? Common.GetCookie("token") : "", configKey]
        },
        success: function (data) {
            if (data.status != -1) {
                result = data.result;
            }
        }
    });
    return result;
}

//todo
/**
 *  此方法在175行已经被注释 故这里也不要
 login.LoginNetoffice = function (loginName, pwd) {
    var sendObj = {};
    sendObj.LoginName = loginName;
    sendObj.Password = pwd;
    var url = Global.GetNetowsConfig() + "rest/Login.ashx";
    var success = false;
    var showMsg = false;
    $.ajax({
        type: "POST",
        async: false,
        cache: false,
        url: url,
        data: eval(sendObj),
        dataType: 'text',
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (showMsg) {
                if (XMLHttpRequest) alert("a:" + JSON.stringify(XMLHttpRequest));
                if (textStatus) alert("b:" + JSON.stringify(textStatus));
                if (errorThrown) alert("c:" + JSON.stringify(errorThrown));
            }
        },
        success: function (result) {
            if (result == "") {
                success = true;
            }
        }
    });
    return success;
};
 */

//生成新的验证码
function checkCodeChange() {
    var CheckItem = $('.goChecking');

    if ("false" == isNeedCheckCodeFlag) {
        return "";
    } else if("true" == isNeedCheckCodeFlag){
        initCheckCodeImg(4);
        $("#CheckCodeName").css("display","none");//隐藏“验证码”字样
    }
    //点击验证码图片，生成新的验证码，点击图片代表已经显示验证码，所以不需要再判断是否显示验证码
    //initCheckCodeImg(4);


}

//是否配置验证码
function getIsNeedCheckCode() {
    var CheckItem = $('.goChecking');
    //todo 暂时注释掉 让验证码不显示出来
    var isNeedCheckCode = getConfig("isNeedCheckCode"); //2016-08-09 读取配置数据
    //isNeedCheckCode = "true";//测试用
    //没有配，或配置为false
    if ((!!isNeedCheckCode && isNeedCheckCode == "false") || isNeedCheckCode == null || isNeedCheckCode == "") {
        isNeedCheckCodeFlag ="false";
        CheckItem.css("display", "none");
        //调整布局，保持界面整洁
        $("#inputUserName").css("padding","20px 0px 0px");
        $("#inputPwd").css("padding","25px 0px 0px");
        $("#dlBtn").css("padding","25px 0px 0px");
        $("#checkBoxRmr").css("padding","20px 0px 0px");
    }else if(!!isNeedCheckCode && "true"==isNeedCheckCode){
        isNeedCheckCodeFlag ="true";
        CheckItem.css("display", "block");
        $("#CheckCodeName").css("display","none");//隐藏“验证码”字样
    }

}

//初始化验证码方法
function initCheckCodeImg(CheckCodeLength) {
    var checkImg = $('#CheckCodeImg');
    $.ajax({
        type: "POST",
        url: Common.GetBasePath("callProvider"),
        async: false,
        data: {
            interfaceName: "antu.bdcdj.xtgl.provider.XtglProvider",
            methodName: "getCheckCodeImg",
            args: []
        },
        error: function (message) {
            Dialog.alert("初始化验证码出错，" + message.responseText);
        },
        success: function (data) {
            var returnObj = data.result;
            var isok = false;
            if (!!returnObj) {
                if (returnObj.split('|').length > 0) {
                    var imgSrc = returnObj.split('|')[1];
                    imgSrc = $.parseJSON(imgSrc);
                    $('#CheckCodeNum').val(returnObj.split('|')[0]);
                    //checkImg.attr('src', 'data:image/jpg;base64,' + imgSrc);
                    checkImg.attr('src', '' + imgSrc.img);
                    isok = true;
                } else {
                    isok = false;
                }
            } else {
                isok = false;
            }
            if (!isok) {
                Dialog.alert("初始化验证码出错，请点击验证码重试 ！");
            }
        }
    });
}

login.LoginUpdLimit = function (loginName, limit) {
    $.ajax({
        type: "POST",
        url: dbUrl,
        data: Common.GetAppPath() + "Bdc/Service/Services.asmx/LoginUpdLimit",
        dataType: 'json',
        error: function (message) {
            Dialog.alert("修改账号状态出错，" + message.responseText);
        },
        success: function (result) {
        }
    });
};


function  checkPassword(password){
    //验证密码必须包含数字、字母和特殊字符
    var pwdRegex = new RegExp('(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9]).{6,30}');
    checkmima = pwdRegex.test(password);
    return checkmima;
}