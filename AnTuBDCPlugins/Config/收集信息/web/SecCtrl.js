////////////////////////////////////////////////////
//定义相关接口参数
// 非对称算法
var ALGID_AUTO = 0;							// 自动适配
var ALGID_RSA1024 = 1;						// RSA 1024位算法
var ALGID_RSA2048 = 2;						// RSA 2048位算法
var ALGID_SM2 = 3;							// SM2 256位算法

// 摘要算法
var ALGID_HASH_SHA1 = 1;					//SHA-1算法
var ALGID_HASH_SHA256 = 2;  				//SHA-256算法
var ALGID_HASH_SHA512 = 3;  				//SHA-512算法
var ALGID_HASH_MD5 = 4;     				//MD5算法
var ALGID_HASH_MD4 = 5; 					//MD4算法
var ALGID_HASH_SM3 = 6;						//SM3算法

// 对称算法
var ALGID_3DES_ECB = 3;
var ALGID_3DES_CBC = 4;
var ALGID_AES_128_ECB = 5;
var ALGID_AES_128_CBC = 6;
var ALGID_AES_192_ECB = 7;
var ALGID_AES_192_CBC = 8;

// 证书类型定义
var ENCRYPT_TYPE = 1;						//加密证书
var SIGN_TYPE = 2;							//签名证书

// 证书基本项
var X509_CERT_VERSION =	1;					// 证书版本
var X509_CERT_SERIAL =	2;					// 证书序列号
var X509_CERT_SIGNALG =	3;					// 证书签名算法标识
var X509_CERT_ISSUER_C = 4;					// 证书颁发者国家(C)
var X509_CERT_ISSUER_O = 5;					// 证书颁发者组织名(O)
var X509_CERT_ISSUER_OU = 6;				// 证书颁发者部门名(OU)
var X509_CERT_ISSUER_S = 7;					// 证书颁发者所在的省、自治区、直辖市(S)
var X509_CERT_ISSUER_CN = 8;				// 证书颁发者通用名称(CN)
var X509_CERT_ISSUER_L = 9;					// 证书颁发者所在的城市、地区(L)
var X509_CERT_ISSUER_E = 10;				// 证书颁发者Email
var X509_CERT_NOTBEFORE = 11;				// 证书有效期：起始日期
var X509_CERT_NOTAFTER = 12;				// 证书有效期：终止日期
var X509_CERT_SUBJECT_C = 13;				// 证书拥有者国家(C )
var X509_CERT_SUBJECT_O = 14;				// 证书拥有者组织名(O)
var X509_CERT_SUBJECT_OU = 15;				// 证书拥有者部门名(OU)
var X509_CERT_SUBJECT_S = 16;				// 证书拥有者所在的省、自治区、直辖市(S)
var X509_CERT_SUBJECT_CN = 17;				// 证书拥有者通用名称(CN)
var X509_CERT_SUBJECT_L = 18;				// 证书拥有者所在的城市、地区(L)
var X509_CERT_SUBJECT_E = 19;				// 证书拥有者Email
var X509_CERT_ISSUER_DN = 20;				// 证书颁发者DN
var X509_CERT_SUBJECT_DN = 21;				// 证书拥有者DN
var X509_CERT_DER_PUBKEY = 22;				// 证书公钥信息
var X509_CERT_EXT_CRLDISTRIBUTIONPO = 23;	// CRL发布点
var X509_CERT_EXT_AUTHORITYKEYIDENTIFIER_INFO =	24;	// 颁发者密钥标示符
var X509_CERT_EXT_SUBJECTKEYIDENTIFIER_INFO = 25;  // 持有者密钥标示符

var g_SecCtrl = null;

function IsSupportNPAPI(){
	var npapiSupport = false;
	var pp = navigator.plugins;
	for(var i = 0; i < pp.length; i++) {
		if(pp[i].name == "npSecCtrl plugin"){
			npapiSupport = true;
			break;
		}
	}
	return npapiSupport;
}

function GetSecCtrl(){
	if(window.ActiveXObject || "ActiveXObject" in window){
		document.write("<object id=\"SecCtrlCOM\" classid=\"clsid:17F8D3CF-857C-4D7C-9355-7A2398930B6A\" hidden=\"true\" width=\"0\" height=\"0\"></object>");
		g_SecCtrl = SecCtrlCOM;
	}else if(IsSupportNPAPI()){
		document.write("<embed id=\"SecCtrlNP\" type=\"application/npsecctrl-plugin\"  hidden=\"true\" width=\"0\" height=\"0\"/>");
		g_SecCtrl = SecCtrlNP;
	}else{
		g_SecCtrl = new SecCtrlSS();
	}
	return g_SecCtrl;
}

function SecCtrlSS() {
	var _xmlhttp;

	function HttpSend(json) {
		var _ks_url = "http://127.0.0.1:9002/";
		if (_xmlhttp == null) {
			if (window.XMLHttpRequest) {
				_xmlhttp = new XMLHttpRequest();
			} else {
				_xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
			}
		}
		_xmlhttp.open("POST", _ks_url, false);
		_xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
		_xmlhttp.send(json);
	};

	function GetHttpResult() {
		if (_xmlhttp.readyState == 4 && _xmlhttp.status == 200) {
			var obj = eval("(" + _xmlhttp.responseText + ")");
			return obj;
		}
		else {
			return null;
		}
	}

	// 获取版本信息
	this.KS_GetVersion = function(){
		 try {
			var request = '{"F":1}';
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return ret.V;
		}
	}
	
	// 设置算法提供者
	this.KS_SetProv = function(prov, alg, path){
		var request = '{"F":2,"P":{"Prov":"' + encodeURIComponent(prov) +'","Alg":' + alg + ',"Path":"' + encodeURIComponent(path) + '"}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 设置参数
	this.KS_SetParam = function(name, data){
		var request = '{"F":3,"P":{"Name":"' + encodeURIComponent(name) + '","Data":"' + encodeURIComponent(data) + '"}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 获取参数
	this.KS_GetParam = function(name){
		var request = '{"F":4,"P":{"Name":"' + encodeURIComponent(name) + '"}}';
		 try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 获取设备状态
	this.KS_GetDeviceStatus = function(){
		var request = '{"F":5}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 生成密钥对
	this.KS_GenKeyPair = function(){
		var request = '{"F":6}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}

	// 读取证书
	this.KS_GetCert = function(type){
		var request = '{"F":10,"P":{"Type":' + type + '}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 解析证书
	this.KS_GetCertInfo = function(cert, type){
		var request = '{"F":11,"P":{"Cert":"' + encodeURIComponent(cert) + '","Type":' + type + '}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 解析证书
	this.KS_GetCertInfoByOid = function(cert, oid){
		var request = '{"F":12,"P":{"Cert":"' + encodeURIComponent(cert) + '","OID":"' + encodeURIComponent(oid) + '"}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 生成随机数
	this.KS_GenRandom = function(len){
		var request = '{"F":13,"P":{"Len":' + len + '}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 摘要数据
	this.KS_HashData = function(data, alg){
		var request = '{"F":14,"P":{"Data":"' + encodeURIComponent(data) + ',"Alg":' + alg + '}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 摘要文件
	this.KS_HashFile = function(file, alg){
		var request = '{"F":15,"P":{"File":"' + encodeURIComponent(file) + ',"Alg":' + alg + '}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 数据签名
	this.KS_SignData = function(data, alg){
		var request = '{"F":16,"P":{"Data":"' + encodeURIComponent(data) + '","Alg":' + alg + '}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 验证签名
	this.KS_VerifySignData = function(data, signData, cert, alg){
		var request = '{"F":17,"P":{"Data":"' + encodeURIComponent(data) +'","SignData":"' + encodeURIComponent(signData) + '","Cert":"' + encodeURIComponent(cert)  + '","Alg":' + alg + '}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return ret.V;
		}
	}
	
	// 验证P7签名
	this.KS_VerifyP7SignData = function(p7SignData){
		var request = '{"F":18,"P":{"P7SignData":"' + encodeURIComponent(p7SignData) + '"}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 导入P12证书
	this.KS_ImportP12Cert = function(type, p12, pin){
		var request = '{"F":19,"P":{"Type":' + type +',"P12":' + encodeURIComponent(p12) + ',"Pin":"' + encodeURIComponent(pin) + '"}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 对称加密
	this.KS_SymmEncrypt = function(data, key, alg){
		var request = '{"F":21,"P":{"Data":"' + encodeURIComponent(data) +'","Key":"' + encodeURIComponent(key) + '","Alg":' + alg + '}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 对称解密
	this.KS_SymmDecrypt = function(data, key, alg){
		var request = '{"F":22,"P":{"Data":"' + encodeURIComponent(data) +'","Key":"' + encodeURIComponent(key) + '","Alg":' + alg + '}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 组数字信封
	this.KS_MakeEnvelope = function(data, cert){
		var request = '{"F":23,"P":{"Data":"' + encodeURIComponent(data) +'","Cert":"' + encodeURIComponent(cert) + '"}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 解数字信封
	this.KS_OpenEnvelope = function(envelope){
		var request = '{"F":24,"P":{"Envelope":"' + encodeURIComponent(envelope) + '"}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 验证密码
	this.KS_VerifyPin = function(pin){
		var request = '{"F":25,"P":{"Pin":"' + encodeURIComponent(pin) + '"}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 对称加密文件
	this.KS_SymmEncryptFile = function(infile, outfile, key, alg){
		var request = '{"F":29,"P":{"InFile":"' + encodeURIComponent(infile) +'","OutFile":"' + encodeURIComponent(outfile) + '","Key":"' + encodeURIComponent(key)  + '","Alg":' + alg + '}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 对称解密文件
	this.KS_SymmDecryptFile = function(infile, outfile, key, alg){
		var request = '{"F":30,"P":{"InFile":"' + encodeURIComponent(infile) +'","OutFile":"' + encodeURIComponent(outfile) + '","Key":"' + encodeURIComponent(key)  + '","Alg":' + alg + '}}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 获取最后错误码
	this.KS_GetLastErrorCode = function(){
		var request = '{"F":99}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
	
	// 获取最后错误描述
	this.KS_GetLastErrorMsg = function(){
		var request = '{"F":100}';
		try {
			HttpSend(request);
		} catch (e) {
			return -1;
		}
		var ret = GetHttpResult();
		if (ret) {
			return decodeURIComponent(ret.V);
		}
	}
}

var SecCtrl = GetSecCtrl();

