using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Infrastructure
{
    public static class SecurityHelper
    {
        /// <summary>
        /// AES ECB模式不需要向量，可以提高并行计算，这里虽然设置了但是不会使用
        /// </summary>
        public static char[] AESIv = { '5', '1', 'w', 'n', 'l', 'g', 'a', 'm','e','2','0','4','8','x','y','x' };
        public static string EncryptAES(string input, string key)
        {
            using (var aesAlg = Aes.Create())
            {
                byte[] encryptKey = Encoding.UTF8.GetBytes(key),
                    iv =Encoding.UTF8.GetBytes(AESIv),
                    inputByteArray = Encoding.UTF8.GetBytes(input);

                aesAlg.Key = encryptKey;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;
                using (var encryptor = aesAlg.CreateEncryptor(encryptKey, iv))
                {

                    byte[] results = encryptor.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
                    return Convert.ToBase64String(results, 0, results.Length);

                }
            }
        }

        public static string DecryptAES(string input, string key)
        {

            using (var aesAlg = Aes.Create())
            {
                byte[] encryptKey = Encoding.UTF8.GetBytes(key),
                    iv = Encoding.UTF8.GetBytes(AESIv),
                    inputByteArray = Convert.FromBase64String(input);

                aesAlg.Key = encryptKey;    
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;
                using (var encryptor = aesAlg.CreateDecryptor(encryptKey, iv))
                {

                    byte[] results = encryptor.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
                    return Encoding.UTF8.GetString(results);

                }
            }
        }

        public static T GetAES<T>(string source, string key)
        {
            string decrypt = DecryptAES(source, key);
            return JsonConvert.DeserializeObject<T>(decrypt);
        }
    }
}
