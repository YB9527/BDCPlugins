var causers = [];
var sUser = "";
var sContainer = '';
var sSignCert = '';
var sCertKey = '';
var gdcalog = {};
$GDCA.setProtocol(1);//设置协议方式 1:http 2:https
$GDCA.onDeviceNotify(function (dev_evt) {
	if (dev_evt == DEV_EVENT_ARRIVAL) {
		console.log('设备插入');
		var softdog = Global.GetConfig("softdog");
		var islogin = window.location.href.indexOf('login.html');//是否是登录页
		if (softdog == '1' && islogin > 0) {
			window.location.replace("login.html");
		}
	}
	if (dev_evt == DEV_EVENT_REMOVE) {
		console.log('设备拔出');
		var caout = Global.GetConfig("caout");
		var islogin = window.location.href.indexOf('login.html');//是否是登录页
		if (caout == '1' && islogin < 0) {
			main.coerceExit();//强制退出
		}
	}
});
function caInit() {
	//读取ca用户
	$GDCA.getUserList("0", function (res) {
		causers = res;
		gdcalog.causers = causers;
		getUsers();
	}, function (err) {
		ShowError('读取ca用户', err);
	});
}
//得到用户和容器
function getUsers() {
	if (causers.length > 0) {
		var useinfo = causers[0].split('||');
		gdcalog.useinfo = useinfo;
		sUser = useinfo[0];
		sContainer = useinfo[1];
		sContainer += "_FromSign";
		gdcalog.sContainer = sContainer;
		getSignCert();
	} else {
		console.error('未取到ca用户！');
	}
}
//得到证书
function getSignCert() {
	if (!!sContainer) {
		$GDCA.exportUserCert(sContainer, function (res) {
			sSignCert = res;
			gdcalog.sSignCert = sSignCert;
			getCertKey();
		}, function (err) {
			ShowError('获取证书内容', err);
		});
	}
}
//得到信任服务号
function getCertKey() {
	if (!!sSignCert) {
		var oid_name = '1.2.86.21.1.3';
		$GDCA.getCertInfoByOid(sSignCert, oid_name, function (res) {
			if (oid_name == '1.2.86.21.1.3') res = parse_trustid(res);//1.2.86.21.1.3
			sCertKey = res;
			gdcalog.sCertKey = sCertKey;
			console.info(gdcalog);
			if (!!sCertKey) {
				var cak = $('#cakey');
				if (!!cak) cak.val(sCertKey);
			}

		}, function (err) {
			ShowError('获取证书服务号', err);
		});
	} else {
		console.error('未取到ca证书内容！');
	}
}

function ShowError(step, e) {
	if (e.ErrorMsg != undefined) {
		console.error(step + "处操作错误:消息:" + e.ErrorMsg + ",代码:0x" + e.ErrorCode.toString(16).toUpperCase());
	}
	else
		console.error("Exception:\n" + e.ErrorMsg);
}
function parse_trustid(trustid) {
	if (trustid.substr(0, 2) == '..') return trustid.substr(2);
	return trustid;
}