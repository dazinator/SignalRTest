using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalRTest
{

    public class Position
    {

        public DateTimeOffset Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Accuracy { get; set; }
        public double AltitudeAccuracy { get; set; }
        public double Heading { get; set; }
        public double Speed { get; set; }
    }

    [Route("api/[controller]")]
    public class LocationTrackerController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]Position value, [FromServices]IHubContext<LocationTrackerHub> hubContext)
        {
            //hubContext.
          //  var message = $"I am located @ {value?.Latitude} : {value.Longitude} as of: {value.Timestamp.ToString()}";
            hubContext.Clients.All.InvokeAsync("broadcastLocation", "Daz", value.Latitude, value.Longitude);
            //hubContext.Clients.All.InvokeAsync("Send", "device",);
           
           // chathub.Send("yo", "its me");
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
