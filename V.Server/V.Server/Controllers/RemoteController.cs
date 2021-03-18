using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V.Server.Controllers
{
    public class RemoteController : Controller
    {
        [Route("Remote")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Remote/Date")]
        public long Date()
        {
            var now = DateTime.Now.AddHours(1);
            var year = now.Year;
            var firstInYear = new DateTime(year, 1, 1);
            var seconds = (long)Math.Floor((now - firstInYear).TotalSeconds);
            return seconds;
        }
    }
}
