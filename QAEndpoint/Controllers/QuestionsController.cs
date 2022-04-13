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
/// <summary>
/// 一个符合REST API的例子
/// </summary>
namespace QAEndpoint.Controllers {
    /**
     * Route attributes defines the path controller will handle
     * In our case,the path will be api/question because [controller]
     * is substitued with the name of controller minus the word Controller
     */
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase {
        private IDataRepository dataRepository_ { get; init; }
        public QuestionsController(IDataRepository DataRepository) {

            dataRepository_ = DataRepository;
        }

        //GET: api/questions?search={search}
        [HttpGet]
        public IEnumerable<QuestionGetManyResponse> GetQuestions(string search) {
            if (!string.IsNullOrEmpty(search)) {
                //Console.WriteLine($"{DateTime.Now.ToLongTimeString()}||GetQuestions::{search}");
                return dataRepository_.GetQuestionsBySearch(search);
            }
            else {
                return dataRepository_.GetQuestions();
            }
        }

        //GET: api/Question/{id}
        [HttpGet("{questionId}")]
       public ActionResult<QuestionGetSingleResponse> GetQuestion(int questionId) {
            //  id -> 能够找到一条对应的记录 -> 返回这条记录
            //  id -> 不存在对应的记录 -> NotFound
            var question = dataRepository_.GetQuestion(questionId);
            if (question != null) return question;
            else return NotFound();
        }

        //GET: api/Questions/unanswered
        [HttpGet("unanswered")]
        public IEnumerable<QuestionGetManyResponse> GetUnansweredQuestions() {
            return dataRepository_.GetUnansweredQuestions();
        }

        //POST: api/Questions
        [HttpPost]
        public ActionResult<QuestionGetSingleResponse> 
        PostQuestion(QuestionPostRequest questionPostRequest) {
            var savedQuestion = dataRepository_.PostQuestion(questionPostRequest);
            // 返回一个HTTP 201 状态码，同时在HTTP响应头中 会有一个 location的字段指示 如果
            // 想要获取刚才插入的对象，需要请求的URL是什么
            return CreatedAtAction(nameof(GetQuestion),
                new { questionId = savedQuestion.QuestionId },
                savedQuestion);
        }

        //PUT: api/Questions/1
        [HttpPut("{questionId}")]
        public ActionResult<QuestionGetSingleResponse>
        PutQuestion(int questionId,QuestionPutRequest questionPutRequest) {
            var savedQuestion = dataRepository_.GetQuestion(questionId);
            if (null == savedQuestion) {
                // 更新了一条不存在的记录
                return NotFound();
            }
            //为了让别人用起来更方便，(为了你前端同事的血压和你的身体健康着想)
            //如果请求未提供Question的Title，那么就使用原纪录中的title字段的值(Content字段同理)
            questionPutRequest.Title = string.IsNullOrEmpty(questionPutRequest.Title) ? savedQuestion.Title :
                questionPutRequest.Title;
            questionPutRequest.Content = string.IsNullOrEmpty(questionPutRequest.Content) ? savedQuestion.Content :
                questionPutRequest.Content;

            dataRepository_.PutQuestion(questionId, questionPutRequest);
            return savedQuestion;
        }

        [HttpDelete("{questionId}")]
        public ActionResult DeleteQuestion(int questionId) {
            var question = dataRepository_.GetQuestion(questionId);
            if(null != question) {
                dataRepository_.DeleteQuestion(questionId);
                return NoContent();//删除完毕之后返回HTTP204状态码
            }
            return NotFound();
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

/**
 * 没有前端只有后端代码的时候 如何对接口进行测试？
 * 传统方法有：Postman，Jmeter 等工具 对接口进行测试
 * 可以借助插件：Swagger
 */ 