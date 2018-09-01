using System;
using Microsoft.AspNetCore.Mvc;
using Clockwork.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Clockwork.API.Controllers
{
    [Route("api/[controller]")]
    public class CurrentTimeController : Controller
    {
        // GET api/currenttime
        [HttpGet]
        public IActionResult Get()
        {
            var utcTime = DateTime.UtcNow;
            var serverTime = DateTime.Now;
            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
            
            var returnVal = new CurrentTimeQuery
            {
                UTCTime = utcTime,
                ClientIp = ip,
                Time = serverTime
            };

            //Save to Database
            using (var db = new ClockworkContext())
            {
                db.CurrentTimeQueries.Add(returnVal);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                //List the saved timestamps
                Console.WriteLine();
                foreach (var CurrentTimeQuery in db.CurrentTimeQueries)
                {
                    Console.WriteLine(" - {0}", CurrentTimeQuery.UTCTime);
                }
            }
            
            return Ok(returnVal);
        }

        [HttpPost]
        public IActionResult GetTimeByTimeZoneId(string timeZoneId)
        {
            Console.WriteLine("Converting time to " + timeZoneId);
            var utcTime = DateTime.MinValue;
            var serverTime = DateTime.MinValue;

            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();

            if (!string.IsNullOrEmpty(timeZoneId))
            {
                DateTime dateTimeNowCustomTimeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, timeZoneId);

                try
                {
                    TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    utcTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now, customTimeZone);
                    serverTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, customTimeZone);
                }
                // Handle exception
                //
                // As an alternative to simply displaying an error message, an alternate Eastern
                // Standard Time TimeZoneInfo object could be instantiated here either by restoring
                // it from a serialized string or by providing the necessary data to the
                // CreateCustomTimeZone method.
                catch (TimeZoneNotFoundException)
                {
                    Console.WriteLine("The Eastern Standard Time Zone cannot be found on the local system.");
                }
                catch (InvalidTimeZoneException)
                {
                    Console.WriteLine("The Eastern Standard Time Zone contains invalid or missing data.");
                }
                catch (SecurityException)
                {
                    Console.WriteLine("The application lacks permission to read time zone information from the registry.");
                }
                catch (OutOfMemoryException)
                {
                    Console.WriteLine("Not enough memory is available to load information on the Eastern Standard Time zone.");
                }
            }

            var returnVal = new CurrentTimeQuery
            {
                UTCTime = utcTime,
                ClientIp = ip,
                Time = serverTime
            };

            //Save to Database
            using (var db = new ClockworkContext())
            {
                db.CurrentTimeQueries.Add(returnVal);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                foreach (var CurrentTimeQuery in db.CurrentTimeQueries)
                {
                    Console.WriteLine(" - {0}", CurrentTimeQuery.UTCTime);
                }
            }

            return Ok(returnVal);
        }



    }

    public struct DateTimeWithZoneInfo
    {
        private readonly DateTime utcDateTime;
        private readonly TimeZoneInfo timeZone;

        public DateTimeWithZoneInfo(DateTime dateTime, TimeZoneInfo timeZone)
        {
            var dateTimeUnspec = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, timeZone);
            this.timeZone = timeZone;
        }

        public DateTime UniversalTime { get { return utcDateTime; } }

        public TimeZoneInfo TimeZone { get { return timeZone; } }

        public DateTime LocalTime
        {
            get
            {
                return TimeZoneInfo.ConvertTime(utcDateTime, timeZone);
            }
        }
    }
}
