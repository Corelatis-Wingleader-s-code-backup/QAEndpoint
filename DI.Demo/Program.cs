using System;

namespace DI.Demo {
    class DataRepository : IDataReopository {
        public int GetAnswer(int n) {
            return n + 1;
        }
    }
    class DataRepositoryEx : IDataReopository {
        public int GetAnswer(int n) {
            return n * n;
        }
    }
    class Controller {
        //因为Controller类想要完成自己的功能，需要依赖DataRepository类
        //所以DataRepository类是Controller类的一个依赖项
        // -> 依赖于一个实现 而非一个抽象 业务发生变化时，Controller类中的
        //的代码 需要进行修改
        public int GetAnswerSingle(int number) {
            //Controller类自己去初始化自己的依赖项
            var DataRepository = new DataRepositoryEx();
            return DataRepository.GetAnswer(number);
        }
    }

    interface IDataReopository {
        int GetAnswer(int number);
    }

    class DI_Controller {
        private IDataReopository repository_ {get;init;}
        public DI_Controller(IDataReopository repository) {
            //外部给什么就用什么 自己不去初始化自己的依赖项了
            repository_ = repository;
        }
        public int GetAnswerSingle(int number) {
            return repository_.GetAnswer(number);
        }
    }

    class Program {
        static void Main(string[] args) {
            //在外部初始化依赖，并创建Controller实例的工作在
            //.NET 5中，是由.NET框架自己实现的
            DI_Controller controller1 = new(new DataRepository());
            DI_Controller controller2 = new(new DataRepositoryEx());
            controller1.GetAnswerSingle(10);
            controller2.GetAnswerSingle(10);
        }
    }
}
