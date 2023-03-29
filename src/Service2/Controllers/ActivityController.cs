using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        [HttpGet]
        [Route("Start")]
        public IActionResult GetAsync()
        {
            return Ok(new { status = HttpStatusCode.OK, success = "...stated..." });
        }
    }
}
