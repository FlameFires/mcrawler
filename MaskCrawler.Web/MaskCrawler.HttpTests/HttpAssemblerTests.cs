using MaskCrawler.Models.Http;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MaskCrawler.Http.Tests
{
    [TestClass()]
    public class HttpAssemblerTests
    {
        [TestMethod()]
        public async Task ReqStringTest()
        {
            var info = new HttpInfo
            {
                Url = "https://www.baidu.com/",
                //Url = "https://www.tianqiapi.com/free/day?appid=55586448&appsecret=bQM97pMQ&unescape=1&city=%E4%B9%9D%E6%B1%9F",
                Header = "user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.54 Safari/537.36 Edg/95.0.1020.30"
            };
            IHttpDecorator http = new HttpDecorator(info);
            var rs = await http.ReqStream();


            Debug.WriteLine("字节数：" + rs.Length);

            rs.Position = 0;
            StreamReader sr = new StreamReader(rs);
            var str = sr.ReadToEnd();
            sr.Close();

            Debug.WriteLine("响应内容：" + str);

            Assert.IsNotNull(rs);
        }
    }
}