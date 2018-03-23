using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication.Controllers
{
    public class ValuesController : ApiController
    {
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
        public string Get(int id)
        {
            logger.Debug($"// GET api/values/{id}");
            return "value";
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
