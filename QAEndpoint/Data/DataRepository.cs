using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using QAEndpoint.Data.Models;
namespace QAEndpoint.Data {
    public class DataRepository : IDataRepository {
        //readonly keyword prevents the variable from being 
        // changed outside of the class constructor
        private readonly string connectionString_;

        // The configuration parameter in the constructor gives us
        // access to items within the appsettings.json file. the key 
        // we use when accessing the configuration object is the path
        // to the item we want from the appsettings.json file,with colons
        // being used to navigate the fields in the JSON
        public  DataRepository(IConfiguration configuration) {
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
                new { Serch = search }
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
            throw new NotImplementedException();
        }

        public QuestionGetSingleResponse PostQuestion(QuestionPostRequest question) {
            using var connection = new SqlConnection(connectionString_);
            connection.Open();
            var questionId = connection.QueryFirst<int>(
                @"EXEC dbo.Question_Post
                @Title = @Title , @Content = @Content,
                @UserId = @UserId , @UserName = @UserName,
                @Created = @Created,",
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
