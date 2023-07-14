using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Security.JwtBearer;
using JYD.Helper;
using System.Net;

namespace JYD.Controllers.Base
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public virtual JsonResult Json(object? data)
        {
            return new JsonResult(data);
        }

        
    }

    [ApiController]
    [Route("api/[action]")]
    public class JWTController : ControllerBase
    {
        [NonAction]
        public virtual JsonResult Json(object? data)
        {
            return new JsonResult(data);
        }

        [HttpGet]
        public async Task<ActionResult> CreateToken()
        {
            var token = JwtClient.GetToken();
            var expiry = JwtClient.GetTokenExpirationTime(token);

            var result = new Dictionary<string, string>()
            {
                { "Token",token },
                { "Expiry",expiry.ToString() }
            };

            return Json(new ResponseParameter { Code = HttpStatusCode.OK, Data = JsonConvert.SerializeObject(result), ErrorMessage = null });
        }

        
    }
}
