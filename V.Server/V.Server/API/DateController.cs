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
        [HttpGet]
        public ActionResult<string> GetCurrentSeconds()
        {
            try
            {
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
                var year = cstTime.Year;
                var firstInYear = new DateTime(year, 1, 1);
                var seconds = (long)Math.Floor((cstTime - firstInYear).TotalSeconds);
                return Ok(seconds.ToString());
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }
    }
}
