using Microsoft.AspNetCore.Mvc;

namespace MaskCrawler.Controllers
{
    [Area("Main")]
    [Route("[area]/[controller]/[action]")]
    //[Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
    }
}
