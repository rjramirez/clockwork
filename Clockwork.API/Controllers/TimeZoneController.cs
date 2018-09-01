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
        public IActionResult Get()
        {
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            return Ok(tz.ToList());           
        }
    }
}
