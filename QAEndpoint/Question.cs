using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAEndpoint {
    public class Question {
        public int QuestionId { get; set; }
        public DateTime Create { get; set; } = new DateTime();
        public string UserName { get; set; }
        public string QuestionContent { get; set; }
    }
}
