using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MaskCrawler.Utils
{
    public static class StringUtil
    {
        /// <summary>
        /// 正则匹配获取值
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string RegexGet(this string text, string pattern)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"“{nameof(text)}”不能为 null 或空白。", nameof(text));
            }

            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentException($"“{nameof(pattern)}”不能为 null 或空白。", nameof(pattern));
            }

            if (Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase))
            {
                Match val = Regex.Match(text, pattern);
                return val?.Groups[0]?.Value;
            }

            return null;
        }

        /// <summary>
        /// 切割获取字符数组
        /// </summary>
        /// <param name="text"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static string[] SplitGets(this string text, params string[] paras)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"“{nameof(text)}”不能为 null 或空白。", nameof(text));
            }

            if (paras is null)
            {
                return new string[] { text };
            }

            return text.Split(paras, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 随机获取一个指定字数的字符串
        /// </summary>
        /// <param name="digit">指定位数</param>
        /// <returns></returns>
        public static string RandomGetStr(int digit)
        {
            var alphas = new List<char>(digitallies);
            alphas.AddRange(lower_letters);
            alphas.AddRange(upper_letters);

            StringBuilder sb = new StringBuilder();
            int seed = int.MaxValue;
            Random rand = new Random();
            for (int i = 0; i < digit; i++)
            {
                var val = rand.Next(alphas.Count);
                sb.Append(alphas[val]);
                Thread.Sleep(10);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取随机种子
        /// </summary>
        /// <returns></returns>
        private static int SeedByDate()
        {

            DateTime now = DateTime.Now;
            DateTime dtbegin = new DateTime(DateTime.Now.Year, 1, 1);

            int diffday = now.DayOfYear % 3;//3天一个循环
            DateTime result = dtbegin.AddDays(now.DayOfYear - diffday);
            return (int)result.Ticks;
        }

        private static char[] digitallies = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static char[] lower_letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private static char[] upper_letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };


        public static char GetChar(this int val)
        {
            return (char)val;
        }

        public static int GetInt(this char val)
        {
            return (int)val;
        }
    }
}
