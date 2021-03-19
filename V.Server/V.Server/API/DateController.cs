using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V.Server.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateController : ControllerBase
    {
        public ActionResult<string> Get()
        {
            var now = DateTime.Now.AddHours(1);
            var year = now.Year;
            var firstInYear = new DateTime(year, 1, 1);
            var seconds = (long)Math.Floor((now - firstInYear).TotalSeconds);
            return Ok($"{{{seconds}}}");
        }
    }
}
