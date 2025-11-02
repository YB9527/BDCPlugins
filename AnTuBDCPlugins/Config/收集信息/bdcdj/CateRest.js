var dbUrl=  Common.GetBasePath("callProvider");
var CateRest = {
    token : Common.GetCookie("token") || "",
    appkey : Common.GetCookie("appkey") || "",
    userid: Common.GetCookie("userid") || Global.UserInfo("userid") || "",
    catePath : Global.getConfig("UserCateRest") || "",
    misid : Common.GetCookie("misid") || "",
    groupids :  Common.GetCookie("groupids") || "",
    cxmisid:""

};
function ServerData  (fun) {
    var   callback;
  $.ajax({
        type: "POST",
        url: dbUrl,
        async: false,
        data: {
            interfaceName: "antu.atp.provider.user.UserProvider",
            methodName: fun,
            args: [CateRest.token,CateRest.userid,CateRest.cxmisid==""? CateRest.misid:CateRest.cxmisid]
        },
        dataType: 'json',
        success: function(data) {
            if(data.status != -1){
                callback = JSON.parse(data.result);

            }else {
                //top.Dialog.alert(fun+"接口请求错误");
            }
        }
    });
   return callback;
}
CateRest.UserMenu = function() {
  return CateRest.UserMenuByKey();
};
CateRest.UserMenu = function(misId) {
    return CateRest.UserMenuByKey(misId);
};
CateRest.UserMenuByKey = function(misId) {
    if(!!misId)CateRest.cxmisid = misId;
    var menu;
    var menuObj =ServerData ("getUserMenu");
    if(menuObj == "" || menuObj == null || menuObj == undefined){
          return;
    }
    menu = menuObj.MENU;
    menu = '<div>' + menu + '</div>';
    var zNodes = CateRest.UserMenuChildren(menu, "");
    return zNodes;
};
CateRest.UserMenuChildren = function(menu, pid) {
    var rootpath = Common.GetRootPath() + "/";
    var zNodes = [];
    var obj = $(menu).find("menu").children("mi");
    var subObj = null;
        if (pid && pid != "") {
            subObj = $(menu).find("[id='" + pid + "']").children("mi");
        }
        if (subObj && subObj.length > 0) {
        obj = subObj;
            for (var i = 0; i < obj.length; i++) {
                var fobj = $(obj[i]);
                var id = fobj.attr("id");
                var name = fobj.attr("label");
                var icon = fobj.attr("icon");
                var link = fobj.attr("link");

                if (icon != "") {
                    icon = rootpath + "Theme/Images/Icon/16/" + icon;
                }
                var _c = { id: id, parentId: pid, name: name, pageUrl: link, open: true, icon: icon };
                zNodes.push(_c);
            }
        }
        else
        {
        $.each(obj,function(i,o){
             var fobj = $(obj[i]);
                var id = fobj.attr("id");
                var name = fobj.attr("label");
                var icon = fobj.attr("icon");
                var link = fobj.attr("link");

                 if (icon != "") {
                    icon = rootpath + "Theme/Images/Icon/16/" + icon;
                }
                if(link == '')
                {
                    pid = '';
                }
                var _c = { id: id, parentId: pid, name: name, pageUrl: link, open: true, icon: icon };
                if(link == '')
                {
                    pid = id;
                }
                zNodes.push(_c);
                if(fobj.children().length>0)
                {
                    var sNodes = CateRest.UserMenuChildren(menu, id);
                    zNodes = zNodes.concat(sNodes);
                }
        });
        }
//    }
    return zNodes;

};
//todo 在登录的时候数据已经返回
// CateRest.SetUserRole = function() {
//     //todo
//     // var openCateRest = getConfig("openCateRest");
//     var openCateRest = "1";//模拟数据
//     if (openCateRest && openCateRest == "1" && CateRest.token && CateRest.token != "") {
//        // var data = '{"token":"' + CateRest.token + '","misId": "' + CateRest.misid + '", "userId": "' + CateRest.userid + '" }';
//        var menuObj;
//        //todo
//        var menuObj = ServerData("getUseRole");
//        if(menuObj != "" && menuObj != null && menuObj != undefined){
//            var groupids = "";
//            for (var i = 0; i < menuObj.length; i++) {
//                var roleid = menuObj[i].ROLE_ID;
//                groupids += roleid + ";"
//            }
//            if (groupids.length > 0) {
//                groupids = groupids.substr(0, groupids.length - 1);
//            }
//            Common.SetCookie("groupids", groupids, 240);
//        }
//
//     }
// };
CateRest.Role = function (){
    var data =  '{"Token":"'+ CateRest.token +'","Fields": "*", "RoleId": " 1","OrgId":"  ","Name":" " }';
    var menuObj = CateRest.ServerData("/role",data);
    var menu = menuObj.Result;

};
function getCookie(userName){
    if (document.cookie.length>0){
        c_start=document.cookie.indexOf(userName+ "=");
        if (c_start!=-1){
            c_start=c_start + userName.length+1;
            c_end=document.cookie.indexOf(";",c_start);
            if (c_end==-1){
                c_end=document.cookie.length;
            }
            return unescape(document.cookie.substring(c_start,c_end));
        }
    }
    return "";
}
