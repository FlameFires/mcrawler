using System;
using System.Security.Cryptography;

namespace MaskCrawler.Utils
{
    public static class SecurityUtil
    {
        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Encrypt(this string input)
        {
            try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] bytValue, bytHash;
                bytValue = System.Text.Encoding.UTF8.GetBytes(input);
                bytHash = md5.ComputeHash(bytValue);
                md5.Clear();
                string sTemp = "";
                for (int i = 0; i < bytHash.Length; i++)
                {
                    sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
                }
                input = sTemp.ToLower();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return input;
        }
    }
}
