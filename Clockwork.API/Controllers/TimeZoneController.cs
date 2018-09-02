using System;
using Microsoft.AspNetCore.Mvc;
using Clockwork.API.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Clockwork.API.Controllers
{
    [Route("api/[controller]")]
    public class TimeZoneController : Controller
    {
        // GET api/timezone
        public JsonResult Get()
        {
            using (var db = new ClockworkContext())
            {
                ReadOnlyCollection<TimeZoneInfo> tz;
                tz = TimeZoneInfo.GetSystemTimeZones();
                var result = new { timeZones = tz.ToList(), timeTable = db.CurrentTimeQueries.ToList() };

                return Json(result);
            }  
        }
    }
}
