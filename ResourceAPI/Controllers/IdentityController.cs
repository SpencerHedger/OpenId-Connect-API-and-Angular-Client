using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ResourceAPI.Controllers
{
    [Route("[controller]")]
    [Authorize] // Must be authorized to access this.
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // List the user Claims in JSON format.
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
