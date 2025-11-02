
//从右往左去空格
function rtrim(stringObj) {
    while (stringObj.charCodeAt(stringObj.length - 1) == 32) {
        stringObj = stringObj.substring(0, stringObj.length - 1);
    }
    return stringObj;
}

//从左往右去空格
function ltrim(stringObj) {
    while (stringObj.charCodeAt(0) == 32) {
        stringObj = stringObj.substring(1, stringObj.length);
    }
    return stringObj;
}

//去掉字符串左右两边的空格
function trim(stringObj) {
    return(ltrim(rtrim(stringObj)));
}

/*****************************************************
 *  函数名称：replaceChars;
 *  函数功能：替换字符串中的相应字符串;
 *******************************************************/
function replaceChars(entry, orgStr, replaceStr) {
    temp = "" + entry;
    while (temp.indexOf(orgStr) > -1) {
        pos = temp.indexOf(orgStr);
        temp = "" + (temp.substring(0, pos) + replaceStr + temp.substring((pos + orgStr.length), temp.length));
    }

    return temp;
}

function FilePathAddSufix(path, sufix) {

    var idx = path.lastIndexOf(".");
    if(idx != -1)
    {
        return path.substr(0, idx) + sufix + path.substr(idx);
    }
    else
    {
        return path + sufix;
    }
}

function InsertObjectNode(webCore, sObjName, sClassid) {
    if(webCore == 1)
    {
        document.writeln("<OBJECT id=\"" + sObjName + "\" classid=\"CLSID:" + sClassid + "\" style=\"display:none\"></OBJECT>");
    }else{
        document.writeln("<OBJECT id=\"" + sObjName + "\" TYPE=\"application/gdca-activex\" clsid=\"{" + sClassid + "}\" WIDTH=\"0\" HEIGHT=\"0\"></OBJECT>");
    }
}

function LoadComObj(sObjName, sClassid) {
    var browser = navigator.appName;

    //判断浏览器类型选择合适的ActiveX控件调用方式
    if (browser == "Microsoft Internet Explorer")  //IE浏览器
    {
        InsertObjectNode(1, sObjName, sClassid);
    }
    else if (browser == "Netscape")  //IE11, Chrome,Firefox浏览器
    {
        if (navigator.userAgent.search("Trident") != -1) //IE11
            InsertObjectNode(1, sObjName, sClassid);
        else
            InsertObjectNode(2, sObjName, sClassid);
    }
}

/**
 * Created with JetBrains WebStorm.
 * User: Saturn
 * Date: 12-7-3
 * Time: 下午3:57
 * To change this template use File | Settings | File Templates.
 */
/*
 * MAP对象，实现MAP功能
 *
 * 接口：
 * size()     获取MAP元素个数
 * isEmpty()    判断MAP是否为空
 * clear()     删除MAP所有元素
 * put(key, value)   向MAP中增加元素（key, value)
 * remove(key)    删除指定KEY的元素，成功返回True，失败返回False
 * get(key)    获取指定KEY的元素值VALUE，失败返回NULL
 * element(index)   获取指定索引的元素（使用element.key，element.value获取KEY和VALUE），失败返回NULL
 * containsKey(key)  判断MAP中是否含有指定KEY的元素
 * containsValue(value) 判断MAP中是否含有指定VALUE的元素
 * values()    获取MAP中所有VALUE的数组（ARRAY）
 * keys()     获取MAP中所有KEY的数组（ARRAY）
 *
 * 例子：
 * var map = new Map();
 *
 * map.put("key", "value");
 * var val = map.get("key")
 * ……
 *
 */
function Map() {
    this.elements = [];

    //获取MAP元素个数
    this.size = function () {
        return this.elements.length;
    };

    //判断MAP是否为空
    this.isEmpty = function () {
        return (this.elements.length < 1);
    };

    //删除MAP所有元素
    this.clear = function () {
        this.elements = [];
    };

    //向MAP中增加元素（key, value)
    this.put = function (_key, _value) {
        this.elements.push({
            key:_key,
            value:_value
        });
    };

    //删除指定KEY的元素，成功返回True，失败返回False
    this.remove = function (_key) {
        var bln = false;
        try {
            for (i = 0; i < this.elements.length; i++) {
                if (this.elements[i].key == _key) {
                    this.elements.splice(i, 1);
                    return true;
                }
            }
        } catch (e) {
            bln = false;
        }
        return bln;
    };

    //获取指定KEY的元素值VALUE，失败返回NULL
    this.get = function (_key) {
        try {
            for (i = 0; i < this.elements.length; i++) {
                if (this.elements[i].key == _key) {
                    return this.elements[i].value;
                }
            }
        } catch (e) {
            return null;
        }
    };

    //获取指定索引的元素（使用element.key，element.value获取KEY和VALUE），失败返回NULL
    this.element = function (_index) {
        if (_index < 0 || _index >= this.elements.length) {
            return null;
        }
        return this.elements[_index];
    };

    //判断MAP中是否含有指定KEY的元素
    this.containsKey = function (_key) {
        var bln = false;
        try {
            for (i = 0; i < this.elements.length; i++) {
                if (this.elements[i].key == _key) {
                    bln = true;
                }
            }
        } catch (e) {
            bln = false;
        }
        return bln;
    };

    //判断MAP中是否含有指定VALUE的元素
    this.containsValue = function (_value) {
        var bln = false;
        try {
            for (i = 0; i < this.elements.length; i++) {
                if (this.elements[i].value == _value) {
                    bln = true;
                }
            }
        } catch (e) {
            bln = false;
        }
        return bln;
    };

    //获取MAP中所有VALUE的数组（ARRAY）
    this.values = function () {
        var arr = [];
        for (i = 0; i < this.elements.length; i++) {
            arr.push(this.elements[i].value);
        }
        return arr;
    };

    //获取MAP中所有KEY的数组（ARRAY）
    this.keys = function () {
        var arr = [];
        for (i = 0; i < this.elements.length; i++) {
            arr.push(this.elements[i].key);
        }
        return arr;
    }
}

function setOpacity(obj, opacity) { 
	if (obj.style.opacity != undefined) { 
		obj.style.opacity = opacity;  ///兼容FF和GG和新版本IE 
	} else { 
		obj.style.filter = "alpha(opacity=" + opacity * 100 + ")";  ///兼容老版本ie 
	} 
}

var is_fading = false;

//淡入
function fadein(obj, interval, precision) {
	var num = 0;
    if (!is_fading) {
        var st = setInterval(function(){
            num++;
            is_fading = true;
            setOpacity(obj, num / precision);
            if (num >= precision) {
                clearInterval(st);
                is_fading = false;
            }
        }, interval);
    }
}

 //淡出
function fadeout(obj, interval, precision, cb_end) {
	var num = precision;
    if (!is_fading) {
        var st = setInterval(function(){
            num--;
            is_fading = true;
            setOpacity(obj, num / precision);
            if (num <= 0) {
                clearInterval(st);
                is_fading = false;
                cb_end();
            }
        }, interval);
    }
}

function OnFadoutEnd() {
	var msg_wrap=document.getElementById('msg_wrap');
	msg_wrap.style.display="none";
}

//弹出一个淡入淡出的提示框, stay为停留时间
function show_msg(text, stay) {
	var msg_wrap=document.getElementById('msg_wrap');
	var msg=document.getElementById('msg');
	var old_ie = false;

	if (navigator.appName == "Microsoft Internet Explorer")  //IE浏览器
	{
		old_ie = true;
	}

	msg_wrap.style.display="inline-block";
	msg.innerHTML=text;

	old_ie?fadein(msg, 30, 20):fadein(msg_wrap, 30, 20);
		
	setTimeout(function(){
		old_ie?fadeout(msg, 30, 20, OnFadoutEnd):fadeout(msg_wrap, 30, 20, OnFadoutEnd)
	}, stay);
}

function get_radio_val(radio_name) {
	var vals = document.getElementsByName(radio_name);  
    for(var i = 0;i < vals.length;i++)  {
        if(vals[i].checked==true) {
            return vals[i].value;  
        }  
    }
    return null;
}

function genRandString(len) {
　　len = len || 32;
　　var $chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890';
　　var maxPos = $chars.length;
　　var s = '';
　　for (i = 0; i < len; i++) {
　　　　s += $chars.charAt(Math.floor(Math.random() * maxPos));
　　}
　　return s;
}