	//1、页面初始化时，将模态框添加到页面中指定的div中
    /*$(function (){
  		var modalDiv = $("#modalDiv");//指定的div
  		//modalDiv.empty();
		modalDiv.append(
			'<div class="modal fade" id="myModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false" aria-labelledby="myModalLabel" aria-hidden="true">'
				+'<div class="modal-dialog" style="width:900px!important">'
					+'<div class="modal-content">'
						+'<div class="modal-header" style="height:40px;color:#fff;background:#FF8800">'
							+'<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;'
							+'</button>'
							+'<h5 class="modal-title" id="myModalLabel">请选择你要用的证书'
							+'</h5>'
						+'</div>'
						+'<div class="modal-body">请在列表中选择证书'
							+'<table id="tableList" width="100%" border="0" align="center" cellpadding="0" cellspacing="0">'
							+'</table>'
						+'</div>'
						+'<div class="modal-footer" style="text-align:center">'
							+'<button type="button" class="btn-warning button orange bigrounded" onClick=\"selectCerButton(this)\">确定</button>&nbsp;'
							+'&nbsp;<button type="button" class="btn-default button bigrounded" onClick=\"CloseButton(this)\"data-dismiss="modal">关闭</button>'
						+'</div>'
					+'</div>'
				+'</div>'
			+'</div>'
		);
  	});*/
  	//全局变量
  	var obj = new IWSAgent();
  	var cerDN = null;
  	var PlainText = null;
	var ErrorCode = -1 ;
	var SignData = "" ;
	var CertListObj ;
	var comSign ;
	var SelectSn = "";
	var Selectdn = "";
	var signCallBackFun = null;
    //2、点击签名按钮事件
  	function InfosecAttachedSign(strPlainText,pfun){

		signCallBackFun = pfun;
		ErrorCode = 0;
		PlainText = strPlainText;//原文
  		//modalDiv.empty();
		obj.IWSASetAsyncMode(false);
		var json = [];
		CertListObj = json;
		if (!!window.ActiveXObject || "ActiveXObject" in window){
		//是

			/*if(window.navigator.platform == "Win32")
			{//IE是32位的！
				comSign = new ActiveXObject('NetSign.InfoSecNetSignGM.1');
			}
			else
			{//IE是64位的！
				comSign = new ActiveXObject('NetSign.InfoSecNetSignGM2_64.1');
			}*/

			 certcount = comSign.NSGetAllCertsInfo(1);

			for(var i=0;i<certcount;i++)
			{
				var row1 = {};
				row1.certDN= comSign.NSGetCertInfo_Subject(i);
				row1.issuerDN = comSign.NSGetCertInfo_Issuer(i);
				row1.certSN= comSign.NSGetCertInfo_SN(i);
				row1.validBegin = comSign.NSGetCertInfo_ValidBegin(i);
				row1.validEnd= comSign.NSGetCertInfo_ValidEnd(i);
				json.push(row1);
			}
			CertListObj = json ;

		}else{
		//不是
			obj.IWSASkfGetCertList("SKFAPI20052.dll",buttonAddAllCert_Succeed)   //IWSAGetAllCertsListInfo("","Sign",0);
		}
		//buttonAddAllCert_Succeed(CertListObj);
  	}
	function buttonAddAllCert_Succeed(CertListData){
		var table = $("<TABLE class=\"tableStyle\"></TABLE>");
		table.empty();
		table.append('<tr style="text-align: center;">'
					+'<td></td>'
					+'<td>证书主题</td>'
					+'<td>颁发者</td>'
					+'<td>截止日期</td>'
				+'</tr>'
		);
		Common.SetCookie("CAName",CertListData[0].certDN.split(",")[7].split("=")[1]);
		for(var i=0;i< CertListData.length;i++){//取出证书各项信息
			if(CertListData[i].KeyUsage == "signature")
			{
				CertListData[i].certDN;
				CertListData[i].issuerDN;
				CertListData[i].certSN;
				CertListData[i].validBegin;
				CertListData[i].validEnd;
				CertListData[i].CertStore;
				CertListData[i].KeyUsage;
				CertListData[i].CertType;
				table.append('<tr style="height:40px;text-align:center;">'
					+'<td width="30px">'
					+'<a onclick=\'top.Dialog._dialogArray[top.Dialog._dialogArray.length-1].ParentWindow["buttonAttachedSign_onclick"](' + i +')\'>选中</a>'
					+'</td>'
					+'<td width="50px">'+CertListData[i].certDN+'</td>'
					+'<td width="50px">'+CertListData[i].issuerDN+'</td>'
					+'<td width="50px">'+CertListData[i].validEnd+'</td>'
					+'</tr>'
				);
			}
		}
		top.Dialog.open({
			InnerHtml: "<div>CA密码：<input type='password' id='caPwd' /></div><br/>" +  table[0].outerHTML,
			ParentWindow: window,
			Width:1000,
			Title: "请在列表中选择证书"
		});
	}
	//4、点击确定把选中的证书序列号提交到签名方法
  	function selectCerButton(){
  		var index = $('input[name="index"]:checked').val();//证书序列号
		if(CertListObj.length>index)
		{
			SelectSn = CertListObj[index].certSN;
			Selectdn = CertListObj[index].issuerDN;
		}
  		buttonAttachedSign_onclick(index);

		$('#myModal').modal('hide');
  	}
	function CloseButton()
	{
  		ErrorCode = -20051;
  	}
  	//5、签名方法
	function buttonAttachedSign_onclick(index) {

  		var caPwd = $("#caPwd").val();
        if(!caPwd)
		{
			Dialog.alert("请输入CA密码！" );
			return;
		}

		if(CertListObj.length>index)
		{
			SelectSn = CertListObj[index].certSN;
			Selectdn = CertListObj[index].issuerDN;
		}
		if(typeof(index)=="undefined")
		{
			Dialog.alert("请选择证书!" );
		}else{
			//$('#myModal').modal('hide');

			if (!!window.ActiveXObject || "ActiveXObject" in window)
			{ //是
				if(CertListObj.length>index)
				{
					SelectSn = CertListObj[index].certSN;
					Selectdn = CertListObj[index].issuerDN;
					//comSign.NSSetPlainText(PlainText);
					//SignData = comSign.NSAttachedSignByIssuerSN(SelectSn,Selectdn);
					//ErrorCode = comSign.errorCode;
				}
			}else {//不是
				obj.IWSASetAsyncMode(true);
				//var SigndataOBJ = obj.IWSAAttachedSign(1,PlainText,index,"SHA1");//普通Key Attached签名
				//obj.IWSASkfSignData(PlainText, index, "111111", "SHA256", SignSuccess);
				obj.IWSASkfDetachedSign(PlainText, index,caPwd,"SM3", SignSuccess);
			}
		}
		return;
	}

	function SignSuccess(errorCode,signedData) {
		ErrorCode = errorCode ;
		SignData = signedData;
		if(!!signCallBackFun)
		{
			top.Dialog._dialogArray[top.Dialog._dialogArray.length - 1].close();
			signCallBackFun({});
		}
	}

	//
	function InfosecGetError()
	{
		return ErrorCode ;
	}
	function InfosecGetSignData()
	{
		return SignData ;
	}
	function InfosecGetSelectSn()
	{
		return SelectSn ;
	}
	function InfosecGetSelectDn()
	{
		return Selectdn ;
	}
	function InfosecSetCom(infosecCom)
	{
		comSign = infosecCom;
	}
