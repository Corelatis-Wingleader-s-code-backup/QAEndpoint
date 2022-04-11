using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAEndpoint.Data.Models;

namespace QAEndpoint.Data {
    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IDataRepository {
        /** READ DATA FROM DATABASE **/
        IEnumerable<QuestionGetManyResponse> GetQuestions();
        IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search);
        IEnumerable<QuestionGetManyResponse> GetUnansweredQuestions();
        QuestionGetSingleResponse GetQuestion(int questionId);
        bool QuestionExists(int questionId);
        AnswerGetResponse GetAnswer(int answerId);

        /**WRITE DATA TO DATABASE**/
        QuestionGetSingleResponse PostQuestion(QuestionPostRequest question);
        QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest question);
        void DeleteQuestion(int questionId);
        AnswerGetResponse PostAnswer(AnswerPostRequest answer);
    }
}
