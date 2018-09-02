using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Clockwork.API.Models;
using System.Security;

namespace Clockwork.API.Controllers
{
    [Produces("application/json")]
    [Route("api/CurrentTimeQuery")]
    public class CurrentTimeQueryController : Controller
    {
        // GET: api/CurrentTimeQuery
        [HttpGet]
        public JsonResult Get()
        {
            DateTime utcTime = DateTime.Now;
            DateTime serverTime = DateTime.Now;
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

                return Json(db.CurrentTimeQueries.ToList());
            }

        }

        // GET: api/CurrentTimeQuery/5
        [HttpGet("{timeZoneId}", Name = "GetTimeByTimeZoneId")]
        public JsonResult GetTimeByTimeZoneId(string timeZoneId)
        {
            var utcTime = DateTime.MinValue;
            var serverTime = DateTime.MinValue;

            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();

            if (!string.IsNullOrEmpty(timeZoneId))
            {
               
                try
                {

                    DateTime dateTimeNowCustomTimeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, timeZoneId);
                    TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    utcTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeInfo);
                    serverTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeInfo);

                    //TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    //utcTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.UtcNow, customTimeZone);
                    //serverTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, customTimeZone);
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
                Time = serverTime,
                TimeZoneId = timeZoneId
            };

            //Save to Database
            using (var db = new ClockworkContext())
            {
                db.CurrentTimeQueries.Add(returnVal);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                return Json(db.CurrentTimeQueries.ToList());
            }

            
        }
        
        // POST: api/CurrentTimeQuery
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/CurrentTimeQuery/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
