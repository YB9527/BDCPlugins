	var cwcaClient = new CWCAClient();

    var containerName;
    //获取key数量
	function getKeyCount()
	{
		return 100;
		//return cwcaClient.SOF_GetKeyCountEx();
	}

	//获取证书列表
	function getContainer() {
        var ret = cwcaClient.SOF_GetUserListEx();
        var cerList = ret.split("&&&");
        if (cerList != null && cerList.length > 0) {
            return cerList[0].split("||")[1];
        }
        else if (cerList == null)
            alert("无安装证书用户列表");
        else
            alert("获取证书列表失败,错误码:" + cwcaClient.SOF_GetLastError());
        return "";
    }

	//验证用户密码
	function verifyUserPin(pin)
	{
		var ret = 0;
		var strRes = "";
		containerName = getContainer();
		ret = cwcaClient.SOF_Login(containerName,pin);
		if(ret != true)
		{	
			var lastErr = cwcaClient.SOF_GetLastError();
			if (lastErr == 0)
			{
				var retryCount = cwcaClient.SOF_GetPinRetryCount(containerName);
                top.Dialog.alert("验证密码错误,剩余密码重试次数为：" + retryCount);
			}
			else{
                top.Dialog.alert("验证密码失败,错误码：" + lastErr);
			}
            return false;			
		}
		else
		{
			return true;
		}		
	}

	//p7数据签名
	function p7Sign(inData) {
        return cwcaClient.SOF_SignMessage(1, containerName, inData);
    }

 // 文鼎CA
	var prov = "SKF&EsShecaStdV2EBankCspV2.dll";
	function WENDINGCA(strpassword,plain){
		var alg = ALGID_AUTO; // 自动判断
		var hashAlg = ALGID_AUTO; // 自动判断

		//1、设置UKEY调用环境
		var nRet = SecCtrl.KS_SetProv( "SKF&EsShecaStdV2EBankCspV2.dll", alg, "");
		if(nRet != 0){
			alert("设置算法提供者失败或未安装证书助手");
			return;
		}

		//2、校验UKEY密码
		// strpassword = prikeypwd.value;
		// if(strpassword == ""){
		// 	alert("请输入UKEY密码");
		// 	return;
		// }
		nRet = SecCtrl.KS_VerifyPin(strpassword);
		if(nRet != 0){
			top.Dialog.alert("输入UKEY密码错误");
			return "";
		}
		// 预设密码，免弹密码框
		SecCtrl.KS_SetParam("defaultpin", strpassword);	 // 如果没有这行代码，在调用KS_SignData方法时控件将会弹出密码输入框

		//3、获取UKEY证书内容
		var signCert = SecCtrl.KS_GetCert(2);
		if(SecCtrl.KS_GetLastErrorCode() != 0){
			top.Dialog.alert("请插入含证书的UKEY");
			return "";
		}

		//4、UKEY数字签名
		var signD = SecCtrl.KS_SignData(plain, hashAlg);

		//将以上签名原文（strOrgData）、数字证书（signCert）、签名值（signD）传到后台给到签名验证接口进行验证

		// Cert.value = signCert;
		// Org.value = plain;
		// SignData.value = signD;

		/*
          如需从UKEY中获取用户身份信息，可通过以下函数获取：证书使用者（即用户姓名）、证书序列号和用户身份证号
          如果不需要，以下函数可不需要调用
        */
		//获取证书使用者（即用户姓名）（在证书管理工具的查看证书信息界面可以查看）
		commname = SecCtrl.KS_GetCertInfo(signCert, 17);
		//alert("证书使用者（即用户姓名）：" + commname);

		//获取证书序列号（在证书管理工具的查看证书信息界面可以查看）
		certsn = SecCtrl.KS_GetCertInfo(signCert, 2);
		//alert("证书序列号：" + certsn);

		var yxq = certsn = SecCtrl.KS_GetCertInfo(signCert, 12);

		//获取用户身份证号（在证书管理工具的查看证书信息界面可以查看）
		//返回值中‘SF0’字符以后内容为用户身份证号
		strOID = SecCtrl.KS_GetCertInfoByOid(signCert,"1.2.156.112570.11.205");  //根据证书模板确定OID参数值
		if(SecCtrl.KS_GetLastErrorCode() != 0) {
			top.Dialog.alert("KS_GetCertInfoByOid error：" + SecCtrl.KS_GetLastErrorCode() + "\n" + SecCtrl.KS_GetLastErrorMsg());
			return "";
		}
		//OID.value = strOID;

		//截取唯一标识号'SF0'之后内容，即身份证号
		idno = strOID.substring(strOID.indexOf('@SF')+3);
		//alert("截取["+strOID+"]中SF0之后内容即为身份证号：" + idno);
        var caInfo = {username:commname,caxlh:certsn,idno:idno,qymc:"重庆市嘉圆房地产有限公司",yxq:'20'+yxq};
		return escape(JSON.stringify(caInfo));
	}
	

	
	
	
	
