namespace MaskCrawler.Web.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //// https://blog.csdn.net/qq_40600379/article/details/107102802
    //public class AuthenticationController : ControllerBase
    //{
    //    #region 注入
    //    private IJWTService _iJWTService = null;//注入IJWTService接口
    //    private IConfiguration _configuration = null;//注入配置信息接口

    //    /// <summary>
    //    /// 构造器中进行注入
    //    /// </summary>
    //    /// <param name="logger"></param>
    //    /// <param name="iJWTService"></param>
    //    /// <param name="configuration"></param>
    //    public AuthenticationController(IJWTService iJWTService, IConfiguration configuration)
    //    {
    //        this._iJWTService = iJWTService;
    //        this._configuration = configuration;
    //    }
    //    #endregion

    //    /// <summary>
    //    /// 请求token
    //    /// </summary>
    //    /// <param name="name"></param>
    //    /// <returns></returns>
    //    [Route("RequestToken")]
    //    [HttpGet]
    //    public string RequestToken(string name)
    //    {
    //        //如果等于admin那么就调用方法生成token，这里测试所以写死了
    //        if ("admin".Equals(name))
    //        {
    //            string token = this._iJWTService.GetToken(name);
    //            return JsonConvert.SerializeObject(new
    //            {
    //                result = true,
    //                token
    //            });
    //        }
    //        else
    //        {
    //            return JsonConvert.SerializeObject(new
    //            {
    //                result = false,
    //                token = "无法请求"
    //            });
    //        }
    //    }

    //    /// <summary>
    //    /// 校验token并返回
    //    /// </summary>
    //    /// <returns></returns>
    //    [HttpGet]
    //    [Route("CheckAuthorize")]
    //    [Authorize] //Microsoft.AspNetCore.Authorization
    //    public IActionResult CheckAuthorize()
    //    {
    //        try
    //        {
    //            //获取claims
    //            var claims = base.HttpContext.AuthenticateAsync().Result.Principal.Claims.ToList();
    //            //获取请求的token
    //            var token = base.HttpContext.AuthenticateAsync().Result.Properties.Items.ToArray()[0].Value;
    //            string json = JWTDecode(token);//解析token
    //            return new JsonResult(new
    //            {
    //                Data = "已授权",
    //                Type = "CheckAuthorize",
    //                Claim = claims[0].Issuer,
    //                Json = json
    //            });

    //        }
    //        catch (Exception ex)
    //        {
    //            return new JsonResult(new
    //            {
    //                Data = "未授权",
    //                Type = "CheckAuthorize",
    //                Exception = ex.ToString()
    //            });
    //        }
    //    }

    //    /// <summary>
    //    /// 解析JWT
    //    /// </summary>
    //    /// <param name="token"></param>
    //    public string JWTDecode(string token)
    //    {
    //        try
    //        {
    //            var json = new JwtBuilder()
    //            .WithAlgorithm(new HMACSHA256Algorithm()) // 设置JWT算法
    //            .WithSecret(_configuration["secret"])//校验的秘钥
    //            .MustVerifySignature()//必须校验秘钥
    //            .Decode(token);//解析token
    //            //输出解析后的json
    //            Console.WriteLine($"方式二解析后的json为：[{json}]");
    //            return json;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("解析json时出现异常：" + ex.ToString());
    //            return "";

    //        }

    //    }
    //}
}
