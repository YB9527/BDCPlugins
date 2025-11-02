/***************************************************************************/
/*                                                                         */
/*  This obfuscated code was created by Javascript Obfuscator Free Version.*/
/*  Javascript Obfuscator Free Version can be downloaded here              */
/*  http://javascriptobfuscator.com                                        */
/*                                                                         */
/***************************************************************************/
var _$_13e8 = ["localhost", "55661", "http://", "PROTOCOL", "SERVICE_HOST", ":", "SERVICE_PORT", "https://", "URL", "encode", "decode", "MAX_PARAM_LEN", "length", "substring", "push", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "random", "", "join", "Deferred", "stringify", "base64Encode", "_cutParam", "_createSN", "callback", "_", "sn", "sort", "Status", "Result", "resolve", "ErrorCode", "ErrorMsg", "reject", "0000", "jsonp", "promise", "fail", "Version", "then", "SOF_GetVersion", "exeFn", "DeviceInfo", "SOF_ExtEnumDevice", "SOF_GetUserList", "SOF_Login", "SOF_ChangePassWd", "RetryCount", "SOF_GetPinRetryCount", "SignMethod", "SOF_GetSignMethod", "UserCert", "SOF_ExportUserCert", "SOF_ExportExChangeUserCert", "Info", "SOF_ValidateCert", "Sign", "SOF_SignData", "SignData", "SOF_SignMessage", "SOF_VerifySignedData", "SOF_VerifySignedMessage", "SOF_GetInfoFromSignedMessage", "EncryptData", "SOF_EncryptData", "DecryptData", "SOF_DecryptData", "SOF_GetCertInfo", "SOF_GetCertInfoByOid", "HashValue", "SOF_HashFile", "SOF_WriteUsrDataFile", "ReadData", "SOF_ReadUsrDataFile", "RandomData", "SOF_GenRandom", "SignedData", "SOF_SignDataXML", "SOF_VerifySignedDataXML", "SOF_GetXMLSignatureInfo", "TimeStamp", "SOF_ExtGMTspGetTime", "SealData", "SOF_ExtGMTspSealTimeStamp", "SOF_ExtGMTspVerifyTimeStamp", "SOF_ExtControl"];
var SOR_OK = 0;
var SOR_UnknownErr = 0X0B000001;
var SOR_IndataLenErr = 0X0B000200;
var SOR_IndataErr = 0X0B000201;
var SOR_InitializeErr = 0X0B000424;
var SOR_NotExportErr = 0X0B000311;
var SGD_DEVICE_SORT = 0x00000201;
var SGD_DEVICE_TYPE = 0x00000202;
var SGD_DEVICE_NAME = 0x00000203;
var SGD_DEVICE_MANUFACTURER = 0x00000204;
var SGD_DEVICE_HARDWARE_VERSION = 0x00000205;
var SGD_DEVICE_SOFTWARE_VERSION = 0x00000206;
var SGD_DEVICE_STANDARD_VERSION = 0x00000207;
var SGD_DEVICE_SERIAL_NUMBER = 0x00000208;
var SGD_DEVICE_SUPPORT_ALG = 0x00000209;
var SGD_DEVICE_SUPPORT_SYM = 0x0000020A;
var SGD_DEVICE_SUPPORT_HASH_ALG = 0x0000020B;
var SGD_DEVICE_SUPPORT_STORAGE_SPACE = 0x0000020C;
var SGD_DEVICE_SUPPORT_FREE_SPACE = 0x0000020D;
var SGD_DEVICE_RUNTIME = 0x0000020E;
var SGD_DEVICE_USED_TIMES = 0x0000020F;
var SGD_DEVICE_LOCATION = 0x00000210;
var SGD_DEVICE_DESCRIPTION = 0x00000211;
var SGD_DEVICE_MANAGER_INFO = 0x00000212;
var SGD_DEVICE_MAX_DATA_SIZE = 0x00000213;
var SGD_CERT_ALL = 0x00000000;
var SGD_CERT_VERISON = 0x00000001;
var SGD_CERT_SERIAL = 0x00000002;
var SGD_CERT_ISSUER = 0x00000005;
var SGD_CERT_VALID_TIME = 0x00000006;
var SGD_CERT_SUBJECT = 0x00000007;
var SGD_CERT_DER_PUBLIC_KEY = 0x00000008;
var SGD_CERT_DER_EXTENSIONS = 0x00000009;
var SGD_EXT_AUTHORITYKEYIDENTIFIER_INFO = 0x00000011;
var SGD_EXT_SUBJECTKEYIDENTIFIER_INFO = 0x00000012;
var SGD_EXT_KEYUSAGE_INFO = 0x00000013;
var SGD_EXT_PRIVATEKEYUSAGEPERIOD_INFO = 0x00000014;
var SGD_EXT_CERTIFICATEPOLICIES_INFO = 0x00000015;
var SGD_EXT_POLICYMAPPINGS_INFO = 0x00000016;
var SGD_EXT_BASICCONSTRAINTS_INFO = 0x00000017;
var SGD_EXT_POLICYCONTRAINTS_INFO = 0x00000018;
var SGD_EXT_EXTKEYUSAGE_INFO = 0x00000019;
var SGD_EXT_CRLDISTRIBUTIONPOINTS_INFO = 0x0000001A;
var SGD_EXT_NETSCAPE_CERT_TYPE_INFO = 0x0000001B;
var SGD_EXT_SELFDEFINED_EXTENSION_INFO = 0x0000001C;
var SGD_CERT_ISSUER_CN = 0x00000021;
var SGD_CERT_ISSUER_O = 0x00000022;
var SGD_CERT_ISSUER_OU = 0x00000023;
var SGD_CERT_SUBJECT_CN = 0x00000031;
var SGD_CERT_SUBJECT_O = 0x00000032;
var SGD_CERT_SUBJECT_OU = 0x00000033;
var SGD_CERT_SUBJECT_EMAIL = 0x00000034;
var SGD_SM3_RSA = 0x00010001;
var SGD_SHA1_RSA = 0x00010002;
var SGD_SHA256_RSA = 0x00010004;
var SGD_SM3_SM2 = 0x00020201;
var SGD_SM1_ECB = 0x00000101;
var SGD_SM1_CBC = 0x00000102;
var SGD_SM1_CFB = 0x00000104;
var SGD_SM1_OFB = 0x00000108;
var SGD_SM1_MAC = 0x00000110;
var SGD_SSF33_ECB = 0x00000201;
var SGD_SSF33_CBC = 0x00000202;
var SGD_SSF33_CFB = 0x00000204;
var SGD_SSF33_OFB = 0x00000208;
var SGD_SSF33_MAC = 0x00000210;
var SGD_SM4_ECB = 0x00000401;
var SGD_SM4_CBC = 0x00000402;
var SGD_SM4_CFB = 0x00000404;
var SGD_SM4_OFB = 0x00000408;
var SGD_SM4_MAC = 0x00000410;
var SGD_ZUC_EEA3 = 0x00000801;
var SGD_ZUC_EEI3 = 0x00000802;
var SGD_RSA = 0x00010000;
var SGD_SM2 = 0X00020100;
var XML_SIGN_INFO_PLAIN_DATA = 1;
var XML_SIGN_INFO_DIGEST = 2;
var XML_SIGN_INFO_SIGNVALUE = 3;
var XML_SIGN_INFO_SIGNER_CERT = 4;
var XML_SIGN_INFO_DIGESTALGORITHM = 5;
var XML_SIGN_INFO_SIGNALGORITHM = 6;
var SIGN_FLAG_WITH_ORI = 0;
var SIGN_FLAG_WITHOUT_ORI = 1;
var CERT_TYPE_SIGN = 1;
var CERT_TYPE_EXCHANGE = 2;
var DEV_EVENT_ARRIVAL = 1;
var DEV_EVENT_REMOVE = 2;
var PROTO_HTTP = 1;
var PROTO_HTTPS = 2;
var ServiceConfig = {SERVICE_HOST: _$_13e8[0], SERVICE_PORT: _$_13e8[1], MAX_PARAM_LEN: 800, PROTOCOL: _$_13e8[2]};
var last_dev_num = -1;
var GDCA_PKI_LIB = $GDCA = {
    URL: function () {
        var _0x7C32 = ServiceConfig[_$_13e8[3]] + ServiceConfig[_$_13e8[4]] + _$_13e8[5] + ServiceConfig[_$_13e8[6]];
        return _0x7C32
    }(), setProtocol: function (_0x7C91) {
        if (_0x7C91 == PROTO_HTTP) {
            ServiceConfig[_$_13e8[6]] = 55661;
            ServiceConfig[_$_13e8[3]] = _$_13e8[2]
        } else {
            if (_0x7C91 == PROTO_HTTPS) {
                ServiceConfig[_$_13e8[6]] = 55662;
                ServiceConfig[_$_13e8[3]] = _$_13e8[7]
            } else {
                return false
            }
        }
        this[_$_13e8[8]] = ServiceConfig[_$_13e8[3]] + ServiceConfig[_$_13e8[4]] + _$_13e8[5] + ServiceConfig[_$_13e8[6]];
        return true
    }, base64Encode: function (_0x7CF0) {
        return gdca_base64[_$_13e8[9]](_0x7CF0)
    }, base64Decode: function (_0x7CF0) {
        return gdca_base64[_$_13e8[10]](_0x7CF0)
    }, _cutParam: function (_0x7E0D) {
        var _0x7D4F = this;
        var _0x7F89 = ServiceConfig[_$_13e8[11]];
        var _0x7F2A = _0x7E0D[_$_13e8[12]];
        if (_0x7F2A <= _0x7F89) {
            return [_0x7E0D]
        }
        var _0x8047 = 0;
        if (_0x7F2A % _0x7F89 > 0) {
            _0x8047 = parseInt(_0x7F2A / _0x7F89) + 1
        } else {
            _0x8047 = parseInt(_0x7F2A / _0x7F89)
        }
        var _0x7DAE = [];
        for (var _0x7ECB = 0; _0x7ECB < _0x8047; _0x7ECB++) {
            var _0x7FE8 = 0 + (_0x7ECB * _0x7F89);
            var _0x7E6C = _0x7FE8 + _0x7F89;
            if (_0x7ECB + 1 >= _0x8047) {
                _0x7E6C = _0x7E0D[_$_13e8[12]]
            }
            _0x7DAE[_$_13e8[14]](_0x7E0D[_$_13e8[13]](_0x7FE8, _0x7E6C))
        }
        return _0x7DAE
    }, _createSN: function () {
        var _0x7DAE = [_$_13e8[15], _$_13e8[16], _$_13e8[17], _$_13e8[18], _$_13e8[19], _$_13e8[20], _$_13e8[21], _$_13e8[22], _$_13e8[23], _$_13e8[24], _$_13e8[25], _$_13e8[26], _$_13e8[27], _$_13e8[28], _$_13e8[29], _$_13e8[30], _$_13e8[31], _$_13e8[32], _$_13e8[33], _$_13e8[34], _$_13e8[35], _$_13e8[36], _$_13e8[37], _$_13e8[38], _$_13e8[39], _$_13e8[40], _$_13e8[41], _$_13e8[42], _$_13e8[43], _$_13e8[44], _$_13e8[45], _$_13e8[46], _$_13e8[47], _$_13e8[48], _$_13e8[49], _$_13e8[50]];
        var _0x8105 = [];
        for (var _0x7ECB = 0; _0x7ECB < 16; _0x7ECB++) {
            var _0x80A6 = parseInt(Math[_$_13e8[51]]() * 35 + 0);
            _0x8105[_$_13e8[14]](_0x7DAE[_0x80A6])
        }
        return _0x8105[_$_13e8[53]](_$_13e8[52])
    }, exeFn: function (_0x81C3, _0x8222) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        _0x8222 = _0x8222 || {};
        setTimeout(function () {
            var _0x833F = JSON[_$_13e8[55]](_0x8222);
            var _0x7E0D = _0x7D4F[_$_13e8[56]](_0x833F);
            var _0x7DAE = _0x7D4F[_$_13e8[57]](_0x7E0D);
            var _0x839E = _0x7D4F[_$_13e8[58]]();
            var _0x8281 = [];
            for (var _0x7ECB = 0, _0x7F2A = _0x7DAE[_$_13e8[12]]; _0x7ECB < _0x7F2A; _0x7ECB++) {
                var _0x82E0 = {sn: _0x839E, total: _0x7F2A, sort: _0x7ECB, part: _0x7DAE[_0x7ECB]};
                _0x8281[_$_13e8[14]](_0x82E0)
            }
            var _0x83FD = function (_0x82E0, _0x845C) {
                var _0x7C32 = _0x7D4F[_$_13e8[8]];
                $[_$_13e8[70]]({
                    url: _0x7C32, data: _0x82E0, callbackParameter: _$_13e8[59], callback: _0x845C + _$_13e8[60] + _0x82E0[_$_13e8[61]] + _$_13e8[60] + _0x82E0[_$_13e8[62]], success: function (_0x8105) {
                        if (_0x8105[_$_13e8[63]] == 1) {
                            _0x8164[_$_13e8[65]](_0x8105[_$_13e8[64]])
                        } else {
                            if (_0x8105[_$_13e8[63]] == 0) {
                                _0x8164[_$_13e8[68]]({ErrorCode: _0x8105[_$_13e8[66]], ErrorMsg: _0x8105[_$_13e8[67]]})
                            }
                        }
                    }, error: function (_0x8579, _0x851A) {
                        var _0x84BB = {ErrorCode: _$_13e8[69], ErrorMsg: _0x851A};
                        _0x8164[_$_13e8[68]](_0x84BB)
                    }
                })
            };
            for (var _0x7ECB = 0; _0x7ECB < _0x8281[_$_13e8[12]]; _0x7ECB++) {
                _0x83FD(_0x8281[_0x7ECB], _0x81C3)
            }
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, getVersion: function (_0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        _0x7D4F[_$_13e8[76]](_$_13e8[75])[_$_13e8[74]](function (_0x84BB) {
            var _0x8696 = _0x84BB[_$_13e8[73]];
            if (_0x8637) {
                _0x8637(_0x8696)
            }
            _0x8164[_$_13e8[65]](_0x8696)
        })[_$_13e8[72]](function (_0x84BB) {
            if (_0x85D8) {
                _0x85D8(_0x84BB)
            }
            _0x8164[_$_13e8[68]](_0x84BB)
        });
        return _0x8164[_$_13e8[71]]()
    }, enumDevice: function (_0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        _0x7D4F[_$_13e8[76]](_$_13e8[78])[_$_13e8[74]](function (_0x84BB) {
            var _0x86F5 = _0x84BB[_$_13e8[77]];
            if (_0x8637) {
                _0x8637(_0x86F5)
            }
            _0x8164[_$_13e8[65]](_0x86F5)
        })[_$_13e8[72]](function (_0x84BB) {
            if (_0x85D8) {
                _0x85D8(_0x84BB)
            }
            _0x8164[_$_13e8[68]](_0x84BB)
        });
        return _0x8164[_$_13e8[71]]()
    }, getUserList: function (_0x8754, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {ReturnMethod: parseInt(_0x8754)};
            _0x7D4F[_$_13e8[76]](_$_13e8[79], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB)
                }
                _0x8164[_$_13e8[65]](_0x84BB)
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, Login: function (_0x8812, _0x87B3, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {UserName: _0x8812, PassWd: _0x87B3};
            _0x7D4F[_$_13e8[76]](_$_13e8[80], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB)
                }
                _0x8164[_$_13e8[65]](_0x84BB)
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, ChangePin: function (_0x8812, _0x88D0, _0x8871, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {ContainerName: _0x8812, OldPassWd: _0x88D0, NewPassWd: _0x8871};
            _0x7D4F[_$_13e8[76]](_$_13e8[81], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB)
                }
                _0x8164[_$_13e8[65]](_0x84BB)
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, getPinRetryCount: function (_0x8812, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {ContainerName: _0x8812};
            _0x7D4F[_$_13e8[76]](_$_13e8[83], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[82]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[82]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, getSignMethod: function (_0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        _0x7D4F[_$_13e8[76]](_$_13e8[85])[_$_13e8[74]](function (_0x84BB) {
            if (_0x8637) {
                _0x8637(_0x84BB[_$_13e8[84]])
            }
            _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[84]])
        })[_$_13e8[72]](function (_0x84BB) {
            if (_0x85D8) {
                _0x85D8(_0x84BB)
            }
            _0x8164[_$_13e8[68]](_0x84BB)
        });
        return _0x8164[_$_13e8[71]]()
    }, exportUserCert: function (_0x8812, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {ContainerName: _0x8812};
            _0x7D4F[_$_13e8[76]](_$_13e8[87], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[86]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[86]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, exportExChangeUserCert: function (_0x8812, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {ContainerName: _0x8812};
            _0x7D4F[_$_13e8[76]](_$_13e8[88], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[86]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[86]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, validateCert: function (_0x8812, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {Cert: _0x8812};
            _0x7D4F[_$_13e8[76]](_$_13e8[90], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[89]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[89]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, signData: function (_0x8812, _0x892F, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {ContainerName: _0x8812, InData: _0x892F};
            _0x7D4F[_$_13e8[76]](_$_13e8[92], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[91]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[91]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, signMessage: function (_0x8812, _0x892F, _0x898E, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {Flag: _0x898E, ContainerName: _0x8812, InData: _0x892F};
            _0x7D4F[_$_13e8[76]](_$_13e8[94], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[93]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[93]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, verifySignedData: function (_0x89ED, _0x8A4C, _0x8AAB, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {EncodeCert: _0x89ED, InData: _0x8A4C, SignValue: _0x8AAB};
            _0x7D4F[_$_13e8[76]](_$_13e8[95], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB)
                }
                _0x8164[_$_13e8[65]](_0x84BB)
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, verifySignedMessage: function (_0x8B0A, _0x8A4C, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {MessageData: _0x8B0A, InData: _0x8A4C};
            _0x7D4F[_$_13e8[76]](_$_13e8[96], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB)
                }
                _0x8164[_$_13e8[65]](_0x84BB)
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, getInfoFromSignedMessage: function (_0x8B0A, _0x8B69, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {MessageData: _0x8B0A, Type: parseInt(_0x8B69)};
            _0x7D4F[_$_13e8[76]](_$_13e8[97], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[89]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[89]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, encryptData: function (_0x8BC8, _0x892F, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {EncodeCert: _0x8BC8, InData: _0x892F};
            _0x7D4F[_$_13e8[76]](_$_13e8[99], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[98]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[98]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, decryptData: function (_0x8C27, _0x8A4C, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {ContainerName: _0x8C27, InData: _0x8A4C};
            _0x7D4F[_$_13e8[76]](_$_13e8[101], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[100]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[100]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, getCertInfo: function (_0x8C86, _0x8B69, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {Cert: _0x8C86, Type: _0x8B69};
            _0x7D4F[_$_13e8[76]](_$_13e8[102], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    if (_0x8B69 == SGD_CERT_ISSUER || _0x8B69 == SGD_CERT_SUBJECT || _0x8B69 == SGD_CERT_ISSUER_CN || _0x8B69 == SGD_CERT_ISSUER_O || _0x8B69 == SGD_CERT_ISSUER_OU || _0x8B69 == SGD_CERT_SUBJECT_O || _0x8B69 == SGD_CERT_SUBJECT_OU || _0x8B69 == SGD_CERT_SUBJECT_CN || _0x8B69 == SGD_CERT_SUBJECT_EMAIL || _0x8B69 == SGD_CERT_DER_PUBLIC_KEY || _0x8B69 == SGD_CERT_DER_EXTENSIONS) {
                        _0x8637(_0x84BB[_$_13e8[89]])
                    } else {
                        _0x8637(gdca_base64[_$_13e8[10]](_0x84BB[_$_13e8[89]]))
                    }
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[89]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, getCertInfoByOid: function (_0x8C86, _0x8CE5, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {Cert: _0x8C86, Oid: _0x8CE5};
            _0x7D4F[_$_13e8[76]](_$_13e8[103], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(gdca_base64[_$_13e8[10]](_0x84BB[_$_13e8[89]]))
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[89]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, hashFile: function (_0x8B69, _0x8D44, _0x8DA3, _0x8E02, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {AlgorithmType: _0x8B69, InFile: _0x8D44, PublicKey: _0x8DA3, ID: _0x8E02};
            _0x7D4F[_$_13e8[76]](_$_13e8[105], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[104]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[104]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, writeUsrDataFile: function (_0x8E61, _0x903C, _0x8F7E, _0x8EC0, _0x8F1F, _0x8FDD, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {ContainerName: _0x8E61, Pin: _0x903C, FileType: _0x8F7E, FileIndex: _0x8EC0, Offset: _0x8F1F, WriteData: _0x8FDD};
            _0x7D4F[_$_13e8[76]](_$_13e8[106], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB)
                }
                _0x8164[_$_13e8[65]](_0x84BB)
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, readUsrDataFile: function (_0x8E61, _0x8F7E, _0x8EC0, _0x8F1F, _0x909B, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {ContainerName: _0x8E61, FileType: _0x8F7E, FileIndex: _0x8EC0, Offset: _0x8F1F, ReadLen: _0x909B};
            _0x7D4F[_$_13e8[76]](_$_13e8[108], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[107]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[107]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, genRandData: function (_0x90FA, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {Len: _0x90FA};
            _0x7D4F[_$_13e8[76]](_$_13e8[110], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[109]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[109]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, signXML: function (_0x8812, _0x892F, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {ContainerName: _0x8812, InData: _0x892F};
            _0x7D4F[_$_13e8[76]](_$_13e8[112], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[111]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[111]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, verifyXMLSign: function (_0x9159, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {SignedData: _0x9159};
            _0x7D4F[_$_13e8[76]](_$_13e8[113], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB)
                }
                _0x8164[_$_13e8[65]](_0x84BB)
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, parseXMLSign: function (_0x9159, _0x8B69, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {SignedData: _0x9159, Type: _0x8B69};
            _0x7D4F[_$_13e8[76]](_$_13e8[114], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[89]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[89]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, tspGetTime: function (_0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {};
            _0x7D4F[_$_13e8[76]](_$_13e8[116], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[115]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[115]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, tspSealTimeStamp: function (_0x9217, _0x91B8, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {InData: _0x9217, AlgType: _0x91B8};
            _0x7D4F[_$_13e8[76]](_$_13e8[118], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[117]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[117]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, tspVerifyTimeStamp: function (_0x9217, _0x9276, _0x8BC8, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {InData: _0x9217, SealData: _0x9276, Cert: _0x8BC8};
            _0x7D4F[_$_13e8[76]](_$_13e8[119], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB[_$_13e8[115]])
                }
                _0x8164[_$_13e8[65]](_0x84BB[_$_13e8[115]])
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, apiControl: function (_0x92D5, _0x9393, _0x9334, _0x8637, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        setTimeout(function () {
            var _0x8222 = {ContainerName: _0x92D5, CtrlName: _0x9393, CtrlCmd: _0x9334};
            _0x7D4F[_$_13e8[76]](_$_13e8[120], _0x8222)[_$_13e8[74]](function (_0x84BB) {
                if (_0x8637) {
                    _0x8637(_0x84BB)
                }
                _0x8164[_$_13e8[65]](_0x84BB)
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 0);
        return _0x8164[_$_13e8[71]]()
    }, onDeviceNotify: function (_0x93F2, _0x85D8) {
        var _0x8164 = $[_$_13e8[54]]();
        var _0x7D4F = this;
        var _0x9451 = setInterval(function () {
            _0x7D4F[_$_13e8[76]](_$_13e8[78])[_$_13e8[74]](function (_0x84BB) {
                var _0x86F5 = eval(_0x84BB[_$_13e8[77]]);
                if (_0x86F5[_$_13e8[12]] > last_dev_num) {
                    if (_0x93F2 && last_dev_num != -1) {
                        _0x93F2(DEV_EVENT_ARRIVAL)
                    }
                    last_dev_num = _0x86F5[_$_13e8[12]]
                } else {
                    if (_0x86F5[_$_13e8[12]] < last_dev_num) {
                        if (_0x93F2 && last_dev_num != -1) {
                            _0x93F2(DEV_EVENT_REMOVE)
                        }
                        last_dev_num = _0x86F5[_$_13e8[12]]
                    }
                }
                _0x8164[_$_13e8[65]](_0x86F5)
            })[_$_13e8[72]](function (_0x84BB) {
                if (_0x85D8) {
                    _0x85D8(_0x84BB)
                }
                _0x8164[_$_13e8[68]](_0x84BB)
            })
        }, 3000);
        return _0x8164[_$_13e8[71]]()
    }
};