using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAEndpoint.Controllers {
    [ApiController]

    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase {
        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger) {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetWeather")]
        public IEnumerable<WeatherForecast> GetWeather() {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet]
        [Route("GetQuestions")]
        public IEnumerable<Question> GetQuestions() {
            return QuestionRepository.QuestionList;
        }
        [HttpGet]
        [Route("GetQuestions/{keywords}")]
        public IEnumerable<Question> GetQuestions(string keywords) {
            return QuestionRepository.QuestionList.Where(q => q.QuestionContent.Contains(keywords));
        }

        //假设前端是通过表单的方式提交数据
        [HttpPost]
        [Route("login")]
        public bool Login([FromForm]string userName,[FromForm]string password) {
            return false;
        }

        [HttpPost]
        [Route("AskQuestion")]
        public IActionResult AddQuestion([FromBody] Question question) {
            QuestionRepository.QuestionList.Add(question);
            return new JsonResult(new {
                code = "1",
                message = "success"
            });
        }
    }
}

// 什么是restfull api ？
// 查询：Get  插入：Post 更新：Put 删除：Delete
/**
 * 从服务端查询问题列表：
 * Http Get: api/question/questions
 * 插入操作：
 * Http Post: api/question/questions
 * 更新操作：
 * Http Put：
 * 删除：
 * Http Delete：
 * 
 * 
 * HTTP QUERY:  api/question/questions  ?id=10&&user=abc
 * ROUTE:       api/question/questions/10
 */
