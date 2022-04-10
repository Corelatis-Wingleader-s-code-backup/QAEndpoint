using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAEndpoint.Data.Models {
    /// <summary>
    /// The property names match the fields that have been output from the Question_GetMany stored procedure.
    /// This allows Dapper to automatically map the data from the database to this class. The property types 
    /// have also been carefully chosen so that this Dapper mapping process works.
    /// Dpper will ignore fields that don't have the correspondding proterties in the class.
    /// </summary>
    public class QuestionGetManyResponse {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
    }
}
