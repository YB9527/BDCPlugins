// 需要安装 BouncyCastle NuGet 包
// Install-Package BouncyCastle

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Security.Cryptography;
using System.Text;

public class RSAEncryptionWithBouncyCastle
{
    public static string EncryptStringWithBouncyCastle(string plainText, string publicKeyBase64)
    {
        try
        {
            // 将 Base64 公钥转换为字节数组
            byte[] publicKeyBytes = Convert.FromBase64String(publicKeyBase64);

            // 使用 BouncyCastle 解析公钥
            var publicKey = PublicKeyFactory.CreateKey(publicKeyBytes);
            var rsaParams = (RsaKeyParameters)publicKey;

            // 创建 RSA 参数
            var parameters = new RSAParameters
            {
                Modulus = rsaParams.Modulus.ToByteArrayUnsigned(),
                Exponent = rsaParams.Exponent.ToByteArrayUnsigned()
            };

            // 使用 .NET RSA 加密
            using (var rsa = new RSACryptoServiceProvider(1024))
            {
                rsa.ImportParameters(parameters);

                // 将字符串转换为字节数组
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                // 加密数据
                byte[] encryptedBytes = rsa.Encrypt(plainTextBytes, false);

                // 将加密后的字节数组转换为 Base64 字符串
                return Convert.ToBase64String(encryptedBytes);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("加密失败: " + ex.Message);
        }
    }


    // 使用 Base64 格式的私钥加密字符串
    public static string EncryptWithPrivateKey(string plainText, string privateKeyBase64)
    {
        try
        {
            // 将 Base64 私钥转换为字节数组
            byte[] privateKeyBytes = Convert.FromBase64String(privateKeyBase64);

            // 使用 BouncyCastle 解析私钥
            var privateKey = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(privateKeyBytes);

            // 创建 RSA 参数
            var parameters = new RSAParameters
            {
                Modulus = privateKey.Modulus.ToByteArrayUnsigned(),
                Exponent = privateKey.PublicExponent.ToByteArrayUnsigned(),
                D = privateKey.Exponent.ToByteArrayUnsigned(),
                P = privateKey.P.ToByteArrayUnsigned(),
                Q = privateKey.Q.ToByteArrayUnsigned(),
                DP = privateKey.DP.ToByteArrayUnsigned(),
                DQ = privateKey.DQ.ToByteArrayUnsigned(),
                InverseQ = privateKey.QInv.ToByteArrayUnsigned()
            };

            // 使用 .NET RSA 加密
            using (var rsa = new RSACryptoServiceProvider(1024))
            {
                rsa.ImportParameters(parameters);

                // 将字符串转换为字节数组
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                // 使用私钥加密数据
                byte[] encryptedBytes = rsa.Encrypt(plainTextBytes, false);

                // 将加密后的字节数组转换为 Base64 字符串
                return Convert.ToBase64String(encryptedBytes);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("私钥加密失败: " + ex.Message);
        }
    }

    // 使用 XML 格式的私钥加密字符串
    public static string EncryptWithXmlPrivateKey(string plainText, string privateKeyXml)
    {
        using (var rsa = new RSACryptoServiceProvider(1024))
        {
            // 导入 XML 格式的私钥
            rsa.FromXmlString(privateKeyXml);

            // 将字符串转换为字节数组
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // 使用私钥加密数据
            byte[] encryptedBytes = rsa.Encrypt(plainTextBytes, false);

            // 将加密后的字节数组转换为 Base64 字符串
            return Convert.ToBase64String(encryptedBytes);
        }
    }

    // 使用公钥解密（用于验证私钥加密的数据）
    public static string DecryptWithPublicKey(string encryptedBase64, string publicKeyBase64)
    {
        try
        {
            // 将 Base64 公钥转换为字节数组
            byte[] publicKeyBytes = Convert.FromBase64String(publicKeyBase64);

            // 使用 BouncyCastle 解析公钥
            var publicKey = (RsaKeyParameters)PublicKeyFactory.CreateKey(publicKeyBytes);

            // 创建 RSA 参数
            var parameters = new RSAParameters
            {
                Modulus = publicKey.Modulus.ToByteArrayUnsigned(),
                Exponent = publicKey.Exponent.ToByteArrayUnsigned()
            };

            // 使用 .NET RSA 解密
            using (var rsa = new RSACryptoServiceProvider(1024))
            {
                rsa.ImportParameters(parameters);

                // 将 Base64 字符串转换为字节数组
                byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);

                // 使用公钥解密数据
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);

                // 将解密后的字节数组转换为字符串
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("公钥解密失败: " + ex.Message);
        }
    }



}