using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAEndpoint.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {
       private ILogger logger_ { get; init; }
       public ValuesController(ILogger logger) {
            logger_ = logger;
        }

        [Route("index")]
        public IActionResult Index() {
            
        }
    }
}

/**
 * localhost:3000/api/weatherforecast 
 * localhost:3000/api/values
 */
