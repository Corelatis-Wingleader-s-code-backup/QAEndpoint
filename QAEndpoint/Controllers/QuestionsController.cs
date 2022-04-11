using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/****/
using QAEndpoint.Data;
using QAEndpoint.Data.Models;

namespace QAEndpoint.Controllers {
    /**
     * Route attributes defines the path controller will handle
     * In our case,the path will be api/question because [controller]
     * is substitued with the name of controller minus the word Controller
     */
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase {
        private ILogger logger_ { get; init; }
        private IDataRepository dataRepository_ { get; init; }
        public QuestionsController(ILogger logger, IDataRepository DataRepository) {
            logger_ = logger;
            dataRepository_ = DataRepository;
        }
        [HttpGet]
        public IEnumerable<QuestionGetManyResponse> GetQuestions() {
            var questions = dataRepository_.GetQuestions();
            return questions;            
        }
    }
}

/**
 * localhost:3000/api/weatherforecast 
 * localhost:3000/api/values
 * 
 * Dependency injection is the process of injecting an instance 
 * of a class into another object. The goal of dependency injection 
 * is to decouple a class from its dependencies so that the dependencies 
 * can be changed without changing the class. ASP.NET has its own dependency 
 * injection facility that allows class dependencies to be defined 
 * when the app starts up. These dependencies are then available 
 * to be injected into other class constructors.
 */
