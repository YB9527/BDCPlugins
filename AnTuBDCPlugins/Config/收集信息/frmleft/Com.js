// JScript 文件
function resizeWin() {
    var width = screen.availWidth;
    var height = screen.availHeight;

    try {
        self.moveTo(0, 0);
        self.resizeTo(screen.availWidth, screen.availHeight);
    }
    catch (e) { }
}

function switchSysBar(divId, inputHidden) {
    var hidden = document.getElementById(inputHidden);
    var div = document.getElementById(divId);
    if (hidden.value == "hide") {
        hidden.value = "show"
        //document.getElementById("split").src="~/App_Themes/Default/Images/left01.gif"
        div.style.display = "none"
        document.getElementById('Content').className = 'HideContent';
    }
    else {
        hidden.value = "hide"
        //document.getElementById("split").src="~/App_Themes/Default/Images/left02.gif"
        div.style.display = ""
        document.getElementById('Content').className = 'ShowContent';
    }
}

function ShowDeleteConfirm() {
    return confirm('确认要删除吗？');
}

function OpenSet(control) {
    if (control != null) {
        control.disabled = true;
    }
}

//限制输入数字
function inputdig(obj) {
    var key = event.keyCode;
    if ((key > 95 && key < 106) || (key > 47 && key < 60) ||
        (key == 110 && obj.value.indexOf(".") < 0) ||
        (key == 190 && obj.value.indexOf(".") < 0)) {
    }
    else if (key != 8) {
        event.returnValue = false;
    }
}

//只能输入数字和.by liuzhen 2008-10-29
function clearNoNum(obj) {
    obj.value = obj.value.replace(/[^\d.]/g, "");
    obj.value = obj.value.replace(/^\./g, "");
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
}

function onlyNum(obj) {
    obj.value = obj.value.replace(/[^\d]/g, "");
}

//禁止键盘输入 by liuzhen 2008-10-24
function isDigit() {
    return (event.keyCode == 0);
}


//删cookies函数
function delCookie(name) {//为了删除指定名称的cookie，可以将其过期时间设定为一个过去的时间
    var date = new Date();
    date.setTime(date.getTime() - 10000);
    document.cookie = name + "=a; expires=" + date.toGMTString();
}

//写cookies函数  //整理  2008-10-10
function setCookie(name, value)//两个参数，一个是cookie的名子，一个是值
{
    var Days = 30; //此 cookie 将被保存 30 天
    var exp = new Date();    //new Date("December 31, 9998");
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}

//取cookies函数  
function getCookie(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) return unescape(arr[2]); return null;

}


//整理  2008-10-31
var request = {
    queryString: function(val) {
        var uri = window.location.search;//获取浏览器当前的地址
        var re = new RegExp("[\?\&]" + val + "\=([^\&\?]*)", "i");
        var arr = uri.match(re);
        //return (!!arr ? unescape(arr[1]) : null);
        return (!!arr ? decodeURIComponent(arr[1]) : null);
    },
    querySpeString: function(val, url) {
        var uri = url;
        var re = new RegExp("[\?\&]" + val + "\=([^\&\?]*)", "i");
        var arr = uri.match(re);
        //return (!!arr ? unescape(arr[1]) : null);
        return (!!arr ? decodeURIComponent(arr[1]) : null);
    },
    queryStrings: function() {
        var uri = window.location.search;
        var re = /\w*\=([^\&\?]*)/ig;
        var retval = [];
        while ((arr = re.exec(uri)) != null)
            retval.push(arr[0]);
        return retval;
    },
    setQuery: function(val1, val2) {
        var a = this.QueryStrings();
        var retval = "";
        var seted = false;
        var re = new RegExp("^" + val1 + "\=([^\&\?]*)$", "ig");
        for (var i = 0; i < a.length; i++) {
            if (re.test(a[i])) {
                seted = true;
                a[i] = val1 + "=" + val2;
            }
        }
        retval = a.join("&");
        return "?" + retval + (seted ? "" : (retval ? "&" : "") + val1 + "=" + val2);
    }
}

//ASCII 转字符 by JiangPan
function UrlDecode(str) {
    var ascTable = ["%40", "%3a", "%2c"];
    var charTable = ["@", ":", ","];
    for (var i = 0; i < ascTable.length; i++) {
        str = str.replace(new RegExp(ascTable[i], 'g'), charTable[i]);
    }
    return str;
}

//判断子符串是否存在
function contains(string, substr, isIgnoreCase) {
    if (isIgnoreCase) {
        string = String(string).toLowerCase();
        substr = String(substr).toLowerCase();
    }
    var startChar = substr.substring(0, 1);
    var strLen = substr.length;
    for (var j = 0; j < string.length - strLen + 1; j++) {
        if (string.charAt(j) == startChar)//如果匹配起始字符,开始查找
        {
            if (string.substring(j, j + strLen) == substr)//如果从j开始的字符与str匹配，那ok
            {
                return true;
            }
        }
    }
    return false;
}

//转成浮点数 并保留小数位数pos位  2009-05-04
function toFloat(src, pos) {
    return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
}


//调用事件代理传入的方法  2008-11-03
function callFunction(fnc, params, obj, obj1) {
    return fnc(params, obj, obj1);
}

//打开窗口  2009-08-17
function openWindow(url, title, w, h, bResize) {
    w = (w) ? w : "200px";
    h = (h) ? h : "150px";

    window.open(url, title, 'height=' + h + ', width=' + w + ', top=0, left=0,toolbar=no,menubar=no,scrollbars=yes,resizable=' + (bResize == false ? 'no' : 'yes') + ',location=no,status=yes');

}


//打开层窗口  2009-09-04
function openDivWin(url, title, w, h, bResize, bshowClose, divId, closeHandler) {
    var divWinId = "dialogBox";
    var ht = (h) ? h : $(window).height() - 60;
    var wh = (w) ? w : $(window).width() - 60;

    if (divId != undefined && divId != "") {
        divWinId = divId;

    }
    else {

        if ($("#" + divWinId).id == undefined) {
            $(document).append('<div id="' + divWinId + '"></div>');
        }

        $("#" + divWinId).empty();
        if (url != null && url != '') {
            $("#" + divWinId).append('<iframe id="frmBox" frameborder="0" scrolling="auto" src="' + url + '" style=" height:' + ht + 'px;width:' + (wh - 6) + 'px;"></iframe>');
        }
    }

    var args = {
        bgiframe: true,
        height: ht,
        width: wh,
        modal: true,
        draggable: false,
        title: title,
        close: closeHandler == undefined ? function() { $("#" + divWinId).dialog('destroy'); }
        : closeHandler,  // 2009-05-23
        resizable: bResize == undefined ? false : bResize//不允许改变大小
        // buttons: { "关闭": function() { $(this).dialog("close"); }  }
    };

    if (bshowClose)
        args.buttons = { "关闭": function() { $(this).dialog("close"); } };

    $("#" + divWinId).dialog(args, 'open');

    $("#" + divWinId).parent().appendTo($("form:first")); //避免server控件不可用  2009-05-26
}
//复制到剪贴板
function copyToClipboard(txt) {
    if (window.clipboardData) {
        window.clipboardData.clearData();
        window.clipboardData.setData("Text", txt);
    } else if (navigator.userAgent.indexOf("Opera") != -1) {
        window.location = txt;
    } else if (window.netscape) {
        try {
            netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
        } catch (e) {
            alert("被浏览器拒绝！\n请在浏览器地址栏输入'about:config'并回车\n然后将'signed.applets.codebase_principal_support'设置为'true'");
        }
        var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
        if (!clip)
            return;
        var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
        if (!trans)
            return;
        trans.addDataFlavor('text/unicode');
        var str = new Object();
        var len = new Object();
        var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
        var copytext = txt;
        str.data = copytext;
        trans.setTransferData("text/unicode", str, copytext.length * 2);
        var clipid = Components.interfaces.nsIClipboard;
        if (!clip)
            return false;
        clip.setData(trans, null, clipid.kGlobalClipboard);
    }
}

String.prototype.replaceAll = function(reallyDo, replaceWith, ignoreCase) {
    if (!RegExp.prototype.isPrototypeOf(reallyDo)) {
        return this.replace(new RegExp(reallyDo, (ignoreCase ? "gi" : "g")), replaceWith);
    } else {
        return this.replace(reallyDo, replaceWith);
    }
}

//验证日期（年月日；不要时分秒的）格式 by yizhi 2010-11-23
function checkDate(str) {
    var reg = /^(\d+)-(\d{1,2})-(\d{1,2})$/;
    var r = str.match(reg);
    if (r == null) return false;
    r[2] = r[2] - 1;
    var d = new Date(r[1], r[2], r[3]);
    if (d.getFullYear() != r[1]) return false;
    if (d.getMonth() != r[2]) return false;
    if (d.getDate() != r[3]) return false;
    return true;
}

function GetDdlSelectValue(ddlId) {
    var ddl = document.getElementById(ddlId);
    var index = ddl.selectedIndex; //取得当前的选中值
    var value = 0;
    if (index > -1) {
        value = ddl.options[index].value;
    }
    return value;
}

function GetDdlSelectText(ddlId) {
    var ddl = document.getElementById(ddlId);
    var index = ddl.selectedIndex; //取得当前的选中值
    var text = "";
    if (index > -1) {
        text = ddl.options[index].text;
    }
    return text;
}

