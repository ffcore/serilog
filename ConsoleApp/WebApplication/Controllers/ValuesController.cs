using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Owin;
using System.Web;

namespace WebApplication.Controllers
{
    public class ValuesController : ApiController
    {
        const string MessageTemplate =
             "{ID} - HTTP {RequestMethod} {RequestPath} {Message}";

        private ILogger logger { get; set; }
        
        public ValuesController(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException("logger");
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            logger.Debug("// GET api/values");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var owinContext = HttpContext.Current.GetOwinContext();

            string traceId = owinContext.Get<string>("TraceID") ?? "N/A";

            logger.Write(LogEventLevel.Debug, MessageTemplate, traceId, Request.Method.Method, Request.RequestUri.PathAndQuery, "Get with value");

            if (id == 2)
                return InternalServerError(new Exception("bla bla"));

            return Ok("value");
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            logger.Debug("// POST api/values");
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            logger.Debug($"// PUT api/values/{id}");
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            logger.Debug($"// DELETE api/values/{id}");
        }
    }
}
