﻿using Microsoft.AspNetCore.Http;
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
        public ActionResult<long> GetCurrentSeconds()
        {
            var year = DateTime.Today.Year;
            var firstInYear = new DateTime(year, 1, 1);
            var seconds = (long)Math.Floor((DateTime.Now - firstInYear).TotalSeconds);
            return Ok(seconds);
        }
    }
}