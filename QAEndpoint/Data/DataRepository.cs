using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

//添加Dapper和SqlClient的引用
using System.Data.SqlClient;
using Dapper;

using QAEndpoint.Data.Models;

namespace QAEndpoint.Data {
    //仓储的实现类 - 使用Dapper
    public class DataRepository : IDataRepository {
        //readonly keyword prevents the variable from being 
        // changed outside of the class constructor
        private readonly string connectionString_;

        // The configuration parameter in the constructor gives us
        // access to items within the appsettings.json file. the key 
        // we use when accessing the configuration object is the path
        // to the item we want from the appsettings.json file,with colons
        // being used to navigate the fields in the JSON
        public DataRepository(IConfiguration configuration) {
            connectionString_ = configuration["ConnectionStrings:DefaultConnection"];
        }

        public void DeleteQuestion(int questionId) {
            using var connection = new SqlConnection(connectionString_);
            connection.Open();
            connection.Execute(
                @"EXEC dbo.Question_Delete @QuestionId = @QuestionId",
                new { QuestionId = questionId }
                );
        }

        public AnswerGetResponse GetAnswer(int answerId) {
            using var connection = new SqlConnection(connectionString_);
            connection.Open();
            return connection.QueryFirstOrDefault<AnswerGetResponse>(
                @"EXEC dbo.Answer_Get_ByAnswerId @AnswerId=@AnswerId",
                new { AnswerId = answerId });
        }

        public QuestionGetSingleResponse GetQuestion(int questionId) {
            using var connection = new SqlConnection(connectionString_);
            connection.Open();
            // return a single record (or null if the record isn't found) rather than 
            // a collection of records.
            var question =
                connection.QueryFirstOrDefault<QuestionGetSingleResponse>(
                    @"EXEC dbo.Question_GetSingle @QuestionId = @QuestionId",
                    new { QuestionId = questionId }
                    );
            if (question != null) {
                question.Answers =
                    connection.Query<AnswerGetResponse>(
                        @"EXEC dbo.Answer_Get_ByQuestionId @QuestionId=@QuestionId",
                        new { QuestionId = questionId }
                        );
            }
            return question;
        }

        public IEnumerable<QuestionGetManyResponse> GetQuestions() {
            //notice that use a using block to declare the database connection
            using var connection = new SqlConnection(connectionString_);
            connection.Open();
            // Query 是Dapper提供的 SqlConnection对象的  扩展 
            // DAPPER执行无参的存储过程或者SQL
            return connection.Query<QuestionGetManyResponse>(
                @"EXEC dbo.Question_GetMany"
            );
        }

        public IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search) {
            using var connection = new SqlConnection(connectionString_);
            connection.Open();
            // This is how pass in the Stored Procedure parameter value
            return connection.Query<QuestionGetManyResponse>(
                @"EXEC dbo.Question_GetMany_BySearch @Search=@Search",
                new { Search = search }
                );
        }

        public IEnumerable<QuestionGetManyResponse> GetUnansweredQuestions() {
            using var connection = new SqlConnection(connectionString_);
            connection.Open();
            return connection.Query<QuestionGetManyResponse>(
                @"EXEC dbo.Question_GetUnanswered"
            );
        }

        public AnswerGetResponse PostAnswer(AnswerPostRequest answer) {
            using var connection = new SqlConnection(connectionString_);
            connection.Open();
            return connection.QueryFirst<AnswerGetResponse>(
                @"EXEC dbo.Answer_Post 
                @QuestionId = @QuestionId,
                @Content=@Content,
                @UserId =@UserId,
                @UserName=@UserName,
                @Created=@Created",
                answer);
        }

        public QuestionGetSingleResponse PostQuestion(QuestionPostRequest question) {
            using var connection = new SqlConnection(connectionString_);
            connection.Open();
            var questionId = connection.QueryFirst<int>(
                @"EXEC dbo.Question_Post
                @Title = @Title , @Content = @Content ,
                @UserId = @UserId , @UserName = @UserName,
                @Created = @Created",
                question
                );
            return GetQuestion(questionId);
        }

        public QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest question) {
            using var connection = new SqlConnection(connectionString_);
            // use Dapper Execute method because we are simply executing a  
            // stored procedure and not returning anything
            connection.Execute(@"EXEC dbo.Question_Put 
            @QuestionId = @Question Id,@Title = @Title,@Content = @Content",
            new {
                QuestionId = questionId,
                question.Title,
                question.Content
            });
            return GetQuestion(questionId);
        }

        public bool QuestionExists(int questionId) {
            using var connection = new SqlConnection(connectionString_);
            connection.Open();
            // use QueryFirst method rather than QueryFirstOrDefault because
            // the Stored Procedure will always return a single record
            return connection.QueryFirst<bool>(
                @"EXEC dbo.Question_Exists @QuestionId = @QuestionId",
                new { QuestionId = questionId }
                );
        }
    }
}

/**
 * Execute: 仅执行SQL语句，不返回任何结果的时候使用
 * QueryFirst：与EF中的FirstOrDefault类似，返回查询结果的第一条；把查询结果映射到泛型参数指定的类型
 * Query：执行SQL语句，并且返回一个集合；把查询结果映射到泛型参数指定的类型
 * 
 * ---> Dapper的所有函数 同时都实现了一个名字里带Async的异步版本
 * 
 * ----> 使用Dapper的优点：
 * Dapper体积小，整个库只有一个静态类，并且兼容到.NET 2.0版本 非常易于集成
 * Dapper执行速度很快，执行速度相当于IDataReader 
 * 不依赖于数据库的结构，代码写起来比较自由；
 * 不需要用户对数据库优化，EF有很深的理解，学习成本低
 * 反过来，EF应用在大型数据库（数据量和访问量都很巨大）的时候，EF是对数据库的抽象
 * 采取的方式是将LINQ转换成SQL 如果使用者对于数据库性能优化以及EF的原理没有很深刻的理解
 * 就会遇到性能挑战（学习成本已经高于EF的抽象提供的便利）
 */