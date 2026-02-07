using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /*
    Base api controller class to reduce code redundancy
    Don't need to repeat
    "[Route("api/[controller]")]
    [ApiController]"
    */ 
    public class BaseAPIController : ControllerBase
    {
    }
}
