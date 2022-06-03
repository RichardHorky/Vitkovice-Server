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
        private readonly Helpers.DateHelper _dateHelper;

        public DateController(Helpers.DateHelper dateHelper)
        {
            _dateHelper = dateHelper;
        }

        public ActionResult<string> Get()
        {
            var seconds = _dateHelper.GetSeconds();
            return Ok($"{{{seconds}}}");
        }
    }
}
