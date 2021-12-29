using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
namespace BTL_LT_UD_WEB.Security
{
    public class Crypto
    {
        public static string Hash(string value)
        {
            //return Convert.ToBase64String(
            //    System.Security.Cryptography.SHA256.Create()
            //    .ComputeHash(Encoding.UTF8.GetBytes(value))
            //    );
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(value);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public static string Decrypt(string value)
        {

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Convert.FromBase64String(value);
            TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider();
            //tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(value));
            tripDES.Mode = CipherMode.ECB;

            byte[] fromData = Encoding.UTF8.GetBytes(value);
            byte[] targetData = md5.ComputeHash(fromData);
            ICryptoTransform transform = tripDES.CreateDecryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
            return UTF8Encoding.UTF8.GetString(result);

        }
    }
}