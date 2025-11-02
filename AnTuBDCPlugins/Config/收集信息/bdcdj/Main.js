var handleInterval;
var dbUrl=  Common.GetBasePath("callProvider");
var   ipUrl = "../getIpAddr";//访问ATPurl
var sessionOut = 1;
$().ready(function () {
    Common.Loading(true);
    main.GetDay();
    main.MenuOpenStaut();
    Common.Loading(false);
    initSys();
    //1分钟刷新保证Session不掉  IIS默认时间为20分钟 yhd 20150518
    var token = Common.GetCookie("token");
    var caServerPath = getConfig("CaServerPath");
    if (!!caServerPath && !!token) {
        try {
            init();
        } catch (e) {

        }
    }
    handleInterval = setInterval(main.refreshSession, 60000);
});
//获取参数配置替代 getConfig()方法
function getConfig (configKey){
    if(configKey =="" || configKey == null){
        return;
    }
    var result = "127.0.0.1";
    $.ajax({
        type: "POST",
        url: dbUrl,
        async:false,
        data: {
            interfaceName: "antu.bdcdj.xtgl.provider.BdcParmProvider",
            methodName: "getParmValue",
            args: [Common.GetCookie("token"), configKey]
        },
        success: function (data) {
            if(data.status != -1){
                result = data.result;
                //     alert(result);
            }
        }
    })
    return result;
};
var main = {
    menutype: "1",
    _openstype: "129009001",//多选
    tab: null,
    settingMore: {},
    rightmenu: {}
    //,"关闭右侧":{icon:'icon-arrow-left',text:'Back',click:main.rightmenucallback},"关闭全部":{icon:'icon-arrow-left',text:'Back',click:main.rightmenucallback}

};
//main.rightmenu["close"] = {icon:'glyphicon glyphicon-remove',text:'关闭',click:rightmenucallback};
main.rightmenu["closeright"] = { icon: 'glyphicon glyphicon-remove-sign', text: '关闭右侧', click: rightmenucallback };
main.rightmenu["closeall"] = { icon: 'glyphicon glyphicon-remove-circle', text: '关闭全部', click: rightmenucallback };

function rightmenucallback(target, element) {
    //alert(1);
    var id = $(element).attr("id");
    var pid = $(target).attr("id");
    if (id == "close") {
        main.tab.close(pid);
    }
    else if (id == "closeright") {
        var nextall = $("#" + pid).nextAll();
        for (var i = 0; i < nextall.length; i++) {
            var npid = $(nextall[i]).attr("id");
            main.tab.close(npid);
        }
    }
    else if (id == "closeall") {
        var nextall = $("#" + pid).siblings("[id!='tab1_index1']");
        for (var i = 0; i < nextall.length; i++) {
            var npid = $(nextall[i]).attr("id");
            main.tab.close(npid);
        }
        main.tab.close(pid);
    }
}
//获取客户端ip 访问ATP的接口
function getIpAddress () {
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
};
main.exitHandler = function () {
    var userid = Common.GetCookie("userid");
    var username = Common.GetCookie("username");
    var employee_id = Common.GetCookie("EMPLOYEE_ID");
    var json = {
        "do":"权籍系统用户注销",
        "iid": "",
        "userid": userid,
        "username":username,
        "employee_id":employee_id,
        "path": Common.GetCookie("ip")
    }
    var strison = encodeURIComponent(JSON.stringify(json));

    Dialog.confirm("确定要退出系统吗", function () {
        $.ajax({
            //url: Common.GetAppPath() + "Bdc/Service/BWRZService.asmx/AddLog",
            type: "POST",
            //data: { 'do': '用户注销' },
            url: dbUrl,
            data: {
                interfaceName: "antu.bdcdj.xtgl.provider.XtglProvider",
                methodName: "addLog",
                args: [Common.GetCookie("token"), strison]
            },
            dataType: "json",
            async: false,
            success: function (data) {
                return data.result;
            },
            error: function (err) {
                top.Dialog.alert("记录用户操作日志错误!");
            }
        });
        var uid = Common.GetCookie("userid");
        // Global.Logout();  这里的业务逻辑是取到后台删除 session 暂时不做
        Common.RemoveCookie("token");
        Common.RemoveCookie("misid");
        Common.RemoveCookie("userid");
        Common.RemoveCookie("menu");
        Common.RemoveCookie("username");
        Common.RemoveCookie("f_site_id");
        Common.RemoveCookie("cxHeadMenu");
        main.ExitNetoffice(uid);
        var res = Common.GetCookie("loginFromCx");
        window.location.replace("Login.html");
    });
};

//强制退出 用于ca拔出
main.coerceExit = function () {
    $.ajax({
        url: Common.GetAppPath() + "Bdc/Service/BWRZService.asmx/AddLog",
        type: "post",
        data: { 'do': '权籍系统用户注销' },
        dataType: "json",
        async: false,
        success: function (data) {
            return data;
        },
        error: function (err) {
            top.Dialog.alert("记录用户操作日志错误!");
        }
    });
    var uid = Common.GetCookie("userid");
    Global.Logout();
    Common.RemoveCookie("token");
    Common.RemoveCookie("misid");
    Common.RemoveCookie("userid");
    Common.RemoveCookie("menu");
    Common.RemoveCookie("username");
    Common.RemoveCookie("f_site_id");
    Common.RemoveCookie("cxHeadMenu");
    main.ExitNetoffice(uid);
    var res = Common.GetCookie("loginFromCx");
    window.location.replace("Login.html");
};
main.ExitNetoffice = function (userid) {
    var sendObj = {};
    sendObj.action = "loginout";
    sendObj.userid = userid;
    var url =Global.GetNetowsConfig() + "rest/Login.ashx";
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
            alert(result);
        }
    });
};
main.GetDay = function () {
    var weekDayLabels = new Array("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六");
    var now = new Date();
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate();
    var currentime = year + "年" + month + "月" + day + "日 " + weekDayLabels[now.getDay()];
    $("#thisday").html(currentime);
};

//菜单打开方式
main.MenuOpenStaut = function () {
    var username = main.refreshSession();
    if (!!Common.GetCookie("userid") && username != "") {
        // $("#frmleft").attr("src", "");
        main.LoadTab();
        main.menutype = "2";
        return;
    }
    //var userid = Global.UserInfo("userid");
    var userid = Common.GetCookie("userid")
    if (userid && userid != "") {
        var json = Global.BussinessHandle("UserSetHandle", "UserSet", userid);
        if (json != "[]") {
            var myjson = json.toJson();
            var openstype = myjson[0].openstype;
            if (openstype) {
                main._openstype = openstype;
            }
        }
        else {
            main._openstype = "129009001";
        }

        if (main._openstype == "129009001") {//多选
            $("#frmleft").attr("src", "");
            main.LoadTab();
            main.menutype = "2";


        }
        //            else if (main._openstype == "129009002") {//单选
        //                main.menutype = "1";
        //                $("#frmleft").show();
        //                $("#frmleft").width("200px");
        //            }
        else if (main._openstype == "129009003") {//桌面
            window.location.href = Common.GetRootPath() + "/Views/Templet/MainDesk.html";
        }
        else if (main._openstype == "129009004") {//手风琴
            $("#frmleft").attr("src", "");
            main.LoadGroup();
        }
        else {
            $("#frmleft").attr("src", "");
            main.LoadTab();
            main.menutype = "2";
        }
    }
    else {
        window.location.href = "Login.html";
    }
};
main.LoadGroup = function () {
    $("#outlookmenu").show();
    // $("#frmleft").remove();
    $("#rboxmore").hide();
    // $("#scrollContent").show();
    $("#scrollContentall").hide();
    $("#scrollContentGroup").show();
    var rootpath = Common.GetRootPath() + "/";
    var userid = Global.UserInfo("userid");
    var _znodes = Global.BussinessHandle("MenuHandle", "MenuFactory", userid).toJson();
    var zindex = 0;
    var zfirstid = "";
    if (_znodes) {
        $(_znodes).each(function (i, col) {
            var menu_name = col.menu_name;
            var menu_parentid = col.menu_parentid;
            var menu_id = col.menu_id;
            var menu_url = col.menu_url;
            if (menu_parentid == "0") {//一级菜单
                zindex++;
                $("<div class='subtitle' style='cursor:pointer' sid='" + menu_id + "' ><div class='subtitle_con'>" + menu_name + "</div></div><ul id='" + menu_id + "'></ul>").appendTo($("#scrollContentGroup"));
                if (zindex == 1) {
                    zfirstid = menu_id;
                }
            }
            else if (menu_parentid != "-1") {//二级菜单
                $("<li ><a onclick='main.MenuClick(this)' href='" + menu_url + "' target='frmright'><span class='text_slice'>" + menu_name + "</span></a></li>").appendTo($("#" + menu_parentid));
            }
        });
    }

    main.HiddenAll();
    $("#" + zfirstid).show();
    $(".subtitle").click(
        function () {
            main.HiddenAll();
            var sid = $(this).attr("sid");
            if ($("#" + sid).is(":hidden")) {
                $("#" + sid).show();
            }
            else {
                $("#" + sid).hide();
            }
        });
};
main.HiddenAll = function () {
    $("ul").hide();
};

main.MenuClick = function (obj) {
    //$("#frmright").attr("src", obj.href);
};

main.LoadTab = function () {
    $("#rbox").hide();
    //$("#frmleft").hide();
    $("#rboxmore").show();
    //$("#scrollContent").show();
    //$("#scrollContentall").show();
    var rootpath = Common.GetRootPath() + "/";
    var userid = Common.GetCookie("userid") || Global.UserInfo("userid");
    // var openCateRest =getConfig("openCateRest");
    //todo
    var openCateRest ="1";
    if (openCateRest && openCateRest == "1" && CateRest.token && CateRest.token != "") {
        var _znodes = CateRest.UserMenu();
        $.fn.zTree.init($("#treeDemo"), main.settingMore, _znodes);
        main.initTab();
    }
    else {
        var _znodes = Global.BussinessHandle("MenuHandle", "MenuFactory", userid).toJson();
        //var _znodes = Global.TableData("sv_user_menu", "*", " and userid='" + userid + "'  and menu_id  <> '0'  order by menu_sort").toJson();
        var zNodes1 = [];
        if (_znodes) {
            $(_znodes).each(function (i, col) {
                var _icon = col.icon;
                if (_icon != "") {
                    _icon = rootpath + "Theme/Images/Icon/16/" + col.icon;
                }
                if (col.menu_parentid != "-1") {
                    var _c = { id: col.menu_id, parentId: col.menu_parentid, target: col.menu_target, name: col.menu_name, pageUrl: col.menu_url, open: col.open == "1", icon: _icon };
                    zNodes1.push(_c);
                }
            });
        }
        $.fn.zTree.init($("#treeDemo"), main.settingMore, zNodes1);
        main.initTab();
    }


};

//多选项卡
main.initTab = function () {
    main.tab = new TabView({
        containerId: 'tab_menu',
        pageid: 'page',
        cid: 'tab1',
        position: "top"
    });
    $("body").bind("dynamicTabActived", function (e, tabId) {
        if ($("#" + tabId).length <= 0) return;
        var tabTtitle = $("#" + tabId)[0].innerText;
        $("#dv_position").html("当前位置：主页>>" + tabTtitle);
    });

    main.tab.add({
        id: 'tab1_index1',
        title: "首  页   ",
        // url: "open.html",
        url: "MainShade.html",
        isClosed: false
    });
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
    var id = $.query.get("tid");
    if (id) {

        var node = treeObj.getNodeByParam("id", id); //获取id为1的点

        main.treeOnClick(null, "main.treeOnClick", node);
    }

};
main.treeOnClick = function (e, treeId, treeNode) {
    var userid = Common.GetCookie("userid") || Global.UserInfo("userid");
    top.activeNode = treeNode;
    if (userid && userid != "") {
        if (treeNode.pageUrl != null && treeNode.pageUrl != "") {
            Common.Loading(true);

            var IsReLoadTab =getConfig("IsReLoadTab");
            if (IsReLoadTab == "1") {
                main.tab.close(treeNode.id);
            }
            if (treeNode.target == "1") {
                if (Common.Lower(treeNode.pageUrl).indexOf("http") < 0) {
                    //                    var msg = Common.FileExist(treeNode.pageUrl);
                    //                    if(msg == "0")
                    //                    {
                    //                        window.open("../"+treeNode.pageUrl);
                    //                    }
                    //                    else
                    //                    {
                    //                        window.open(treeNode.pageUrl);
                    //                    }
                    window.open(Common.GetAppPath() + treeNode.pageUrl);
                }
                else {
                    window.open(treeNode.pageUrl);
                }
            }
            else {
                if (Common.Lower(treeNode.pageUrl).indexOf("http") < 0) {
                    //                    if(Common.FileExist(treeNode.pageUrl))
                    //                    {
                    //                        //调用方法弹出tab
                    //                        main.tabAddHandler(treeNode.id, treeNode.name, treeNode.pageUrl);
                    //                    }
                    //                    else
                    //                    {                
                    //                        main.tabAddHandler(treeNode.id, treeNode.name,"../"+ treeNode.pageUrl);
                    //                    }

                    main.tabAddHandler(treeNode.id, treeNode.name, Common.GetAppPath() + treeNode.pageUrl);
                }
                else {
                    main.tabAddHandler(treeNode.id, treeNode.name, treeNode.pageUrl);
                }
            }
        }
    }
    else {
        window.location.href = "login.html";
    }
};
main.settingMore = {
    callback: {
        onClick: main.treeOnClick
    }
}
main.tabAddHandler = function (mid, mtitle, murl) {
    //var rpath = Common.GetRootPath();
    //    var msg =  Common.FileExist(murl);
    //        var msg = "0";
    //        if (murl.toLocaleLowerCase().indexOf("views") < 0) {
    //            msg = "0";
    //        }


    //        var fpath =getConfig("FilePath");
    //        if (msg == "0" && murl.indexOf("http") == -1) {
    //            murl = Common.GetRootPath().replace("/FrameWork", "") + murl.replace("../../", "");
    //        }
    var tempUrl = murl;
    if (tempUrl.indexOf("?") > -1) {
        tempUrl += "&p=" + (new Date()).valueOf();
    } else {
        tempUrl += "?p=" + (new Date()).valueOf();
    }
    main.tab.add({
        id: mid,
        title: mtitle,
        url: tempUrl,
        isClosed: true
    });
    main.tab.activate(mid);
    //position
    //top.positionType="simple";
    //top.positionContent="当前位置：主页>>"+mtitle;
    //$("#dv_position").html("当前位置：主页>>"+mtitle);
    try {
        $('#' + mid).contextMenu(main.rightmenu);
        $("#" + mid).bind("click", function a() {
            var treeObj = $.fn.zTree.getZTreeObj("treeDemo1");
            var treeNode = treeObj.getNodeByParam("id", $(this).attr("id"));
            top.activeNode = treeNode;
        });
    }
    catch (e)
    { }

};


main.showAll = function () {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
    treeObj.expandAll(true);
};
main.hideAll = function () {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo")
    treeObj.expandAll(false);
};
main.HomeClick = function () {
    Common.Loading(true);
    if (main._openstype == "129009001") {//多选项卡
        main.tabAddHandler('tab1_index1', '首页', 'open.html')
    }
    else if (main._openstype == "129009002") {//单选项卡
        $("#frmright").attr("src", "open.html");
        jQuery.jCookie('leftTreeNodeId', 'iframehome');
    }
};

//main.FindNodePath = function(path) {      
//   if(path)
//   {
//        path = Common.Decode(path);
//        var params = path.split("?");
//        var url = params[0];
//        var par = "";
//        if(params.length>1)
//        {
//            par = params[1];
//        }
//        var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
//        var node = treeObj.getNodesByParamFuzzy("pageUrl", url);  
//        
//        var id =  $(node).attr("tId");
//        $("#"+id).parent().prev("a").click();
//        treeObj.selectNode(node);
//        $("#"+id).find("a").click(); 
//   }
//    
//};


/*
为left.html调用,20150505 ruir
*/

main.Refresh = function () {
    var iframeWindow = window.frames['page_tab1_index1'];
    iframeWindow.index.Refresh();
};

main.forleftOpenMenu = function (menu_id, menu_name, menu_pageurl) {
    var menu = {
        id: menu_id,
        name: menu_name,
        pageUrl: menu_pageurl
    }
    if (Common.Lower(menu.pageUrl).indexOf("http") < 0) {
        if(menu.pageUrl.indexOf("?")>-1){
            menu.pageUrl += "&cdname="+Common.EncodeAscii(menu_name);
        }else{
            menu.pageUrl += "?cdname="+Common.EncodeAscii(menu_name);
        }
        main.tabAddHandler(menu.id, menu.name, Common.GetAppPath() + menu.pageUrl);
    }
    else {
        if(menu.pageUrl.indexOf("?")>-1){
            menu.pageUrl += "&cdname="+Common.EncodeAscii(menu_name);
        }else{
            menu.pageUrl += "?cdname="+Common.EncodeAscii(menu_name);
        }
        window.open(menu.pageUrl);
        //main.tabAddHandler(menu.id, menu.name, menu.pageUrl);
    }
    UpdatetabCss(menu_id);
};

//在执行完添加tab项后执行该方法调整样式
function UpdatetabCss(menu_id) {
    //根据表格的id来判断是否移除选中效果
    $(".tab_item").each(function () {
        if ($(this).attr("id") != menu_id) {
            $(this).removeClass("tab_itemed");
        }
        else {
            $(this).addClass("tab_itemed");
        }
    });
    $(".tabContainer").children(":not(':last')").addClass("tabContainer1");
    //移除原来页面多余的样式
    $(".tab_hr").removeClass('tab_hr');
    //移除原来多余的td样式
    $(".benma_ui_tab").css({"height": "48px"});
    $(".tab_close").css({"top": "15px"});
    $(".tab_item > tbody > tr > td").removeClass();
    //选择第二个td然后动态添加样式
    var $td2 = $(".tab_item > tbody > tr").children('td:nth-child(2)');
    $td2.css({"height": "41px", "width": "108px", "box-sizing": "border-box", "text-align": "center", "vertical-align": "middle"});
    $(".tab_close").css({"top": "15px"});
    //左侧导航栏对应选中的页面高亮
    changeNavigationStyle(menu_id);
    $(".tab_item").click(function () {
        if ($(this).attr('id') == 'tab1_index1') {
            $('.page').css("background-color", "#f0f2f5")
        } else {
            $('.page').css("background-color", "#fff")
        }

        var menu_id1 = $(this).attr("id");
        //左侧导航栏对应选中的页面高亮
        changeNavigationStyle(menu_id1);
        //移除原来页面多余的样式
        $(".benma_ui_tab").css({"height": "48px"});
        $(".tab_hr").removeClass('tab_hr');
        //移除原来多余的td样式
        $(".tab_item > tbody > tr > td").removeClass();
        //选择第二个td然后动态添加样式
        var $td2 = $(".tab_item > tbody > tr").children('td:nth-child(2)');
        $td2.css({"height": "41px", "width": "108px", "box-sizing": "border-box", "text-align": "center", "vertical-align": "middle"});

        $(".tab_item").each(function () {
            if ($(this).attr("id") != menu_id1) {
                $(this).removeClass("tab_itemed");
            }
            else {
                $(this).addClass("tab_itemed");
            }
        });

        $(".tab_close").css({"top": "15px"});
    });
}
/*
使系统Session永不过期,yhd 20150518
*/
main.refreshSession = function () {
    // var explorerName=main.getExplorer();
    // if(explorerName=="IE"){
    //     ifrmautorefresh.window.location.reload();
    // }else{
    //     ifrmautorefresh.window.location=ifrmautorefresh.window.location;
    // }
    sessionOut++;
    //todo
//     var loginUrl = Common.GetAppPath() + "Bdc/Service/Services.asmx/HelloWorld";
//     var result = '';
//     $.ajax({
//         type: "POST",
//         contentType: "application/json",
//         url: loginUrl,
//         data: '{}',
//         dataType: 'json',
//         async: false,
//         error: function (message) {
//             //alert(message.responseText);
//         },
//         success: function (data) {
//             if (!data) {
//
//             } else {
//                 result = data.d;
//             }
//         }
//     });
    var uiusername = $("#username").html();
    //todo
    //替代上面的ajax请求
    var result =  Common.GetCookie("username");

    if (result == '' || (uiusername != "" && result != uiusername)) {
        clearInterval(handleInterval);
        top.Dialog.alert("用户没有登录或用户会话已丢失，请重新登录!本次会话持续" + sessionOut + "分钟", function () {
            window.location.replace("Login.html");
        });
    }

    return result;
};
//todo
main.getExplorer = function () {
    var explorer = window.navigator.userAgent;
    //ie
    if (explorer.indexOf("MSIE") >= 0) {
        return "IE";
    }
    //firefox
    else if (explorer.indexOf("Firefox") >= 0) {
        return "Firefox";
    }
    //Chrome
    else if (explorer.indexOf("Chrome") >= 0) {
        return "Chrome";
    }
    //Opera
    else if (explorer.indexOf("Opera") >= 0) {
        return "Opera";
    }
    //Safari
    else if (explorer.indexOf("Safari") >= 0) {
        return "Safari";
    }
};
//logo上传层
main.logoTab = function () {
    var a;
    var diag = new top.Dialog();
    diag.Title = "修改项目logo";
    diag.Width = 800;
    diag.Height = 350;
    diag.ButtonAlign = "center"; /// <reference path="../../LogoTab.html" />

    diag.URL = "LogoTab.html";
    diag.OKEvent = function () {
        a = diag.innerFrame.contentWindow.BtnOk();
        if (a) {
            top.Dialog.alert("LOGO更新成功！重启浏览器后生效");
            diag.close();
        }
        else {
            top.Dialog.alert("没有更新LOGO");
        }
    };
    diag.show();
};

function init() {
    HTiasClientObj.SetDevEvent();
}
function RemoveStatusAlert(Flag) {
    if (Flag == 1) {
        //alert("KEY已经插入")
    }
    else if (Flag == 2) {
        //alert("KEY已经拔出")
        top.Dialog.alert("KEY已经拔出,用户会话结束，请重新登录!", function () {
            var uid = Common.GetCookie("userid");
            Global.Logout();
            Common.RemoveCookie("token");
            Common.RemoveCookie("misid");
            Common.RemoveCookie("userid");
            Common.RemoveCookie("menu");
            Common.RemoveCookie("username");
            Common.RemoveCookie("f_site_id");
            Common.RemoveCookie("cxHeadMenu");
            main.ExitNetoffice(uid);
            window.location.replace("Login.html");
        });
    }
}
