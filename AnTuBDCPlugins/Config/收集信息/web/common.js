//对Jquery.Common.js扩展 ruir create 20150429
//

//按钮公共入口
function openBdcWin(o) {
    var d = $(o).data("data");
    var filter = $(o).data("filter");
    var st = $(o).data("suspendstatustip");
    var ct = $(o).data("continuestatustip");
    var ywlx = $(o).data("ywlx");
    if (!!d && !!d.url) {
        //弹出页面
        if (d.url.indexOf("/") > -1 || d.url.indexOf(".html") > -1) {
            var url = d.url.replace(/\{detailId\}/i, curDetailId);
            url = url.replace(/\{dybdclx\}/i, (!!allBusunessData.detailName ? Common.Encode(escape(allBusunessData.detailName)) : ""));
            var selectMode = (!!allBusunessData.data.selectmode ? allBusunessData.data.selectmode : "0");
            var customselectmode = "";
            if (!!d.selectmode) {
                selectMode = d.selectmode;
                customselectmode = "1";
            }
            url = url.replace(/\{selectmode\}/i, selectMode);
            var pagenameParam = "pagename=" + Common.EncodeAscii(d.id) + "&selectmode=" + selectMode + "&";
            if (!!st) {
                pagenameParam += "st=" + Common.Encode(escape(st)) + "&";
            }
            if (!!ct) {
                pagenameParam += "ct=" + Common.Encode(escape(ct)) + "&";
            }
            if (!!ywlx) {
                pagenameParam += "ywlx=" + Common.Encode(escape(ywlx)) + "&";
            }
            var parms = url.indexOf("?") > -1 ? "&" + pagenameParam : "?" + pagenameParam;
            if (!filter) {
                filter = '';
            }
            //filter += " and f_site_id=" + Common.GetCookie("f_site_id");
            parms += "WHERE=" + Common.Encode(escape(filter));
            if (typeof selectedOLDIID == 'string' && !!selectedOLDIID) {
                parms += "&SELECTEDOLDIID=" + selectedOLDIID;
            }
            url += parms;
            if(url.indexOf("/") == 0)
            {
                url = url.substr(1);
            }
            Common.OpenTopDialog(Common.GetBasePath(url), d.buttonName, eval(d.callBack), d.popCallBack, null, null, st, ct, null, null, customselectmode);
            // Common.OpenTopDialog(Common.GetAppPath() + url, d.buttonName, eval(d.callBack), d.popCallBack,null,null,st,ct,null,null,customselectmode);
        } else {
            (eval(d.url))(); //如果是方法名-则执行方法来弹出页面
        }
    } else if (!!d.callBack) {
        var cb = eval(d.callBack);
        cb.call(null, o);
    }
}

function initSys() {
    var showApp = [];
    $.requestServer({
        type: "POST",
        url: dbUrl,
        async: true,
        data: {
            interfaceName: "antu.atp.provider.user.UserProvider",
            methodName: "getUserMis",
            args: [CateRest.token, CateRest.userid]
        },
        dataType: 'json',
        success: function (data) {
            if (data.status != -1) {
                var returnObj1 = eval('(' + data.result + ')');
                var returnObj = returnObj1.Result;
                Common.SetCookie("misList", returnObj, 240);  //todo
                if (returnObj) {
                    for (var j = 0; j < ak.length; j++) {
                        for (var i = 0; i < returnObj.length; i++) {
                            if (ak[j] == returnObj[i].APP_KEY) {
                                showApp.push(returnObj[i]);
                                break;
                            }
                        }
                    }
                    for (var s = 0; s < showApp.length; s++) {
                        $("#sysUl").append("<li onclick=\"OpenSys('" + showApp[s].MAIN_URL + "')\"><div class='topimg'><img src='" + showApp[s].ICON
                            + "'/></div><div class='bottomtext'>" + showApp[s].SHORT_NAME + "</div></li>");
                    }
                }
            }
        }
    })
}

var Common = Common || {};
$.extend(Common, {

     getDictionaryDataByCategory: function(category,document) {
    var result = {"list":[]};
    $.ajax({
        type: "POST",
        async: false,
        url: Common.GetBasePath("callProvider"),
        data: {
            interfaceName: "antu.bdcdj.xtgl.provider.CommonsCxProvider",
            methodName: "getDictionaryDataByCategory",
            args: [Common.GetCookie("token"), encodeURIComponent(category)]
        },
        error: function (message) {
            alert(message.responseText);
        },
        success: function (res) {
            if(res.status==0){
                var arr = $.parseJSON(res.result);
                var newArr = [];
                for(var i=0;i<arr.length;i++){
                    newArr.push({
                        "VALUE":arr[i].VALUE?arr[i].VALUE:arr[i].value,
                        "NAME":arr[i].NAME?arr[i].NAME:arr[i].name
                    })
                }
                result.list = newArr;
            }else{
                alert(res.message);
            }
        }
    });
    document.data("data",result);
    document.render();
},
     getDictionaryDataByCategoryAndNote: function(category,note,document) {
    var result = {"list":[]};
    $.ajax({
        type: "POST",
        url: Common.GetBasePath("callProvider"),
        async: false,
        data: {
            interfaceName: "antu.bdcdj.xtgl.provider.CommonsCxProvider",
            methodName: "getDictionaryDataByCategoryAndNote",
            args: [Common.GetCookie("token"),category,note]
        },
        error: function (message) {
            alert(message.responseText);
        },
        success: function (res) {
            if(res.status==0){
                var arr = $.parseJSON(res.result);
                var newArr = [];
                for(var i=0;i<arr.length;i++){
                    newArr.push({
                        "VALUE":arr[i].VALUE?arr[i].VALUE:arr[i].value,
                        "NAME":arr[i].NAME?arr[i].NAME:arr[i].name
                    })
                }
                result.list = newArr;
            }else{
                alert(res.message);
            }
        }
    });
    document.data("data",result);
    document.render();
},
    // 用于数据管理系统
     getDictionaryTreeDataByCategory:function(category,document){
    var result = {"treeNodes":[]};
    $.ajax({
        type: "POST",
        url: Common.GetBasePath("callProvider"),
        async: false,
        data: {
            interfaceName: "antu.bdcdj.xtgl.provider.CommonsCxProvider",
            methodName: "getDictionaryTreeDataByCategory",
            args: [Common.GetCookie("token"),category]
        },
        error: function (message) {
            alert(message.responseText);
        },
        success: function (res) {
            if(res.status==0){
                var arr = $.parseJSON(res.result);
                result.treeNodes = arr;
            }else{
                alert(res.message);
            }
        }
    });
    document.data("data",result);
    document.render();
},
    getDictionaryTreeDataBySjglCategory:function(category,document){
        var result = {"treeNodes":[]};
        $.ajax({
            type: "POST",
            url: Common.GetBasePath("callProvider"),
            async: false,
            data: {
                interfaceName: "antu.bdcdj.xtgl.provider.CommonsCxProvider",
                methodName: "getDictionaryTreeDataBySjglCategory",
                args: [Common.GetCookie("token"),category]
            },
            error: function (message) {
                alert(message.responseText);
            },
            success: function (res) {
                if(res.status==0){
                    var arr = $.parseJSON(res.result);
                    result.treeNodes = arr;
                }else{
                    alert(res.message);
                }
            }
        });
        document.data("data",result);
        document.render();
    },
    // 获取页面参数 ，使用$.query.get();取到的wiid会少一位
    QueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null)
            return unescape(r[2]);
        return "";
    },
    GetBdcPath: function () {
        return this.GetBasePath("Bdc");
    },
    GetHJPath: function () {
        return this.GetBasePath("hj");
    },
    GetChPath: function () {
        return this.GetBasePath("ch");
    },
    GetSjglPath: function () {
        return this.GetBasePath("Sjgl");
    },
    GetMapServerPath:function(){
         return "http://192.168.0.244:8081/onemapV4.1/"; // 注意“/”结尾
    },

    //获取文件模板
    downTemplateFile:function(url,fileName){
      var fd = new FormData();
      fd.append("token",Common.GetCookie("token"));
      fd.append("fileName",fileName);
      var xhr = new XMLHttpRequest();
      xhr.open('post',url);
      xhr.setRequestHeader("token", Common.GetCookie("token"));
      xhr.responseType = 'blob';
      xhr.onload = function (e) {
        if ((xhr.status >= 200 && xhr.status < 300) || xhr.status === 304) {
          var blob = xhr.response;
          var filename = fileName;
          if (window.navigator.msSaveOrOpenBlob) {
            navigator.msSaveBlob(blob, filename);
          } else {
            var a = document.createElement('a');
            blob = new Blob([blob]);
            a.href = URL.createObjectURL(blob);
            a.download = filename;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
          }
        }
      };
      xhr.send(fd);
    },


    GetBasePath: function (rootPath) {
        if(rootPath == "callProvider" || rootPath == "exportFile")
        {
            return "/ANTU.BDCDJ.XTGL.PROVIDER/" + rootPath;
        }
        var root;
        if (!Common.GetAppPath) {
            var strFullPath = window.document.location.href;
            var strPath = window.document.location.pathname;
            var pos = strFullPath.indexOf(strPath);
            var prePath = strFullPath.substring(0, pos);
            var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
            root = prePath + postPath;
            return root + "/" + rootPath ;
        }
        return Common.GetAppPath() + rootPath;
    },
    GetRootPath: function () {
        var strFullPath = window.top.location.href;
        var strPath = window.top.location.pathname;
        var pos = strFullPath.indexOf(strPath);
        var prePath = strFullPath.substring(0, pos);
        return prePath;
    },
    OpenTopDialog: function (url, title, cb, f, o, c, st, ct, w, h, cselectmode) {
        var diag = new top.Dialog();
        diag.Title = title;

        if (url.indexOf("http://") > -1)
            diag.URL = url;
        else
            diag.InnerHtml = url;
        diag.ShowMaxButton = true;
        diag.ShowMinButton = false;
        diag.ShowOkButton = !!o;
        diag.ShowCancelButton = !!c;
        if (w) {
            diag.Width = w;
        }
        if (h) {
            diag.Height = h;
        }
        diag.OKEvent = function () { //diag.CancelEvent  =
            if (!!cb) {
                $("#ReceiveLogInfo").val('');
                var fun = diag.innerFrame.contentWindow;
                var funs = !!f ? f.split(".") : [];
                for (var i = 0; i < funs.length; i++) {
                    if (!!fun)
                        fun = fun[funs[i]];
                }
                fun = !!fun && funs.length > 0 ? fun : diag.innerFrame.contentWindow["getSelectedData"];
                if (fun != undefined) {
                    var d = fun(st, ct);
                    if (d && d.length > 0 && d[0].BSM) {
                        for (var errorIndex = 0; errorIndex < d.length; errorIndex++) {
                            if (d[errorIndex].BSM == "error") {
                                return;
                            }
                        }
                        if (!!cselectmode) {
                            for (var bsmIndex = 0; bsmIndex < d.length; bsmIndex++) {
                                d[bsmIndex].BSM = d[bsmIndex].BSM + "customselectmode";
                            }
                        }
                    }
                    setTimeout(function () {
                        cb.call(null, d);
                        if (!!d && !!d[0]) {
                            if (!!d[0].TIPS) {
                                $("#ReceiveLogInfo").val(d[0].TIPS); //2016-06-17 zhuzq 将提示信息传回绑定到页面控件
                            }
                        }
                    }, 10);
                }
            }
            //            if (top.main && top.main.Refresh)
            //                top.main.Refresh();

            try {
                diag.close();
            } catch (ex) {

            }

        };
        if (title == "权利人编辑") {
            diag.CancelEvent = function () {
                //刷新列表
                receiveQLEREF();
                try {
                    diag.close();
                } catch (ex) {

                }

            };
        }

        diag.show();
        if (!w && !h) {
            diag.max();
        }
        return diag;
    },
    OpenNoButtonDialog: function (url, title, cb, f) {
        var diag = new top.Dialog();
        diag.Title = title;
        diag.URL = url;
        diag.ShowMaxButton = true;
        diag.ShowMinButton = false;
        diag.show();
        diag.max();
    },

    // 小写转大写--用于绑定一表单数据
    lowerToUpper: function (originalObj) {
        var resultObj = [];
        if (typeof originalObj == "object" && originalObj.length != undefined) {
            for (var i = 0; i < originalObj.length; i++) {
                var obj = {};
                for (var o in originalObj[i]) {
                    obj[o.toLocaleUpperCase()] = originalObj[i][o];
                }
                resultObj.push(obj);
            }
        } else {
            var obj = {};
            for (var o in originalObj) {
                obj[o.toLocaleUpperCase()] = originalObj[o];
            }
            resultObj.push(obj);
        }
        return resultObj;
    },
    localeLower: function (originalObj) {
        var resultObj = [];
        if (typeof originalObj == "object" && originalObj.length != undefined) {
            for (var i = 0; i < originalObj.length; i++) {
                var obj = {};
                for (var o in originalObj[i]) {
                    obj[o.toLocaleLowerCase()] = originalObj[i][o];
                }
                resultObj.push(obj);
            }
        } else {
            var obj = {};
            for (var o in originalObj) {
                obj[o.toLocaleLowerCase()] = originalObj[o];
            }
            resultObj.push(obj);
        }
        return resultObj;
    },
    // 删除无用数据
    delUseless: function (obj, arrUseless) {
        var arr = ["__ID", "__PREVID", "__INDEX", "__STATUS", "ROWPOSITION", "PARENTASSET", "ISCHANGED", "__NEXTID"];
        if (arrUseless != undefined && arrUseless.length != undefined)
            arr = arrUseless;
        for (var j = 0; j < arr.length; j++) {
            if (obj[arr[j]] != undefined || obj[arr[j]] == null)
                delete obj[arr[j]];
            if (obj[arr[j].toLowerCase()] != undefined || obj[arr[j].toLowerCase()] == null)
                delete obj[arr[j].toLowerCase()];
        }
    },
    // 是否已经选中
    existSelected: function (id, rows, pk) {
        var isExist = false;
        for (var i = 0; i < rows.length; i++) {
            if (rows[i][pk] == id || rows[i][pk] == id[pk]) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },
    getIdxInArray: function (id, arr, pk) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] != undefined) {
                if (arr[i][pk] == id)
                    return i;
            }
        }
        return -1;
    },
    relTable: {}
    ,
    // 根据IID得到业务类型
    getDetailId: function (iid) {
        var _val = "";
        if (!!Common.relTable[iid]) {
            return Common.relTable[iid];
        }
        $.requestServer({
            type: "POST",
            url: postUrl,
            data: {
                interfaceName: "antu.bdcdj.xtgl.provider.CommonsCxProvider",
                methodName: "getDetailIdByIid",
                args: [Common.GetCookie("token"), iid == null ? "" : iid]
            },
            // contentType: "application/json",
            // url: Common.GetBdcPath() + "/Service/OneFormService.asmx/GetDetailIdByIid",
            // data: '{ iid: "' + iid + '"}',
            dataType: 'json',
            async: false,
            error: function (message) {
                console.error(message.responseText);
            },
            success: function (data) {
                _val = data.result;
                Common.relTable[iid] = _val;
            }
        });
        return _val;
    },
    // 根据IID得到业务类型
    getRefDetailId: function (iid) {
        var _val = "";
        $.requestServer({
            type: "POST",
            url: twoDbUrl,
            data: {
                interfaceName: "antu.bdcdj.xtgl.provider.CommonsCxProvider",
                methodName: "getRefDetailIdByIid",
                args: [Common.GetCookie("token"), iid == null ? "" : iid]
            },
            // contentType: "application/json",
            // url: Common.GetBdcPath() + "/Service/OneFormService.asmx/GetRefDetailIdByIid",
            // data: '{ iid: "' + iid + '"}',
            dataType: 'json',
            async: false,
            error: function (message) {
                console.error(message.responseText);
            },
            success: function (data) {
                _val = data.result;
            }
        });
        return _val;
    },
    //gui-gui控件,data格式化的行数据
    // 格式化QUI选择的数据中包含的无效属性
    formartQuiData: function (gui, rdata) {
        if (rdata.length != undefined) {
            var rd = [];
            for (var i = 0; i < rdata.length; i++) {
                var d = gui.formatRecord(rdata[i], true);
                if (d) {
                    if (d.hasOwnProperty("ParentAsset"))
                        delete d.ParentAsset;
                    if (d.hasOwnProperty("IsChanged"))
                        delete d.IsChanged;
                    if (d.hasOwnProperty("rowPosition"))
                        delete d.rowPosition;
                }

                rd.push(d);
            }
            return rd;
        } else {
            var dFormatRecord = gui.formatRecord(rdata, true);
            if (dFormatRecord) {
                if (dFormatRecord.hasOwnProperty("ParentAsset"))
                    delete dFormatRecord.ParentAsset;
                if (dFormatRecord.hasOwnProperty("IsChanged"))
                    delete dFormatRecord.IsChanged;
                if (dFormatRecord.hasOwnProperty("rowPosition"))
                    delete dFormatRecord.rowPosition;
            }
            return dFormatRecord;
        }

    },
    formartObj: function (obj) {
        var regex1 = /^(\d{4}\-\d{1,2}\-\d{1,2})\s{1}\d{1,2}:\d{1,2}:\d{1,2}$/gi;
        var regex2 = /^\/Date\((-?\d+)\)\/$/gi;

        for (var o in obj) {
            if (obj[o] == null) {
                obj[o] = "";
            } else if (obj[o].toString().indexOf("@") > -1) {
                var reg = /\@\s+/;
                obj[o] = obj[o].toString().replace(reg, "\@");
            }
            else if (regex1.test(obj[o].toString())) {
                if (o != "CFSDSJ" && o != "JCFSDSJ") {
                    obj[o] = obj[o].toString().replace(regex1, function (all, $1) {
                        return $1;
                    });
                }
            } else {
                var objValue = obj[o].toString();
                if (regex2.test(objValue) || objValue.indexOf("/Date(") > -1) {
                    obj[o] = obj[o].toString().replace(regex2, function (all, $1) {
                        if (!isNaN($1)) {
                            try {
                                var date = new Date(parseInt($1, 10));
                                if (date.toString().indexOf("GMT+0800") > 0) {
                                    date = new Date(date.getTime() - 8 * 60 * 60 * 1000);
                                }
                                var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                                var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                                if (o == "CFSDSJ" || o == "JCFSDSJ") {
                                    var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();       //获取当前小时数(0-23)
                                    var min = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();   //获取当前分钟数(0-59)
                                    var sec = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();    //获取当前秒数(0-59)
                                    return date.getFullYear() + "-" + month + "-" + day + " " + hour + ":" + min + ":" + sec;

                                } else {
                                    return date.getFullYear() + "-" + month + "-" + day;
                                }
                            } catch (ex) {
                                return "";
                            }
                        } else {
                            return $1;
                        }
                    });
                }
            }
        }
    },
    formartdate: function (datenumber, showSfm) {
        var regex2 = /^\/Date\((-?\d+)\)\/$/gi;
        var objValue = datenumber;
        if (regex2.test(objValue) || objValue.indexOf("/Date(") > -1) {
            objValue = objValue.toString().replace(regex2, function (all, $1) {
                if (!isNaN($1)) {
                    try {
                        var date = new Date(parseInt($1, 10));
                        if (date.toString().indexOf("GMT+0800") > 0) {
                            date = new Date(date.getTime() - 8 * 60 * 60 * 1000);
                        }
                        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                        if (!!showSfm) {
                            var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();       //获取当前小时数(0-23)
                            var min = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();   //获取当前分钟数(0-59)
                            var sec = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();    //获取当前秒数(0-59)
                            return date.getFullYear() + "-" + month + "-" + day + " " + hour + ":" + min + ":" + sec;
                        }
                        return date.getFullYear() + "-" + month + "-" + day;
                    } catch (ex) {
                        return "";
                    }
                } else {
                    return $1;
                }
            });
        }
        return objValue;
    },
    formatDateTimeyy: function(rowdata, rowindex, value, column){
        return value.split(" ")[0];
    },
    formatDateTimeYMD: function(rowdata, rowindex, value, column){
        if(value.length == 21){
            var res = value.substring(0,19);
        }else{
            return value;
        }
        return res;
    },
    formatDateTimeYMD2: function(rowdata, rowindex, value, column){
         if(!value) return "";
        var date = new Date(value.replaceAll("-","/").split('.')[0]);
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var res = date.getFullYear() + "-" + month + "-" + day;
        return res;
    },
    // 得到一表单数据包含不动产列表和权利人列表
    getOneFormData: function (detailId, bsm, cfxxid, cdetailId) {
        var fsiteId = Common.GetCookie("BELONG_REGION_ID") ? Common.GetCookie("BELONG_REGION_ID") : 0;
        var hasrightRegionId = Common.GetCookie("HASRIGHT_REGION_ID") ? Common.GetCookie("HASRIGHT_REGION_ID") : "";
        // var url = Common.GetBdcPath() + "/Service/OneFormService.asmx/GetOneFormData";
        if (typeof oneFormSelectedBsm == "string" && oneFormSelectedBsm.constructor == String) {
            oneFormSelectedBsm = bsm;
        }
        if (typeof oldysh == "string" && oldysh.constructor == String && oldysh.indexOf("~wxysywsl") > 0) {
            // 如果是微信业务，则带上微信相关的BSM
            bsm = bsm + "&" + oldysh;
        }
        var result = null;
        $.requestServer({
            type: "POST",
            url: twoDbUrl,
            data: {
                interfaceName: "antu.bdcdj.xtgl.provider.CommonsCxProvider",
                methodName: "getOneFormData",
                args: [Common.GetCookie("token"), detailId, bsm, cfxxid, cdetailId, fsiteId, hasrightRegionId]
            },
            async: false,
            dataType: 'json',
            success: function (data) {
                if (data != null) {
                    result = data.result;
                } else {
                    Dialog.alert("没有不动产信息!");
                }
            },
            error: function (ex) {
                var errObj = JSON.parse(ex.responseText);
                Dialog.alert(errObj.Message);
            }
        });
        var resultJson = Global.toJson(result);
        return resultJson;
    },
    // 得到宗地的状态信息（在办，权属，抵押等） 2016-06-24
    getObjStatusData: function (detailId, bsm) {
        // var url = Common.GetBdcPath() + "/Service/OneFormService.asmx/GetObjStatusData";
        var result = null;
        if (!!detailId) detailId = "";
        if (bsm == "" || bsm == null || bsm == undefined || bsm == 'undefined') {
            bsm = "";
        }
        $.requestServer({
            type: "post",
            url: twoDbUrl,
            data: {
                interfaceName: 'antu.bdcdj.xtgl.provider.CommonsCxProvider',
                methodName: 'getObjStatusData',
                args: [Common.GetCookie("token"), detailId, bsm]
            },
            // contentType: "application/json",
            // url: url,
            // data: "{ detailId:'" + detailId + "',bsm:'" + bsm + "'}",
            dataType: 'json',
            async: false,
            success: function (data) {
                if (!!data) {
                    result = data.result;
                }
            },
            error: function (ex) {
                Dialog.alert(ex.responseText);
            }
        });

        return result;
    },
    // 得到不动产列表
    getAsset: function (detailId, bsm) {
        var url = Common.GetBdcPath() + "/Service/OneFormService.asmx/GetAsset";
        var result = null;
        $.requestServer({
            type: "post",
            contentType: "application/json",
            url: url,
            data: "{ detailId:'" + detailId + "',bsm:'" + bsm + "'}",
            dataType: 'json',
            async: false,
            success: function (data) {
                if (data != null) {
                    result = data.d;
                } else {
                    Dialog.alert("没有不动产信息!");
                }
            },
            error: function (ex) {
                Dialog.alert(ex.responseText);
            }
        });
        return result;
    },
    // 合并数组(对象)中指定列-按指定格式
    getJoinFromArrOrObject: function (arr, columnName, delimiter) {
        var result = "";
        var temparr = [];
        var mydelimiter = !!delimiter ? delimiter : ",";
        if (arr.length != undefined) {
            for (var i = 0; i < arr.length; i++) {
                temparr.push(arr[i][columnName]);
            }
            result = temparr.join(mydelimiter);
        } else
            result = arr[columnName];
        return result;
    },
    savetxt: function (url) {
        var a = window.open(url, "_blank", "height=0,width=0,toolbar=no,menubar=no,scrollbars=no,resizable=on,location=no,status=no");
        var ua = window.navigator.userAgent;
        if (ua.indexOf('MSIE ') > 0 || ua.indexOf('Trident/') > 0){//IE 10 or older|| IE11
            a.document.execCommand("SaveAs");//仅ie支持
            a.window.close();
            a.close();
        } else {
            var index = url.lastIndexOf("/");
            var elt = document.createElement('a');
            elt.setAttribute('href', url);
            elt.setAttribute('download', url.substring(index+1,url.length));
            elt.style.display = 'none';
            document.body.appendChild(elt);
            elt.click();
        }
    },
    clearData: function (obj) {//清空已选数据时，清空数据
        if (!obj)
            return;
        if (obj.length !== undefined) {
            for (var i = obj.length - 1; i >= 1; i--) {
                RemoveArrayItem(obj, i);
            }
            arguments.callee(obj[0]);
        } else {
            for (var o in obj) {
                if (o == "dataType" || o == "index")
                    continue;
                delete obj[o];
            }
        }
    },
    //    jqData: function(rowdata, rowindex, value, column) {
    //        if (!value) return "";
    //        var gs = Math.round(column._width / 14);
    //
    //	    var tempvalue = value;
    //	    var len = 0;
    //	    var wLen = 0;
    //	    for (var i = 0; i < tempvalue.length; i++) {
    //		    if (!isNaN(tempvalue[i]))
    //			    wLen += 0.5;
    //		    else
    //			     wLen += 1;
    //		    len++;
    //		    if (wLen >= gs) {
    //			    break;
    //		    }
    //	    }
    //        if (wLen >= gs) {
    //            tempvalue = tempvalue.substring(0, len) + "..";
    //        }
    //        return "<div title='" + value + "'>" + tempvalue + "</div>";
    //    }
    //update by yanmi 2015-08-14 优化长数据自动省略
    jqData: function (rowdata, rowindex, value, column) {
        value = !!value ? value : "";
        return "<div title='" + value + "' style='overflow: hidden; white-space: nowrap; -o-text-overflow: ellipsis; text-overflow: ellipsis;'>" + value + "</div>";
    },
    expendClick: function expendClick(expend, contentId) {
        if ($(expend).hasClass("icon_btn_up")) {
            $("#" + contentId).css("height", "0px");
            $(expend).removeClass("icon_btn_up");
            $(expend).addClass("icon_btn_down");
        } else {
            $("#" + contentId).css("height", "30px");
            $(expend).removeClass("icon_btn_down");
            $(expend).addClass("icon_btn_up");
        }
    },

    // 取ajax返回字符串
    GetResultStr: function (result) {
        if (result && result.result != undefined) result = result.result;
        if (result && result.replace)
            result = result.replace(/^<pre[^>]*>/i, "").replace(/<\/pre>$/i, "");
        return result;
    },

});
var dbUrl=  Common.GetBasePath("callProvider");
var threeDbUrl= Common.GetBasePath("callProvider");
var twoDbUrl=  Common.GetBasePath("callProvider");
var postUrl = Common.GetBasePath("callProvider");
var fourDbUrl = Common.GetBasePath("callProvider");

//不晓得atp的token啥时候过期 先设置存入八小时后过期（传入expire参数8），后续再根据atp token过期时间调整 --20230423 chenxin
Storage.prototype.setCanExpireLocal = (key,value,expire) =>{
    let time = expire * 60 * 1000
    let obj ={
        data: value,
        time: Date.now(),
        expire: time,
    }
    localStorage.setItem(key,JSON.stringify(obj));
}
Storage.prototype.getCanExpireLocal = key =>{
    let val = localStorage.getItem(key);
    if(!val) return val;
    val = JSON.parse(val);
    if(Date.now() > val.time + val.expire){
        localStorage.removeItem(key)
        return 'token过期了';
    }
    return val.data;
}