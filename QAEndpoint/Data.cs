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


/***
 * .NET 5.0 中 不能使用Entity Framework
 * 应该使用的是 Entity Framework Core
 * 
 * Tabls:
 * Question
 * Answer
 * 
 * Insert/Update/Delete/Exist/Select
 * 需求分析：
 * 1. 按Id查询某一条Question
 * 2. 根据一个检索条件，返回一个Question组成的列表
 * 3. 插入一个新的Question
 * 4. 删除一个Question(根据id进行删除)
 * 5. 根据给定id判断一个Question是否存在 
 * 
 * =====> 创建一个仓储接口 用于进行数据的访问
 * 
 * 阅读一下p197 17.6.1
 * 1、有不同的实现，可以用于应对不同的数据库
 *  客户A：MySql  客户B：Oracle 
 *  如果你的项目在访问数据的时候 没有使用这一层做抽象的话，（直接使用EF+MSSQL实现）
 *  项目会有很大改动
 *  
 *  如果使用仓储模式 将数据访问的功能抽象出来，就可以根据不同的用户需求，
 *  来采取不同的实现（将每一种实现分别写到不同的dll中，遇到不同需求的用户，
 *  就可以直接替换dll文件 来解决 不用大幅度重构项目)
 *  
 *  ===> 面向接口编程
 *  模块不应该依赖于具体的实现，应该依赖于抽象 
 *  
 *  2、从测试的角度来说，使用了仓储模式，更有利于编写测试代码
 *  如果直接实现，那么单元测试将很难进行
 *  而如果使用了仓储模式，可以通过继承仓储接口的方式 来编写测视用的仓储测试类
 *  更易于单元测试的进行
 *  （教材P203 17.6.7 - 仓储模式的优点）
 *  
 *  如何在项目中使用Dapper ？ 
 *  （1）使用nuget包管理器 安装 Dapper
 *  （2）使用nuget包管理器 安装 System.Data.SqlClient
 */ 