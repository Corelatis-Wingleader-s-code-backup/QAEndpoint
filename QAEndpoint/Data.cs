using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAEndpoint {
    public static class QuestionRepository {
        public static List<Question> QuestionList = new List<Question> {
            new Question{QuestionId=1,UserName="Ganyu",QuestionContent="如何评价GenXhin Compact的新卡池角色强度?"},
            new Question{QuestionId=2,UserName="Eula",QuestionContent="如何评价X社新作销量不如XX游戏一个月流水?" },
            new Question{QuestionId=2,UserName="Keqing",QuestionContent="如何评价原神新出的角色Yelan的立绘？" },
               new Question{QuestionId=2,UserName="Raiden Shogun",QuestionContent="原神里的RaidenShogun强度如何？" }
        };
    }
    public class MyTask {
        public int TaskId { get; set; }
        public string Task { get; set; }
    }
}
